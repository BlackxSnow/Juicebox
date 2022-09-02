using System.Reflection;
using Newtonsoft.Json;

namespace Juicebox.Utility
{
    public static class Json
    {
        public static void WriteMember(this JsonWriter writer, object obj, FieldInfo field)
        {
            writer.WritePropertyName(field.Name);
            writer.WriteValue(field.GetValue(obj));
        }
        public static void WriteMember(this JsonWriter writer, object obj, PropertyInfo property)
        {
            writer.WritePropertyName(property.Name);
            writer.WriteValue(property.GetValue(obj));
        }

        public static void ReadValue(this JsonReader reader, object obj, FieldInfo field)
        {
            // property.SetValue();
        }
    }
}