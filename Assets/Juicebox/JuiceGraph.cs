using System;
using Juicebox.Nodes;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Juicebox
{
    public class JuiceGraph : ScriptableObject
    {
        [NonSerialized] public string AssetPath;
        public Nodes.EntryPoint EntryNode;

        public void InitialiseNew()
        {
            EntryNode = new EntryPoint();
        }
    }

    // [CustomEditor(typeof(JuiceGraph))]
    // public class JuiceGraphEditor : Editor
    // {
    //     private static Texture2D _Icon;
    //
    //     public override void OnInspectorGUI()
    //     {
    //         Debug.Log("GUI");
    //         base.OnInspectorGUI();
    //     }
    //
    //     public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    //     {
    //         if (_Icon == null)
    //         {
    //             _Icon = Resources.Load<Texture2D>("JuiceGraph Icon");
    //         }
    //         return _Icon;
    //     }
    // }


}
