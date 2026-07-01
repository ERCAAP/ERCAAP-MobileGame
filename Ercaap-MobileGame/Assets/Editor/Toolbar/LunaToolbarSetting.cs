using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace LunaGames.Editor
{
    [CreateAssetMenu(fileName = "Toolbar Setting", menuName = "Editor Settings/Toolbar Setting")]
    public class LunaToolbarSetting : ScriptableObject
    {
        public List<SceneData> SceneList;
        public float maxGameSpeed;
    }
    [System.Serializable] public class SceneData {
        public string DisplayName;
        [FilePath] public string ScenePath;
    }
}
