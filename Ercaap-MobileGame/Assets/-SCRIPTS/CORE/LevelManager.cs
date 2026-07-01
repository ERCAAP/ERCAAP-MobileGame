using LunaGames.Tools.LevelEditor;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LunaGames.Main
{
    [HideMonoScript]
    public class LevelManager : MonoBehaviour
    {
        public enum LevelListTypes { PrefabList, PrefabFromFolder, ScriptableObjectList, ScriptableObjectFromFolder }
        [BoxGroup("Level List Type"), EnumToggleButtons, SerializeField, HideLabel] LevelListTypes LevelListType;

        [BoxGroup("Prefab List"), ShowIf("LevelListType", LevelListTypes.PrefabList), SerializeField] List<GameObject> levelPrefabs;
        [BoxGroup("Scriptable Object"), ShowIf("LevelListType", LevelListTypes.ScriptableObjectList), SerializeField] List<ScriptableObject> levelScriptableObjects;

        [BoxGroup("Prefab From Folder"), ShowIf("LevelListType", LevelListTypes.PrefabFromFolder), FolderPath(ParentFolder = "Assets/Resources"), InlineButton(nameof(GetLevelCount)), SerializeField] string levelFolderPath;
        [BoxGroup("Prefab From Folder"), ShowIf("LevelListType", LevelListTypes.PrefabFromFolder), SerializeField, ReadOnly] int levelObjectCount;

        [BoxGroup("Scriptable Object From Folder"), ShowIf("LevelListType", LevelListTypes.ScriptableObjectFromFolder), FolderPath(ParentFolder = "Assets/Resources"), InlineButton(nameof(GetLevelCount)), SerializeField] string SOFolderPath;
        [BoxGroup("Scriptable Object From Folder"), ShowIf("LevelListType", LevelListTypes.ScriptableObjectFromFolder), SerializeField, ReadOnly] int scriptableObjectCount;

        [BoxGroup("Index Settings")] public bool customLevel;
        [BoxGroup("Index Settings"), ShowIf("customLevel")] public int customLevelNo;
        [BoxGroup("Index Settings"), ReadOnly] public int currentLevel;
        [BoxGroup("Index Settings"), SerializeField] int startIndexAfterLoop = 0;

        [TabGroup("OnLevelLoad")] public UnityEvent OnLevelLoad;
        [TabGroup("OnLevelStart")] public UnityEvent OnLevelStart;
        [TabGroup("OnLevelEnd")] public UnityEvent OnLevelEnd;

        private GameObject currentLevelObject;
        private ScriptableObject currentSO;
        private LevelEditor LevelEditor;

        private void Awake()
        {
            LevelEditor = GameObject.FindObjectOfType<LevelEditor>();
            CORE.LEVELMANAGER = this;
        }
        void Start()
        {
            currentLevel = customLevel ? customLevelNo : CORE.DATA.LastLevel;
            CORE.UI.levelText.text = $"{currentLevel + 1}";
            SetCurrentLevelObject();
            if (LevelListType == LevelListTypes.PrefabList || LevelListType == LevelListTypes.PrefabFromFolder) Initialize_Prefab();
            if (LevelListType == LevelListTypes.ScriptableObjectList || LevelListType == LevelListTypes.ScriptableObjectFromFolder) Initialize_SO();
        }

        private void SetCurrentLevelObject()
        {
            if (LevelListType == LevelListTypes.PrefabList)
            {
                int totalLevelNumber = levelPrefabs.Count;
                if (totalLevelNumber <= 0) return;
                currentLevelObject = levelPrefabs[GetLevelIndex(totalLevelNumber)];
            }
            else if (LevelListType == LevelListTypes.PrefabFromFolder)
            {
                int totalLevelNumber = levelObjectCount;
                if (totalLevelNumber <= 0) return;
                currentLevelObject = Resources.Load<GameObject>($"{levelFolderPath}/Level {GetLevelIndex(totalLevelNumber)}");
            }
            else if (LevelListType == LevelListTypes.ScriptableObjectList)
            {
                int totalLevelNumber = levelScriptableObjects.Count;
                if (totalLevelNumber <= 0) return;
                currentSO = levelScriptableObjects[GetLevelIndex(totalLevelNumber)];
            }
            else if (LevelListType == LevelListTypes.ScriptableObjectFromFolder)
            {
                int totalLevelNumber = scriptableObjectCount;
                if (totalLevelNumber <= 0) return;
                currentSO = Resources.Load<ScriptableObject>($"{SOFolderPath}/Level {GetLevelIndex(totalLevelNumber)}");
            }
        }

        private int GetLevelIndex(int totalLevelNumber)
        {
            int index = currentLevel % totalLevelNumber;
            if (currentLevel >= totalLevelNumber)
            {
                index = (currentLevel - totalLevelNumber) % (totalLevelNumber - startIndexAfterLoop);
                index += startIndexAfterLoop;
            }
            return index;
        }
        public void GetLevelCount()
        {
            GameObject[] allLevels = Resources.LoadAll<GameObject>($"{levelFolderPath}");
            ScriptableObject[] allSOs = Resources.LoadAll<ScriptableObject>($"{SOFolderPath}");
            levelObjectCount = allLevels.Length;
            scriptableObjectCount = allSOs.Length;
        }

        private void LevelUp() => CORE.DATA.LastLevel++;

        void Initialize_Prefab()
        {
            if (currentLevelObject == null) return;
            GameObject instancedLevel = Instantiate(currentLevelObject);
            // Level Objesi ile ne yap�lacaksa burada yap�lacak;
            OnLevelLoad.Invoke();
        }
        void Initialize_SO()
        {
            if (currentSO == null) return;
            LevelEditor.LevelData = currentSO as LevelData;
            LevelEditor.LoadData();
            OnLevelLoad.Invoke();
        }

        public void StartGame() //Tap To Start
        {
            CORE.FX.List["OnStart"].PlayFeedbacks();
            OnLevelStart.Invoke();
            CORE.DATA.GameState = CORE.DATA.TutorialFinished ? GameStates.OnGame : GameStates.Tutorial;
        }
        [ButtonGroup("EndGameStates")]
        public void FinishGame()
        {
            switch (CORE.DATA.GameState)
            {
                case GameStates.OnGame:
                case GameStates.Tutorial:
                    OnLevelEnd.Invoke();
                    CORE.FX.List["OnWin"].PlayFeedbacks();
                    CORE.DATA.GameState = GameStates.Win;
                    LevelUp();
                    break;
            }
        }
        [ButtonGroup("EndGameStates")]
        public void GameOver()
        {
            switch (CORE.DATA.GameState)
            {
                case GameStates.OnGame:
                case GameStates.Tutorial:
                    OnLevelEnd.Invoke();
                    CORE.FX.List["OnLose"].PlayFeedbacks();
                    CORE.DATA.GameState = GameStates.Fail;
                    break;
            }
        }
    }
}