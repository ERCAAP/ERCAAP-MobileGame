using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace LunaGames.Editor
{
    public class Replacer : EditorWindow
    {
        [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;
        [SerializeField] private VisualTreeAsset m_ComparatorDrawer = default;
        [SerializeField] private SerializedObject thisSO;
        [SerializeField] private DropdownField ModSelector;
        [SerializeField] private VisualElement FontReplacer;
        [SerializeField] private VisualElement ItemReplacer;

        #region FontReplacerProperties
        [SerializeField] private List<TMP_Text> TextMeshComponents = new List<TMP_Text>();
        [SerializeField] private List<ComparatorElement> Comparators = new List<ComparatorElement>();
        [SerializeField] private TMP_FontAsset TargetFont;
        #endregion
        #region ItemReplacerProperties
        [SerializeField] private GameObject[] SelectedObjects = new GameObject[0];
        [SerializeField] private GameObject TargetObject;
        [SerializeField] private bool CanRotate;
        [SerializeField] private Vector3 Rotation;
        [SerializeField] private Vector3Field RotationField;
        #endregion

        public static void OpenWindow()
        {
            Replacer wnd = GetWindow<Replacer>();
            wnd.titleContent = new GUIContent("Replacer V2");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            thisSO = new SerializedObject(this);
            VisualElement UI = m_VisualTreeAsset.Instantiate();

            ModSelector = UI.Q<DropdownField>(nameof(ModSelector));

            FontReplacer = UI.Q<VisualElement>(nameof(FontReplacer));
            ItemReplacer = UI.Q<VisualElement>(nameof(ItemReplacer));

            UI.StretchToParentSize();

            #region FontSetup
            Button ScanButton = UI.Q<Button>(nameof(ScanButton));
            ScanButton.clicked += Scan;
            Button FontReplace = UI.Q<Button>(nameof(FontReplace));
            FontReplace.clicked += FontReplaceFunc;

            ObjectField TargetFont = UI.Q<ObjectField>(nameof(TargetFont));
            TargetFont.BindProperty(thisSO.FindProperty(nameof(TargetFont)));

            ListView TextList = UI.Q<ListView>(nameof(TextList));
            TextList.BindProperty(thisSO.FindProperty(nameof(TextMeshComponents)));

            ListView LS_Comparator = UI.Q<ListView>(nameof(LS_Comparator));
            LS_Comparator.BindProperty(thisSO.FindProperty(nameof(Comparators)));
            LS_Comparator.makeItem = () => m_ComparatorDrawer.Instantiate();
            #endregion
            #region FontSetup
            Button ItemReplace = UI.Q<Button>(nameof(ItemReplace));
            ItemReplace.clicked += ItemReplaceFunc;

            Toggle CanRotate = UI.Q<Toggle>(nameof(CanRotate));
            CanRotate.BindProperty(thisSO.FindProperty(nameof(CanRotate)));
            CanRotate.RegisterValueChangedCallback(ToggleRotation);

            RotationField = UI.Q<Vector3Field>(nameof(RotationField));
            RotationField.BindProperty(thisSO.FindProperty(nameof(Rotation)));

            ObjectField TargetObject = UI.Q<ObjectField>(nameof(TargetObject));
            TargetObject.BindProperty(thisSO.FindProperty(nameof(TargetObject)));

            ListView SelectedObjects = UI.Q<ListView>(nameof(SelectedObjects));
            SelectedObjects.BindProperty(thisSO.FindProperty(nameof(SelectedObjects)));
            #endregion
            root.Add(UI);
        }

        private void Update()
        {
            FontReplacer.style.display = ModSelector.value == "Font" ? DisplayStyle.Flex : DisplayStyle.None;
            ItemReplacer.style.display = ModSelector.value == "Item" ? DisplayStyle.Flex : DisplayStyle.None;
            SelectedObjects = Selection.gameObjects;
        }

        #region FontReplacerFunctions
        private void FindTextMeshPro()
        {
            TextMeshComponents.Clear();
            // Find all TextMeshPro objects in scenes
            TMP_Text[] textMeshProsInScenes = FindObjectsOfType<TMP_Text>();

            // Find all prefabs in the project
            string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab");
            List<TMP_Text> textMeshProsInPrefabs = new List<TMP_Text>();

            foreach (string guid in prefabGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                TMP_Text[] textMeshPros = prefab.GetComponentsInChildren<TMP_Text>(true);
                textMeshProsInPrefabs.AddRange(textMeshPros);
            }

            // Combine both arrays
            List<TMP_Text> allTextMeshPros = new List<TMP_Text>();
            allTextMeshPros.AddRange(textMeshProsInScenes);
            allTextMeshPros.AddRange(textMeshProsInPrefabs);
            TextMeshComponents = new List<TMP_Text>(allTextMeshPros);
        }
        public void ListMaterials()
        {
            Comparators.Clear();
            Material FallBackMaterial = TMP_Settings.defaultFontAsset.material;
            foreach (TMP_Text T in TextMeshComponents)
            {
                if (T.fontSharedMaterial == null)
                    T.fontSharedMaterial = FallBackMaterial;
                ComparatorElement newItem = new ComparatorElement(T.fontSharedMaterial, FallBackMaterial);
                if (!Comparators.Any((item) => item.CurrentMaterial == newItem.CurrentMaterial))
                    Comparators.Add(newItem);
            }
            Debug.Log(Comparators);
        }
        private void Scan()
        {
            FindTextMeshPro();
            ListMaterials();
        }
        public void FontReplaceFunc()
        {
            if (TargetFont == null)
            {
                Debug.LogError("No font asset selected");
                return;
            }
            foreach (TMP_Text T in TextMeshComponents)
            {

                Material TargetMaterial;

                try
                {
                    TargetMaterial = Comparators.First((material) => material.CurrentMaterial == T.fontSharedMaterial).TargetMaterial;
                }
                catch (System.Exception)
                {
                    TargetMaterial = TMP_Settings.defaultFontAsset.material;
                }

                Debug.Log($"Current {T.fontSharedMaterial}, Target {TargetMaterial}");
                T.fontSharedMaterial = TargetMaterial;
                T.font = TargetFont;
                T.SetMaterialDirty();

                Scene currentScene = SceneManager.GetActiveScene();
                EditorSceneManager.MarkSceneDirty(currentScene);
            }
        }
        #endregion

        #region ItemReplacerFunctions

        private void ToggleRotation(ChangeEvent<bool> evt)
        {
            RotationField.SetEnabled(evt.newValue);
        }



        public void ItemReplaceFunc()
        {
            if (TargetObject == null || Selection.count <= 0) return;

            foreach (Transform selected in Selection.transforms)
            {
                GameObject processedGO = TargetObject.gameObject;
                switch (PrefabUtility.GetPrefabAssetType(TargetObject))
                {
                    case PrefabAssetType.Regular:
                        processedGO = (GameObject)PrefabUtility.InstantiatePrefab(processedGO);
                        PrefabUtility.SetPropertyModifications(processedGO, PrefabUtility.GetPropertyModifications(TargetObject));
                        break;
                    case PrefabAssetType.NotAPrefab:
                        processedGO = Instantiate(TargetObject);
                        break;
                    default:
                        break;
                }

                Undo.RegisterCreatedObjectUndo(processedGO, "Prefab Created");

                processedGO.transform.position = selected.position;
                processedGO.transform.rotation = CanRotate ? Quaternion.Euler(Rotation) : selected.rotation;
                processedGO.transform.localScale = selected.localScale;
                if (selected.parent != null) processedGO.transform.parent = selected.parent;
            }

            foreach (GameObject go in Selection.gameObjects)
            {
                Undo.DestroyObjectImmediate(go);
            }
        }
        #endregion
    }
}
