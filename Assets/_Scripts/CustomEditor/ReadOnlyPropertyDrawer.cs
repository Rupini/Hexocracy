
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#else
using System;
#endif

namespace Hexocracy.CustomEditor
{
    #if UNITY_EDITOR
    public class ReadOnlyAttribute : PropertyAttribute
    {

    }

    

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property,
                                             GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position,
                                   SerializedProperty property,
                                   GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
#else

    public class ReadOnlyAttribute : Attribute
    {

    }
#endif
}

