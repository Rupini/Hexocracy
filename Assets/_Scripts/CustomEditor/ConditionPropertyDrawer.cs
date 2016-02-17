
#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
#else
using System;
#endif

namespace Hexocracy.CustomEditor
{
#if UNITY_EDITOR
    public class ConditionAttribute : PropertyAttribute
    {
        public string fieldName { get; private set; }
        public string fieldValue { get; private set; }
        public bool hiden { get; private set; }

        public ConditionAttribute(bool hiden, string fieldName, string fieldValue)
        {
            this.hiden = hiden;
            this.fieldName = fieldName;
            this.fieldValue = fieldValue;
        }
    }



    [CustomPropertyDrawer(typeof(ConditionAttribute))]
    public class ConditionPropertyDrawer : PropertyDrawer
    {
        private ConditionAttribute _cachedAttribute;
        private string _pathOnly;

        private ConditionAttribute cachedAttribute
        {
            get
            {
                if (_cachedAttribute == null)
                {
                    _cachedAttribute = (ConditionAttribute)attribute;
                }

                return _cachedAttribute;
            }
        }

        private string GetPathOnly(string fullPath)
        {
            if (_pathOnly == null)
            {
                var items = fullPath.Split('.');
                _pathOnly = "";
                for (int i = 0; i < items.Length - 1; i++)
                    _pathOnly += items[i] + '.';
            }

            return _pathOnly;
        }


        private bool CheckCondition(SerializedProperty property)
        {
            var pathOnly = GetPathOnly(property.propertyPath);
            var checkingProperty = property.serializedObject.FindProperty(pathOnly + cachedAttribute.fieldName);

            if (checkingProperty != null)
            {
                switch (checkingProperty.propertyType)
                {
                    case SerializedPropertyType.Boolean:
                        return checkingProperty.boolValue.ToString().ToLower() == cachedAttribute.fieldValue;
                    case SerializedPropertyType.Enum:
                        return checkingProperty.enumNames[checkingProperty.enumValueIndex] == cachedAttribute.fieldValue;
                }

            }

            return false;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {

            if (CheckCondition(property) || !cachedAttribute.hiden)
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }
            else
            {
                return 0;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var checkResult = CheckCondition(property);
            if (checkResult || !cachedAttribute.hiden)
            {
                if (!checkResult) GUI.enabled = false;
                EditorGUI.PropertyField(position, property, label, true);
                if (!checkResult) GUI.enabled = true;
            }
        }
    }
#else

    public class ConditionAttribute(string fieldName, string fieldValue) : Attribute
    {

    }
#endif
}

