using Cinemachine;
using LunaGames.Tools.States;
using Sirenix.OdinInspector;
using LunaGames.Main;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

namespace LunaGames.Tutorial
{
    public class TutorialSM : StateMachine
    {
    
        public bool isShowedPortal
        {
            get => PlayerPrefs.GetInt(nameof(isShowedPortal), 0) == 1;
            set => PlayerPrefs.SetInt(nameof(isShowedPortal), value ? 1 : 0);
        }



        


        private void Awake()
        {
            CORE.TUTORIAL = this;
        }
        void Start()
        {
            if (CORE.DATA.TutorialFinished) return;
        }
    }
}