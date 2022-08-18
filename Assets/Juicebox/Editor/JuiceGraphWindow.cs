using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Graphs;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Juicebox.Editor
{
    public class JuiceGraphWindow : EditorWindow
    {
        private static string _JuiceboxPath;

        private static JuiceGraphWindow _CurrentWindow;
        private static JuiceGraphView _GraphView;

        private static JuiceSearchProvider _SearchProvider;
        
        [MenuItem("Juicebox/JuiceGraph")]
        public static void OpenWindow()
        {
            _CurrentWindow = GetWindow<JuiceGraphWindow>();
            _CurrentWindow.titleContent = new GUIContent("Juice Graph");
        }

        private void OnEnable()
        {
            _CurrentWindow = this;
            InitialiseData();
            InitialiseGraph();
            InitialiseToolbar();
            
            _SearchProvider = CreateInstance<JuiceSearchProvider>();
            InitialiseInput();
        }

        private void InitialiseData()
        {
            _JuiceboxPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)));
            Assert.IsNotNull(_JuiceboxPath);
        }

        private void InitialiseGraph()
        {
            _GraphView = new JuiceGraphView();
            _GraphView.StretchToParentSize();
            rootVisualElement.Add(_GraphView);
        }

        private void InitialiseInput()
        {
            rootVisualElement.RegisterCallback<KeyDownEvent>(OnKeyDown);
        }
        
        private void InitialiseToolbar()
        {
            var toolbar = new Toolbar();
            // var createNode = new Button(() =>
            // {
            //     
            // });
            // createNode.text = "New Node";
            // toolbar.Add(createNode);
            
            rootVisualElement.Add(toolbar);
        }

        private void OnKeyDown(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.A && evt.modifiers.HasFlag(EventModifiers.Shift))
            {
                _SearchProvider.Initialise(_GraphView, this);
                SearchWindow.Open(new SearchWindowContext(evt.imguiEvent.mousePosition + position.position),
                    _SearchProvider);
            }
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_GraphView);
        }
        
        
    }

    public class JuiceSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        private JuiceGraphView _GraphView;
        private JuiceGraphWindow _Window;

        public void Initialise(JuiceGraphView view, JuiceGraphWindow window)
        {
            _GraphView = view;
            _Window = window;
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var searchTree = new List<SearchTreeEntry> { new SearchTreeGroupEntry(new GUIContent("Create Node")) };
            searchTree.AddRange(AppDomain.CurrentDomain.GetAssemblies().SelectMany(s =>
            {
                IEnumerable<Type> types = s.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(JuiceNode)));
                return types.Select(t => new SearchTreeEntry(new GUIContent(t.Name)){ level = 1, userData = t });
            }));
            return searchTree;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            Vector2 windowMousePosition = _Window.rootVisualElement.contentContainer.ChangeCoordinatesTo(
                _Window.rootVisualElement, context.screenMousePosition - _Window.position.position);
            Vector2 graphMousePosition = _GraphView.contentViewContainer.WorldToLocal(windowMousePosition);
            _GraphView.AddNode(searchTreeEntry.userData as Type, graphMousePosition);
            return true;
        }
    }
}
