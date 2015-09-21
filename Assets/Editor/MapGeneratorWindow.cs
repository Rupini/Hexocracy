using System;
using UnityEngine;
using UnityEditor;
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


        [MenuItem("HexEditor/Map Generator")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<MapGeneratorWindow>("Map Generator");
            //SceneView.onSceneGUIDelegate += (sv) => { Debug.Log("hello!"); };
        }

        private MapGenType genType;
        private HexEditor prototypeHex;
        private int circleCount;
        private Transform hexContainer;

        private void OnGUI()
        {
            prototypeHex = (HexEditor)EditorGUILayout.ObjectField("Hex Prototype ", prototypeHex, typeof(HexEditor), true);

            hexContainer = (Transform)EditorGUILayout.ObjectField("Hex Container ", hexContainer, typeof(Transform), true);

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

            if (!hexContainer)
            {
                hexContainer = GameObject.Find("HexContainer").transform;
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
            MapFactory factory = new MapFactory(prototypeHex, hexContainer, circleCount);
            factory.Generate();
        }

    }
}
