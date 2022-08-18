using System;
using System.Collections.Generic;
using Juicebox.Editor.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Juicebox.Editor
{
    public class JuiceGraphView : GraphView
    {
        private JuiceNode _EntryPoint;
        public JuiceNode EntryPoint => _EntryPoint;
        
        public JuiceGraphView()
        {
            styleSheets.Add(Resources.Load<StyleSheet>("Style/JuiceGraph"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();
            
            AddElement(GenerateEntryPoint());

            var shakeNode = new ShakeTranslation();
            shakeNode.SetPosition(new Rect(400, 200, 100, 150));
            AddElement(shakeNode);
        }

        private JuiceNode GenerateEntryPoint()
        {
            _EntryPoint = new JuiceNode
            {
                title = "Start",
            };
            _EntryPoint.SetPosition(new Rect(100, 200, 100, 150));
            Port port = GeneratePort(_EntryPoint, Direction.Output, Port.Capacity.Multi);
            port.portName = "Next";
            _EntryPoint.outputContainer.Add(port);

            _EntryPoint.RefreshExpandedState();
            _EntryPoint.RefreshPorts();
            return _EntryPoint;
        }

        public void AddNode(Type nodeType, Vector2 position)
        {
            var node = (JuiceNode)Activator.CreateInstance(nodeType);
            node.SetPosition(new Rect(position, new Vector2(200, 150)));
            AddElement(node);
        }
        
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var validPorts = new List<Port>();
            ports.ForEach((port) =>
            {
                if (port != startPort && port.node != startPort.node && startPort.direction != port.direction) validPorts.Add(port);
            });
            return validPorts;
        }

        private Port GeneratePort(JuiceNode node, Direction direction, Port.Capacity capacity = Port.Capacity.Single)
        {
            return node.InstantiatePort(Orientation.Horizontal, direction, capacity, null);
        }
    }
}
