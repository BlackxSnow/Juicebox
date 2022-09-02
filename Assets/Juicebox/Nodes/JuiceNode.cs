using System;
using Newtonsoft.Json;
using UnityEditor.Experimental.GraphView;

namespace Juicebox.Nodes
{
    [JsonConverter(typeof(Serialisation.Converters.JsonJuiceNode))]
    public class JuiceNode : Node
    {
        private Guid _GUID;
        public Guid GUID => _GUID;
        
        public JuiceNode()
        {
            _GUID = Guid.NewGuid();
        }

        public void Reinitialise(Guid guid)
        {
            _GUID = guid;
        }
    }
}
