using UnityEngine;

namespace LunaGames.Tools
{
    public class InfoBox : MonoBehaviour
    {
        public string Info;
        public bool editMode = false;

        [ContextMenu("Toggle Edit")] public void ToggleEditMode() => editMode = !editMode;
    }
}
