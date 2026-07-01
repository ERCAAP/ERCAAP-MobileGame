using UnityEngine;
using UnityEngine.UIElements;

namespace LunaGames.Editor
{
    [System.Serializable]
    public class ComparatorElement
    {
        public ComparatorElement(Material CurrentMaterial, Material TargetMaterial)
        {
            this.CurrentMaterial = CurrentMaterial;
            this.TargetMaterial = TargetMaterial;
        }
        public Material CurrentMaterial;
        public Material TargetMaterial;
    }
}