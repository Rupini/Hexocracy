using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Hexocracy.CustomEditor
{
    public static class HexEditorMenu
    {
        [MenuItem("HexEditor/Map Generator")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<MapGeneratorWindow>("Map Generator");
        }

        [MenuItem("HexEditor/Refresh Map")]
        public static void RefreshMap()
        {
            GameObject.FindObjectsOfType<MonoBehaviour>().ToList().ForEach(obj =>
                {
                    if (obj.GetType().IsDefined(typeof(ExecuteInEditMode), true))
                    {
                        var awakeMethod = obj.GetType().GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        
                        if (awakeMethod != null)
                        {
                            Debug.Log(obj.name + " Refreshed");
                            awakeMethod.Invoke(obj, null);
                        }
                    }
                });
        }
    }
}
