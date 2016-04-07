#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        [MenuItem("HexEditor/Show Raw Prototypes")]
        public static void ShowRawPrototypes()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.Namespace.Contains(ProjectInfo.MAIN_NAMESPACE));
            var rawAttributeType = typeof(RawPrototypeAttribute);

            foreach(var type in types)
            {
                if(type.IsDefined(rawAttributeType, false))
                {
                    Debug.LogWarning("Class " + type.FullName + " is Raw Prototype");
                }
                else
                {
                    foreach(var method in type.GetMethods())
                    {
                        if(method.IsDefined(rawAttributeType, false))
                        {
                            Debug.LogWarning("Class " + type.FullName + " has Raw Prototype mehtod " + method.Name);
                        }
                    }

                    foreach (var property in type.GetProperties())
                    {
                        if (property.IsDefined(rawAttributeType, false))
                        {
                            Debug.LogWarning("Class " + type.FullName + " has Raw Prototype properties " + property.Name);
                        }
                    }

                    foreach (var field in type.GetFields())
                    {
                        if (field.IsDefined(rawAttributeType, false))
                        {
                            Debug.LogWarning("Class " + type.FullName + " has Raw Prototype field " + field.Name);
                        }
                    }
                }
            }
        }
    }
}

#endif