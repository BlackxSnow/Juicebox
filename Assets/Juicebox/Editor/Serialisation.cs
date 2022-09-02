using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace Juicebox.Editor
{
    public static class Serialisation
    {
        public static void Save(JuiceGraph asset)
        {
            File.WriteAllText(asset.AssetPath, JsonConvert.SerializeObject(asset, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            }));
        }
    }

    public class CreateJuiceGraph : EndNameEditAction
    {
        [MenuItem("Assets/Create/Juice Graph", false, 1)]
        public static void CreateGraph()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateInstance<CreateJuiceGraph>(),
                $"New Juice Graph.{JuiceGraphImporter.JuiceGraphExtension}", null, null);
        }
        
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var data = CreateInstance<JuiceGraph>();
            data.AssetPath = pathName;
            data.InitialiseNew();
            File.WriteAllText(pathName, JsonConvert.SerializeObject(data, data.GetType(), new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            }));
            AssetDatabase.Refresh();
        }
    }
}
