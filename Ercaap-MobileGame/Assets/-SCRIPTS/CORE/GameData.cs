using UnityEngine;
using Sirenix.Utilities;
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using LunaGames.Extentions;

namespace LunaGames.Main
{
    [HideMonoScript]
    public class GameData : MonoBehaviour
    {
        #region GAME DATA
        [ShowInInspector, TabGroup("GAME DATA")] public GameStates GameState
        {
            get { return gameState; }
            set
            {
                gameState = value;
                switch (value)
                {
                    case GameStates.OnGame:
                    case GameStates.Tutorial:
                        Elephant.LevelStarted(LastLevel + 1);
                        break;
                    case GameStates.Win:
                        Elephant.LevelCompleted(LastLevel + 1);
                        break;
                    case GameStates.Fail:
                        Elephant.LevelFailed(LastLevel + 1);
                        break;
                }
            }
        }
        [ShowInInspector, TabGroup("GAME DATA")] public int LastLevel
        {
            get => PlayerPrefs.GetInt(nameof(LastLevel), 0);
            set => PlayerPrefs.SetInt(nameof(LastLevel), value);
        }
        [ShowInInspector, TabGroup("GAME DATA")] public float Gold
        {
            get { return money; }
            set
            {
                money = value;
                if (Application.isPlaying) CORE.UI.moneyText.text = value.ToString();
            }
        }
        [ShowInInspector, TabGroup("GAME DATA")]
        public float Diamond
        {
            get { return diamond; }
            set
            {
                diamond = value;
                if (Application.isPlaying) CORE.UI.diomandText.text = value.ToString();
            }
        }
        [ShowInInspector, TabGroup("GAME DATA")] public float totalMoneyFromLevel => Gold - initMoney;
        [ShowInInspector, PropertyRange(0f, 5f), TabGroup("GAME DATA")] public float GameSpeed
        {
            get { return Time.timeScale; }
            set
            {
                Time.timeScale = value;
                Time.fixedDeltaTime = 0.02f * value;
            }
        }
        [ShowInInspector, TabGroup("GAME DATA")] public bool TutorialFinished
        {
            get => PlayerPrefs.GetInt(nameof(TutorialFinished), 0) == 1;
            set => PlayerPrefs.SetInt(nameof(TutorialFinished), value ? 1: 0);
        }
        #endregion
        #region SETTINGS
        [TabGroup("SETTINGS")] public bool DynamicGameSpeed;
        [ShowInInspector, TabGroup("SETTINGS")] public bool Sound
        {
            get => PlayerPrefs.GetInt(nameof(Sound), 1) == 1;
            set
            {
                PlayerPrefs.SetInt(nameof(Sound), value ? 1 : 0);
                AudioListener.volume = value ? 1 : 0;
            }
        }
        [ShowInInspector, TabGroup("SETTINGS")] public bool Vibration
        {
            get => PlayerPrefs.GetInt(nameof(Vibration), 1) == 1;
            set
            {
                PlayerPrefs.SetInt(nameof(Vibration), value ? 1 : 0);
                HapticController.hapticsEnabled = value;
            }
        }
        [ShowInInspector, TabGroup("SETTINGS")] public bool FPS
        {
            get => PlayerPrefs.GetInt(nameof(FPS), 0) == 1;
            set => PlayerPrefs.SetInt(nameof(FPS), value ? 1 : 0);
        }
        [ShowInInspector, TabGroup("SETTINGS")]  public bool HD
        {
            get => QualitySettings.GetQualityLevel() == 1;
            set => QualitySettings.SetQualityLevel(value ? 1 : 0);
        }
        #endregion
        #region UPGRADES
        #endregion

        private float money;
        private float diamond;
        private float initMoney;
        private GameStates gameState;

        public AstarPath astarPath;
        public float[] targetXP;
        #region REMOTE DATA

        //[Title("Remote Fields Examples")]
        [TabGroup("REMOTE"), ShowInInspector] public List<int> RemoteListExample
        {
            get
            {
                List<int> temp = new List<int>();
                RemoteConfig
                    .GetInstance()
                    .Get(nameof(RemoteListExample), "1*2*3*3*4")
                    .Split('*')
                    .ForEach((element) => temp.Add(int.Parse(element)));
                return temp;
            }
        }
        [TabGroup("REMOTE"), ShowInInspector] public string RemoteStringExample => RemoteConfig.GetInstance().Get(nameof(RemoteStringExample), "Text from remote");
        [TabGroup("REMOTE"), ShowInInspector] public bool Debugger => RemoteConfig.GetInstance().GetBool(nameof(Debugger), false);
        [TabGroup("REMOTE"), ShowInInspector] public bool RemoteBoolExample => RemoteConfig.GetInstance().GetBool(nameof(RemoteBoolExample), false);

        #endregion

        private void Awake()
        {
            CORE.DATA = this;
            if (Debugger) SRDebug.Init();
            AudioListener.volume = Sound ? 1 : 0;
            if (Vibration) CORE.FX.List["EnableHaptics"].PlayFeedbacks();
            else CORE.FX.List["DisableHaptics"].PlayFeedbacks();
            Gold = PlayerPrefs.GetFloat(nameof(Gold));
            Diamond = PlayerPrefs.GetFloat(nameof(Diamond));
        }
        public void SetInitials() {
            initMoney = Gold;
        }
        /*private void OnEnable() => SRDebug.Instance.AddOptionContainer(this);
        private void OnDisable() => SRDebug.Instance.RemoveOptionContainer(this);*/

        public void SaveGold()
        {
            PlayerPrefs.SetFloat(nameof(Gold), Gold);
        }
            
        public void SaveDiamond()
        {
            PlayerPrefs.SetFloat(nameof(Diamond), diamond);
        }
            
        public void SaveMoneyWithBonus(float bonus) => 
            PlayerPrefs.SetFloat(nameof(Gold), Gold + (int)(totalMoneyFromLevel * bonus));
    }
}