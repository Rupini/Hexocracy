#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;
using Hexocracy.Core;

using UObject = UnityEngine.Object;
using Hexocracy.Systems;

namespace Hexocracy.CustomEditor
{
    public class MapGeneratorWindow : EditorWindow
    {
        private enum MapGenType : byte
        {
            Nothing = 0,
            Circle = 1,
            Rectangle = 2
        }

        private MapGenType genType;
        private HexEditor prototypeHex;
        private int circleCount;
        private Transform hexContainer;

        private void OnGUI()
        {
            prototypeHex = (HexEditor)EditorGUILayout.ObjectField("Hex Prototype ", prototypeHex, typeof(HexEditor), true);

            GUI.enabled = false;
            hexContainer = (Transform)EditorGUILayout.ObjectField("Hex Container ", hexContainer, typeof(Transform), true);
            GUI.enabled = true;

            genType = (MapGenType)EditorGUILayout.EnumPopup("Generation Type", genType);

            switch (genType)
            {
                case MapGenType.Circle:
                    circleCount = EditorGUILayout.IntSlider("Circle count ", circleCount, 2, 20);

                    if (GUILayout.Button("Generate")) Generate();

                    if (GUILayout.Button("DestroyAll")) Destroy();

                    break;
            }

            if (!prototypeHex)
            {
                prototypeHex = RM.GetEditorPrefab<HexEditor>("Hex");
            }

            if (!hexContainer)
            {
                var go = GameObject.Find("Hex Container");
                if (go)
                {
                    hexContainer = go.transform;
                }
            }
        }

        private void Destroy()
        {
            if (hexContainer) DestroyImmediate(hexContainer.gameObject);
        }

        private void Generate()
        {
            var factory = new MapGenerator(prototypeHex, circleCount);
            factory.Generate();
            hexContainer = factory.GetContainer();
        }

    }
}

#endif