using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace LunaGames.Main
{
    [InfoBox("Example Usage: CORE.FX.List[\"OnWin\"].PlayFeedbacks();", InfoMessageType.Warning)]
    public class FXManager : SerializedMonoBehaviour
    {
        public VisualEffect GlobalVFX;
        [ShowInInspector] public Dictionary<string, MMF_Player> List => UpdateList();
        private Dictionary<string, MMF_Player> list = new Dictionary<string, MMF_Player>();
        private void Awake()
        {
            CORE.FX = this;
            list = new Dictionary<string, MMF_Player>();
            GetComponentsInChildren<MMF_Player>()
                .ForEach((fxEvent) => list.Add(fxEvent.name, fxEvent));
        }
        Dictionary<string, MMF_Player> UpdateList()
        {
            if (Application.isPlaying) return list;
            else
            {
                list = new Dictionary<string, MMF_Player>();
                GetComponentsInChildren<MMF_Player>()
                    .ForEach((fxEvent) => list.Add(fxEvent.name, fxEvent));
                return list;
            }
        }
    }
}