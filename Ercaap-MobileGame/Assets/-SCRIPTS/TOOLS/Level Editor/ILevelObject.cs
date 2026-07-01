using UnityEngine;

namespace LunaGames.Tools.LevelEditor
{
    public interface ILevelObject
    {
        public string SaveParameters();
        public void LoadParameters(string json);
    }
}