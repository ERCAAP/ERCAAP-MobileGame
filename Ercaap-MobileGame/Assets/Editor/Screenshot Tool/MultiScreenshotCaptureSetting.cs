using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaGames.Editor
{
    [CreateAssetMenu(fileName = "Screenshot Setting", menuName = "Editor Settings/Screenshot Setting")]
    public class MultiScreenshotCaptureSetting : SerializedScriptableObject
    {
        [BoxGroup("Current Resolution"), HideLabel] public ResolutionDatum CurrentResolution;
        [TableList(ShowIndexLabels = false, AlwaysExpanded = true, HideToolbar = false, IsReadOnly = false, ShowPaging = true)]
        public List<ResolutionDatum> ResolutionData;
        public bool currentResolutionEnabled = true;
        public float resolutionMultiplier = 1f;
        [EnumToggleButtons] public TargetCamera targetCamera = TargetCamera.GameView;
        public bool captureOverlayUI = false;
        public bool setTimeScaleToZero = true;
        public bool saveAsPNG = true;
        [ShowIf("saveAsPNG")] public bool allowTransparentBackground = false;
        [FolderPath(AbsolutePath = true)] public string saveDirectory;

    }
    public enum TargetCamera { GameView, SceneView };
    [Serializable]
    public class ResolutionDatum
    {
        [HorizontalGroup("Main"), HideLabel] public bool resolutionsEnabled;
        [HorizontalGroup("Main"), HideLabel] public Vector2 resolution;
    }
}
