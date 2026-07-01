using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LunaGames.Editor
{
    [CustomPropertyDrawer(typeof(ComparatorElement))]
    public class ComparatorDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement UI = new VisualElement();

            ObjectField CurrentMaterial = UI.Q<ObjectField>(nameof(CurrentMaterial));
            CurrentMaterial.BindProperty(property.FindPropertyRelative(nameof(CurrentMaterial)));

            ObjectField TargetMaterial = UI.Q<ObjectField>(nameof(TargetMaterial));
            TargetMaterial.BindProperty(property.FindPropertyRelative(nameof(TargetMaterial)));

            return UI;
        }
    }
}
