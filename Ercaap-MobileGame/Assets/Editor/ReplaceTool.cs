using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine;

namespace LunaGames.Editor
{
    public class ReplaceTool : OdinEditorWindow
    {
        public static void OpenWindow() {
            
            ReplaceTool Window = GetWindow<ReplaceTool>(title: "Replace Tool");
            Window.titleContent = new GUIContent("Replace Tool");
            Window.minSize = new Vector2(300, 300);
            Window.Show();

        }

        //[TabGroup("Replace Objects")]
        [ShowInInspector] 
        public Transform[] SelectedObjects => Selection.transforms;

    
        [AssetSelector]
        [AssetsOnly]
        public GameObject replaceWith;

        public enum ToggleEnum { Off, On }

        //[TabGroup("Replace Objects")]
        [EnumToggleButtons] 
        public ToggleEnum CustomRotation;

        //[TabGroup("Replace Objects")]
        [EnableIf("@CustomRotation == ToggleEnum.On")]
        public Quaternion rotation;

        //[TabGroup("Replace Objects")]
        [Button(ButtonHeight = 50)]
        public void Replace()
        {
            if (replaceWith == null || Selection.count <= 0) return;
            
            foreach (Transform selected in Selection.transforms)
            {
                GameObject processedGO = replaceWith;
                switch (PrefabUtility.GetPrefabAssetType(replaceWith))
                {
                    case PrefabAssetType.Regular:
                        processedGO = (GameObject)PrefabUtility.InstantiatePrefab(processedGO);
                        PrefabUtility.SetPropertyModifications(processedGO, PrefabUtility.GetPropertyModifications(replaceWith));
                        break;
                    case PrefabAssetType.NotAPrefab:
                        processedGO = Instantiate(replaceWith);
                        break;
                    default:
                        break;
                }

                Undo.RegisterCreatedObjectUndo(processedGO, "Prefab Created");

                processedGO.transform.position = selected.position;
                processedGO.transform.rotation = CustomRotation == ToggleEnum.On ? rotation : selected.rotation;
                processedGO.transform.localScale = selected.localScale;
                if (selected.parent != null) processedGO.transform.parent = selected.parent;
            }

            Selection.gameObjects.ForEach((go) => Undo.DestroyObjectImmediate(go));
        }

        /*[TabGroup("Replace Fonts")]
        [OnValueChanged(nameof(ReplaceFonts))] 
        public Font Font;

        [TabGroup("Replace Fonts")]
        [Button] 
        public void ReplaceFonts()
        {
            foreach (Text text in FindObjectsOfType<Text>())
            {
                text.font = Font;
            }
        }*/
    }
}