using System;
using UnityEditor.Experimental.GraphView;

namespace Juicebox.Editor
{
    
    public class JuiceNode : Node
    {
        private Guid _GUID;
        public Guid GUID => _GUID;
        
        public JuiceNode()
        {
            _GUID = Guid.NewGuid();
        }
    }
}
