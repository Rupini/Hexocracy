using System;
using UnityEngine;
using UnityEditor;
using Hexocracy.Core;

using UObject = UnityEngine.Object;

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
                    break;
            }


            if (GUILayout.Button("Generate"))
                Generate();

            if (GUILayout.Button("DestroyAll"))
                Destroy();

            if (!prototypeHex)
            {
                prototypeHex = Resources.Load<HexEditor>("Prefabs/Hex");
            }
        }

        private void Destroy()
        {
            if (hexContainer)
                while (hexContainer.childCount > 0)
                    DestroyImmediate(hexContainer.GetChild(0).gameObject);
        }

        private void Generate()
        {
            MapFactory factory = new MapFactory(prototypeHex, circleCount);
            factory.Generate();
            hexContainer = factory.GetContainer();
        }

    }
}
