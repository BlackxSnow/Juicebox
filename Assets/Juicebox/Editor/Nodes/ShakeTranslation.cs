using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Juicebox.Editor.Nodes
{
    public sealed class ShakeTranslation : JuiceNode
    {

        public ShakeTranslation() : base()
        {
            title = "Shake Translation";
            Port inputFlow = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, null);
            inputFlow.portName = "Execute";
            Port outputFlow = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, null);
            outputFlow.portName = "Next";
            inputContainer.Add(inputFlow);
            outputContainer.Add(outputFlow);
        }
    }
}
