using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Systems
{
    /// <summary>
    /// Resource Manager
    /// </summary>
    public class RM
    {
        #region Static

        private const string PREFAB_PATH = "Prefabs/Play/";
        private const string EDITOR_PREFAB_PATH = "Prefabs/Editor/";
        private const string MATERIAL_PATH = "Models/Materials/";
        private const string TEXTURE_PATH = "Models/Texture/";

        public static T GetPrefab<T>(string prefabName) where T : Behaviour
        {
            return Resources.Load<T>(PREFAB_PATH + prefabName);
        }

        public static T GetEditorPrefab<T>(string prefabName) where T : Behaviour
        {
            return Resources.Load<T>(EDITOR_PREFAB_PATH + prefabName);
        }

        public static T InstantiatePrefab<T>(string prefabName) where T : Behaviour
        {
            return GameObject.Instantiate<T>(Resources.Load<T>(PREFAB_PATH + prefabName));
        }

        public static T InstantiateEditorPrefab<T>(string prefabName) where T : Behaviour
        {
            return GameObject.Instantiate<T>(Resources.Load<T>(EDITOR_PREFAB_PATH + prefabName));
        }

        public static Material LoadMaterial(string materialName)
        {
            return Resources.Load<Material>(MATERIAL_PATH + materialName);
        }

        public static Texture LoadTexture(string textureName)
        {
            return Resources.Load<Texture>(TEXTURE_PATH + textureName);
        }

        #endregion

        private RM() { }
    }
}
