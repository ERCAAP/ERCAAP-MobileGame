using System.Collections.Generic;
using UnityEngine;

namespace LunaGames.Editor
{
    [CreateAssetMenu(fileName = "Asset Manager Setting", menuName = "Editor Settings/Asset Manager Setting")]
    public class AssetManagerSetting : ScriptableObject
    {
        public List<Category> Categories;
    }
    [System.Serializable]
    public class Category
    {
        public string Name;
        public string URL;
        public string UserName;
        public string Password;
    }
}
