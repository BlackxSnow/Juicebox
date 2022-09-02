using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Juicebox.Nodes;
using Juicebox.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Juicebox.Serialisation.Converters
{
    public class JsonJuiceNode : JsonConverter<JuiceNode>
    {
        /// <summary>
        /// Return a collection of fields and properties declared on the provided type.
        /// Either returns members with [DataMember] if the type has [DataContract] or returns all public properties.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static List<MemberInfo> GetSerialisableMembers(Type type)
        {
            var serialisableMembers = new List<MemberInfo>();

            if (type.GetCustomAttribute(typeof(DataContractAttribute)) != null)
            {
                serialisableMembers.AddRange(type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance)
                    .Where(f => f.GetCustomAttribute<DataMemberAttribute>() != null));
                serialisableMembers.AddRange(type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance)
                    .Where(p => p.GetCustomAttribute<DataMemberAttribute>() != null));
            }
            else
            {
                serialisableMembers.AddRange(type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance));
            }

            return serialisableMembers;
        }
        
        public override void WriteJson(JsonWriter writer, JuiceNode value, JsonSerializer serializer)
        {
            // Type and GUID data prefix
            writer.WriteStartObject();
            writer.WritePropertyName("$type");
            writer.WriteValue(value.GetType().ToString());
            writer.WritePropertyName("GUID");
            writer.WriteValue(value.GUID);

            List<MemberInfo> toSerialise = GetSerialisableMembers(value.GetType());

            // Write all relevant fields / properties from derived type.
            foreach (MemberInfo member in toSerialise)
            {
                switch (member)
                {
                    case FieldInfo field:
                        writer.WriteMember(value, field);
                        break;
                    case PropertyInfo property:
                        writer.WriteMember(value, property);
                        break;
                }
            }
        }

        public override JuiceNode ReadJson(JsonReader reader, Type objectType, JuiceNode existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var node = (hasExistingValue ? existingValue : Activator.CreateInstance(objectType)) as JuiceNode;
            JObject data = JObject.Load(reader);

            string guidHex = data.Value<string>("GUID") ?? throw new DataException("Node has no GUID.");
            node!.Reinitialise(Guid.TryParse(guidHex, out Guid guid) ? guid : throw new InvalidDataException("Node GUID was not valid GUID."));
            
            List<MemberInfo> toDeserialise = GetSerialisableMembers(objectType);

            // Read relevant fields and properties from derived type (actual type)
            foreach (MemberInfo member in toDeserialise)
            {
                switch (member)
                {
                    case FieldInfo field:
                    {
                        if (!data.TryGetValue(field.Name, out JToken value)) continue;
                        field.SetValue(node, value.Value<object>());
                        break;
                    }
                    case PropertyInfo property:
                    {
                        if (!data.TryGetValue(property.Name, out JToken value)) continue;
                        var e = value.ToObject(property.PropertyType);
                        property.SetValue(node, e);
                        break;
                    }
                }
            }
            
            return node;
        }
    }
}