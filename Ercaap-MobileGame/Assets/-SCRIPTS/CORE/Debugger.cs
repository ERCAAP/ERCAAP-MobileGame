using LunaGames.Extentions;
using LunaGames.Main;
using Sirenix.OdinInspector;
using System.ComponentModel;
using UnityEngine;

namespace LunaGames.Main
{
    [InfoBox("Build versiyon icerisinde fonksiyonlari buton ile cagirmak icin bu scripti duzenleyin.", InfoMessageType.Warning)]
    public class Debugger : MonoBehaviour
    {
        void Awake() => CORE.DEBUG ??= this;
    }
}
namespace StompyRobot.SROptions
{
    public partial class SROptions
    {
        [Category("Resources")]
        [Increment(500)]
        public int Money
        {
            get => PlayerPrefs.GetInt(nameof(Money), 0) < 0 ? 0 : PlayerPrefs.GetInt(nameof(Money), 0);
            set
            {
                PlayerPrefs.SetInt(nameof(Money), value < 0 ? 0 : value);
                if (CORE.UI != null) CORE.UI.moneyText.text = RichText.Emote("Money").Size(1.2f) + " " + value.ToString();
            }
        }
        [Category("Settings")]
        [NumberRange(0, 5)]
        public int Speed
        {
            get => (int)CORE.DATA.GameSpeed;
            set => CORE.DATA.GameSpeed = value;
        }
        [Category("Settings")]
        public bool Vibration
        {
            get => CORE.DATA.Vibration;
            set => CORE.DATA.Vibration = value;
        }
        [Category("Settings")]
        public bool Sound
        {
            get => CORE.DATA.Sound;
            set => CORE.DATA.Sound = value;
        }
        [Category("Misc")]
        public bool TutorialFinished
        {
            get { return PlayerPrefs.GetInt(nameof(TutorialFinished), 0) == 1; }
            set { PlayerPrefs.SetInt(nameof(TutorialFinished), value ? 1 : 0); }
        }
        [Category("Misc")]
        public void Reset() => CORE.UI.ResetGame();
    }
}