using UnityEditor;
using UnityEngine;

namespace FPSProject.Impl.Configs.Extensions
{
    [CustomEditor(typeof(ScenesConfig))]
    public class ScenesConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var config = (ScenesConfig) target;
 
            if(GUILayout.Button("Save scenes data", GUILayout.Height(40)))
            {
                config.SaveScenesData();
                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssets();
            }
         
        }
    }
}