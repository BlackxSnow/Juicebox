using UnityEditor.Experimental.GraphView;

namespace Juicebox.Editor.Nodes
{
    public sealed class ShakeRotation : JuiceNode
    {
        public ShakeRotation() : base()
        {
            title = "Shake Rotation";
            Port inputFlow = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, null);
            inputFlow.portName = "Execute";
            Port outputFlow = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, null);
            outputFlow.portName = "Next";
            inputContainer.Add(inputFlow);
            outputContainer.Add(outputFlow);
        }
    }
}
