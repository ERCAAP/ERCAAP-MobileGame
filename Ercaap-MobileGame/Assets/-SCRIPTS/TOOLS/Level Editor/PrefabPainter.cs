using LunaGames.Main;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LunaGames.Tools.LevelEditor
{
#if UNITY_EDITOR
    public class PrefabPainter : OdinEditorWindow
    {
        private string settingsPath = "Assets/-SETTINGS/Prefab Painter.asset";
        
        private PrefabPainterSetting Settings;
        public enum States { Off, Place, Delete, Select }
        public enum Folders { Prefabs, Templates, Other }
        public States state;
        public int folder;
        public string[] stateNames = { "OFF", "Place Mode", "Delete Mode", "Select Mode" };
        public string[] folderNames => Settings.Folders.Keys.ToArray();
        private Vector2 scrollPos;
        [SerializeField] float placementHeight = 0;
        [SerializeField] float heightStep = 1;
        [SerializeField] Vector3 snapResolution = Vector3.one;
        [SerializeField] bool customSettings;
        Transform[] proccessedTransforms;
        [SerializeField] public List<Transform> selectedTransforms = new List<Transform>();
        [SerializeField] public List<GameObject> prefabsList = new List<GameObject>();
        [SerializeField] private int prefabIndex;
        private LevelEditor levelEditor;
        private LevelManager levelManager;
        private string levelNo;
        private string prefabName;
        private string prefabPath;
        private GameObject previewGO;
        private int cellSize = 2;

        //[MenuItem("Luna Games/Prefab Painter")]
        public static void OpenWindow() => GetWindow<PrefabPainter>(title: "Prefab Painter").Show();
        private void GetSettings() {
            if (AssetDatabase.LoadAssetAtPath(settingsPath, typeof(PrefabPainterSetting)) as PrefabPainterSetting == null) {
                PrefabPainterSetting NewSetting = ScriptableObject.CreateInstance<PrefabPainterSetting>();
                AssetDatabase.CreateAsset(NewSetting, settingsPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
            Settings = AssetDatabase.LoadAssetAtPath(settingsPath, typeof(PrefabPainterSetting)) as PrefabPainterSetting;
        }
        public void SelectPrefabRoots()
        {
            for (int i = 0; i < proccessedTransforms.Length; i++)
            {
                if (PrefabUtility.IsPartOfPrefabInstance(proccessedTransforms[i]))
                {
                    GameObject root = PrefabUtility.GetOutermostPrefabInstanceRoot(proccessedTransforms[i]);
                    proccessedTransforms[i] = root.transform;
                }
            }
        }
        public void CreateTemplate(string TemplateName)
        {
            proccessedTransforms = selectedTransforms.ToArray();
            ClearSelectedCache();
            SelectPrefabRoots();
            Transform parent = proccessedTransforms[0].parent;
            GameObject prefabTemplate = new GameObject(TemplateName);
            prefabTemplate.transform.position = proccessedTransforms[0].position;
            string localPath = $"{Settings.templateFolder}/{prefabTemplate.name}.prefab";
            for (int i = 0; i < proccessedTransforms.Length; i++)
            {
                proccessedTransforms[i].SetParent(prefabTemplate.transform);
            }
            PrefabUtility.SaveAsPrefabAsset(prefabTemplate, localPath);
            for (int i = 0; i < proccessedTransforms.Length; i++)
            {
                proccessedTransforms[i].SetParent(parent);
            }
            DestroyImmediate(prefabTemplate);
        }
        #region UI
        
        private void HeaderUI() {
            state = (States)GUILayout.SelectionGrid((int)state, stateNames, 4, EditorStyles.toolbarButton);
            if (state == States.Place) {
                folder = EditorGUILayout.Popup("Select Folder:", folder, folderNames, EditorStyles.toolbarDropDown);
                prefabPath = Settings.Folders[folderNames[folder]];
                RefreshPalette();
            }
        }
        private void PrefabPaletteUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            List<GUIContent> paletteIcons = new List<GUIContent>();
            GUILayout.BeginHorizontal();
            for (int i = 0; i < prefabsList.Count; i++)
            {
                if (i % 4 == 0) {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                GUILayout.BeginVertical(GUILayout.Width(position.width / 4));
                Texture2D texture = AssetPreview.GetAssetPreview(prefabsList[i]);
                paletteIcons.Add(new GUIContent(texture, prefabsList[i].name));
                if (GUILayout.Button(texture)) {
                    prefabIndex = i;
                    if (previewGO != null && previewGO.transform.childCount > 0) DestroyImmediate(previewGO.transform.GetChild(0).gameObject);
                } 

                GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(prefabsList[i].name);
                    GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }
        private void SelectUI() {
            EditorGUILayout.BeginHorizontal();
            prefabName = EditorGUILayout.TextField("Template Name:", prefabName, EditorStyles.toolbarTextField, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Create Teplate", EditorStyles.toolbarButton, GUILayout.Width(100)))
            {
                CreateTemplate(prefabName);
            }
            EditorGUILayout.EndHorizontal();
        }
        private void PlaceUI()
        {
            GUILayout.BeginHorizontal();
            customSettings = EditorGUILayout.Toggle("Custom Settings", customSettings);
            EditorGUI.BeginDisabledGroup(customSettings == false);
            if (GUILayout.Button("Reset", GUILayout.Width(100)))
            {
                placementHeight = 0;
                heightStep = 1;
                snapResolution = Vector3.one;
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(customSettings == false);
            placementHeight = EditorGUILayout.FloatField("Place Height:", placementHeight);
            snapResolution.x = EditorGUILayout.Slider("Snap X:", snapResolution.x, 0.1f, 1f);
            snapResolution.z = EditorGUILayout.Slider("Snap Z:", snapResolution.z, 0.1f, 1f);
            heightStep = EditorGUILayout.Slider("Height Step:", heightStep, 0f, 1f);

            
            EditorGUI.EndDisabledGroup();
        }
        protected override void OnGUI()
        {
            //base.OnGUI(); //For Debug
            GetSettings();

            HeaderUI();

            if (state != States.Select) ClearSelectedCache();

            if (state == States.Select)
            {
                SelectUI();
            }
            if (state == States.Place)
            {
                PlaceUI();
                PrefabPaletteUI();
            }
            
        }
        #endregion

        private void CheckParameters() {
            if (previewGO.transform.childCount > 0)
            {
                LevelObjectAttributes attr = previewGO.GetComponentInChildren<LevelObjectAttributes>();
                if (attr == null) return;
                placementHeight = !customSettings ? attr.Height : placementHeight ;
                snapResolution.x = !customSettings ? attr.Tile.x : snapResolution.x;
                snapResolution.z = !customSettings ? attr.Tile.y : snapResolution.z;
            }
        }
        private void OnSceneGUI(SceneView sceneView)
        {
            if (levelEditor == null) levelEditor = FindObjectOfType<LevelEditor>();

            switch (state)
            {
                case States.Place:
                    {
                        selectedTransforms.Clear();
                        if (previewGO == null) previewGO = new GameObject("Preview");
                        Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

                        Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.y / guiRay.direction.y);
                        mousePosition = new Vector3(Snap(mousePosition.x, snapResolution.x), Snap(mousePosition.y, snapResolution.y), Snap(mousePosition.z, snapResolution.z));
                        mousePosition.y = placementHeight;

                        CheckParameters();

                        DisplayVisualHelp(mousePosition);
                        PlaceItemHandler(mousePosition);
                        break;
                    }

                case States.Delete:
                    if (previewGO != null) DestroyImmediate(previewGO);

                    DeleteItemHandler();
                    break;
                case States.Select:
                    if (previewGO != null) DestroyImmediate(previewGO);

                    SelectItemHandler();
                    break;
                default:
                    if (previewGO != null) DestroyImmediate(previewGO);
                    break;
            }
            sceneView.Repaint();
        }
        private void PlaceItemHandler(Vector3 cellCenter)
        {
            if (previewGO.transform.childCount <= 0)
            {
                GameObject PreviewItem = PrefabUtility.InstantiatePrefab(prefabsList[prefabIndex], previewGO.transform) as GameObject;
                PreviewItem.transform.localPosition = Vector3.zero;
            }
            if (Event.current.type == EventType.ScrollWheel)
            {
                Event.current.Use();
                placementHeight += Event.current.delta.y >= 0 ? heightStep : -heightStep;
            }
            if (Event.current.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(0); // Consume the event
            }



            previewGO.transform.position = cellCenter;

            if (prefabIndex < prefabsList.Count && Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                //placementHeight = 0;
                // Create the prefab instance while keeping the prefab link
                GameObject prefab = prefabsList[prefabIndex];
                GameObject gameObject = PrefabUtility.InstantiatePrefab(prefab, levelEditor.levelParent) as GameObject;
                gameObject.transform.position = cellCenter;
                Undo.RegisterCreatedObjectUndo(gameObject, $"{gameObject.name} Created");
            }
        }

        public float Snap(float target, float resolution) => Mathf.Round(target * 1 / resolution) * resolution;
        private void DeleteItemHandler()
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                GameObject pickedObject = HandleUtility.PickGameObject(Event.current.mousePosition, true);
                GameObject root = PrefabUtility.GetOutermostPrefabInstanceRoot(pickedObject);
                if (root == null || root.GetComponentInChildren<LevelObjectAttributes>().selectable == false) return;
                if (PrefabUtility.IsPartOfPrefabInstance(root)) Undo.DestroyObjectImmediate(root);
            }
        }
        private void SelectItemHandler()
        {
            
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                GameObject pickedObject = HandleUtility.PickGameObject(Event.current.mousePosition, true);
                HandleUtility.AddDefaultControl(0);
                if (pickedObject == null) return;
                GameObject root = PrefabUtility.GetOutermostPrefabInstanceRoot(pickedObject);
                if (root.GetComponentInChildren<LevelObjectAttributes>().selectable == false) return;
                
                if (selectedTransforms.Contains(root.transform))
                {
                    selectedTransforms.Remove(root.transform);
                    AssignLabel(root, 0);
                }
                else
                {
                    selectedTransforms.Add(root.transform);
                    AssignLabel(root, 3);
                }
            }
        }
        private void ClearSelectedCache()
        {
            selectedTransforms.ForEach((selected) => AssignLabel(selected.gameObject, 0));
            selectedTransforms.Clear();
        }
        public static void AssignLabel(GameObject g, int id)
        {
            Texture2D tex = EditorGUIUtility.IconContent($"sv_label_{id}").image as Texture2D;
            EditorGUIUtility.SetIconForObject(g, tex);
        }
        private void DisplayVisualHelp(Vector3 mousePosition)
        {
            Vector3 topLeft = mousePosition + Vector3.left * cellSize * 0.5f + Vector3.forward * cellSize * 0.5f;
            Vector3 topRight = mousePosition - Vector3.left * cellSize * 0.5f + Vector3.forward * cellSize * 0.5f;
            Vector3 bottomLeft = mousePosition + Vector3.left * cellSize * 0.5f - Vector3.forward * cellSize * 0.5f;
            Vector3 bottomRight = mousePosition - Vector3.left * cellSize * 0.5f - Vector3.forward * cellSize * 0.5f;

            // Rendering
            Handles.color = Color.green;
            Vector3[] lines = { topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft, topLeft };
            Handles.DrawLines(lines);
        }
        void OnFocus()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
            SceneView.duringSceneGui += this.OnSceneGUI;
        }
        private void RefreshPalette()
        {
            prefabsList.Clear();

            string[] prefabFiles = System.IO.Directory.GetFiles(prefabPath, "*.prefab");
            foreach (string prefabFile in prefabFiles)
                prefabsList.Add(AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }
    }

#endif
}