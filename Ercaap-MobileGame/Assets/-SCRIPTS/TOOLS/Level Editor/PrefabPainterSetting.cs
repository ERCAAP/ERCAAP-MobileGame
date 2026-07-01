using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace LunaGames.Tools.LevelEditor
{
    [CreateAssetMenu(fileName = "Prefab Painter", menuName = "Editor Settings/Painter Setting")]
    public class PrefabPainterSetting : SerializedScriptableObject
    {
        public Dictionary<string, string> Folders = new Dictionary<string, string>();
        [FolderPath] public string templateFolder;
    }
}
