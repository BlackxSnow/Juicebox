using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Juicebox.Editor
{
    [ScriptedImporter(1, JuiceGraphExtension)]
    public class JuiceGraphImporter : ScriptedImporter
    {
        public const string JuiceGraphExtension = "JuiceGraph";
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var data = ScriptableObject.CreateInstance<JuiceGraph>();

            string text = File.ReadAllText(ctx.assetPath);
            data = JsonConvert.DeserializeObject<JuiceGraph>(text, new JsonSerializerSettings() {TypeNameHandling = TypeNameHandling.Auto, MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead});
            data.AssetPath = ctx.assetPath;
            var icon = Resources.Load<Texture2D>("JuiceGraphData Icon");
            ctx.AddObjectToAsset("main", data, icon);
            ctx.SetMainObject(data);
        }
    }
    
    // [CustomEditor(typeof(JuiceGraphImporter))]
    // public class JuiceGraphImporterEditor : ScriptedImporterEditor
    // {
    //     public override void OnInspectorGUI()
    //     {
    //         
    //     }
    // }
}
