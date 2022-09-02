using System;
using UnityEditor.Experimental.GraphView;

namespace Juicebox.Nodes
{
    public sealed class EntryPoint : JuiceNode
    {
        public int testInt { get; set; } = 5;
        public EntryPoint() : base()
        {
            title = "Entry Point";
            Port outputFlow = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, null);
            outputFlow.portName = "Next";
            outputContainer.Add(outputFlow);
            capabilities &= ~Capabilities.Deletable;
        }
    }
}