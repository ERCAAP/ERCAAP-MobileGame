using MPUIKIT;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;

namespace LunaGames.Tools
{
    public class SetTint : MonoBehaviour
    {
        public Color Tint;
        [ShowInInspector] public List<List<Color>> InitialColors = new List<List<Color>>();
        public List<MPImage> Targets;
        public bool initialized;

        [Button] public void SaveInitialColors() {
            if (initialized) return;
            initialized = true;
            for (int i = 0; i < Targets.Count; i++)
            {
                List<Color> cols = new List<Color>();
                cols.Add(Targets[i].color);
                cols.Add(Targets[i].OutlineColor);
                InitialColors.Add(cols);
            }
        }
        [Button] public void TintIt() {
            for (int i = 0; i < Targets.Count; i++)
            {
                Targets[i].color = InitialColors[i][0] * Tint;
                Targets[i].OutlineColor = InitialColors[i][1] * Tint;
            }
        }
    }
}
