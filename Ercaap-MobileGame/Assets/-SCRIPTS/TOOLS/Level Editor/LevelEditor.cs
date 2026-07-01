using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LunaGames.Tools.LevelEditor
{
    [HideMonoScript]
    public class LevelEditor : MonoBehaviour
    {
        private bool SaveSure;
        private bool ClearSure;

        [AssetList, HorizontalGroup("Main", Width = 130), PreviewField(120), AssetsOnly, HideLabel] public LevelData levelData;
        [HorizontalGroup("Main"), BoxGroup("Main/Right", GroupName = "$levelData")] public Transform levelParent;
        [InlineEditor(InlineEditorModes.GUIOnly, InlineEditorObjectFieldModes.CompletelyHidden, Expanded = true), ShowInInspector] public LevelData LevelData { get => levelData; set => levelData = value; }

        [HideIf(nameof(SaveSure), false), Button(ButtonHeight = 53, Icon = SdfIconType.PlayCircleFill), GUIColor(0, 1, 0), ButtonGroup("Main/Right/LevelEditor")]
        public void LoadData()
        {
            if (levelData == null) return;

            Clear();
            foreach (LevelObject obj in levelData.levelObjects)
            {
#if UNITY_EDITOR
                GameObject newObj = Application.isPlaying ? Instantiate(obj.Object, levelParent.transform) : PrefabUtility.InstantiatePrefab(obj.Object, levelParent.transform) as GameObject;
#else
            GameObject newObj = Instantiate(obj.Object, levelParent.transform) as GameObject;
#endif
                newObj.name = obj.Object.name;
                newObj.GetComponentInChildren<ILevelObject>()?.LoadParameters(obj.ParametersJSON);
                newObj.gameObject.SetActive(obj.ActiveSelf);
                newObj.transform.localPosition = obj.RootTransfrom.LocalPosition;
                newObj.transform.localRotation = obj.RootTransfrom.LocalRotation;
                newObj.transform.localScale = obj.RootTransfrom.LocalScale;

                if (newObj.transform.childCount > 0)
                {
                    newObj.transform.GetChild(0).localPosition = obj.ChildTransfrom.LocalPosition;
                    newObj.transform.GetChild(0).localRotation = obj.ChildTransfrom.LocalRotation;
                    newObj.transform.GetChild(0).localScale = obj.ChildTransfrom.LocalScale;
                }
            }
        }
        [HideIf(nameof(SaveSure), false), Button(Name = "Save Data", ButtonHeight = 53, Icon = SdfIconType.DeviceSsdFill), GUIColor(0.85f, 0.85f, 0), ButtonGroup("Main/Right/LevelEditor")]
        public void SaveDataSure() {
            SaveSure = true;
            ClearSure = false;
        }
        [ShowIf(nameof(SaveSure), false), Button(Name = "RETURN", ButtonHeight = 53, Icon = SdfIconType.ArrowLeftSquareFill), GUIColor(0.85f, 0.85f, 0), ButtonGroup("Main/Right/LevelEditor")]
        public void NotSure()
        {
            SaveSure = false;
        }
        [ShowIf(nameof(SaveSure), false), Button(Name = "CONFIRM",ButtonHeight = 53, Icon = SdfIconType.CheckSquareFill), GUIColor(0.9f, 0.6f, 0), ButtonGroup("Main/Right/LevelEditor")]
        public void SaveData()
        {
            SaveSure = false;
#if UNITY_EDITOR
            if (levelData == null) return;

            levelData.levelObjects.Clear();
            foreach (Transform obj in levelParent)
            {
                LevelObject newObject = new LevelObject();
                newObject.Object = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj.gameObject);
                newObject.ParametersJSON = obj.GetComponentInChildren<ILevelObject>()?.SaveParameters();
                newObject.ActiveSelf = obj.gameObject.activeSelf;
                newObject.RootTransfrom.LocalPosition = obj.transform.localPosition;
                newObject.RootTransfrom.LocalRotation = obj.transform.localRotation;
                newObject.RootTransfrom.LocalScale = obj.transform.localScale;

                if (obj.transform.childCount > 0)
                {
                    newObject.ChildTransfrom.LocalPosition = obj.GetChild(0).transform.localPosition;
                    newObject.ChildTransfrom.LocalRotation = obj.GetChild(0).transform.localRotation;
                    newObject.ChildTransfrom.LocalScale = obj.GetChild(0).transform.localScale;
                }

                levelData.levelObjects.Add(newObject);
            }

            EditorUtility.SetDirty(levelData);
            AssetDatabase.SaveAssets();
            
#endif
        }

        [HideIf(nameof(ClearSure), false), Button(Name = "Clear", ButtonHeight = 44, Icon = SdfIconType.TrashFill), GUIColor(1, 0, 0), ButtonGroup("Main/Right/Clear")]
        public void ClearQuestion()
        {
            ClearSure = true;
            SaveSure = false;
        }
        [ShowIf(nameof(ClearSure), false), Button(Name = "RETURN", ButtonHeight = 44, Icon = SdfIconType.ArrowLeftSquareFill), GUIColor(0.85f, 0.85f, 0), ButtonGroup("Main/Right/Clear")]
        public void ClearNot()
        {
            ClearSure = false;
        }
        [ShowIf(nameof(ClearSure), false), Button(Name = "CLEAR", ButtonHeight = 44, Icon = SdfIconType.TrashFill), GUIColor(1, 0, 0), ButtonGroup("Main/Right/Clear")]
        public void Clear()
        {
            ClearSure = false;
            while (levelParent.childCount > 0)
            {
                DestroyImmediate(levelParent.GetChild(0).gameObject);
            }
        }
    }
}