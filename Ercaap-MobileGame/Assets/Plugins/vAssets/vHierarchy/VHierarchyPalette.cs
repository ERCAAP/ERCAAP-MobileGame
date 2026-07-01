#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static V.Hierarchy.Libs.VGUI;
using static V.Hierarchy.Libs.VUtils;



namespace V.Hierarchy
{
    public class VHierarchyPalette : ScriptableObject
    {
        public List<Color> colors = new List<Color>();

        public bool colorsEnabled;

        public void ResetColors()
        {
            colors.Clear();

            for (int i = 0; i < colorsCount; i++)
                colors.Add(GetDefaultColor(i));

            colorsEnabled = true;

            this.Dirty();

        }

        public static Color GetDefaultColor(int colorIndex)
        {
            Color color = default;

            void grey()
            {
                if (colorIndex >= greyColorsCount) return;

#if UNITY_2022_1_OR_NEWER
                color = Greyscale(isDarkTheme ? .16f : .9f);
#else
                color = Greyscale(isDarkTheme ? .315f : .9f);
#endif

            }
            void rainbowDarkTheme()
            {
                if (colorIndex < greyColorsCount) return;
                if (!isDarkTheme) return;

                color = HSLToRGB((colorIndex - greyColorsCount.ToFloat()) / rainbowColorsCount, .45f, .35f);

                if (colorIndex == 1)
                    color *= 1.2f;

                if (colorIndex == 2)
                    color *= 1.1f;

                if (colorIndex == 6)
                    color *= 1.35f;

                if (colorIndex == 7)
                    color *= 1.3f;

                if (colorIndex == 8)
                    color *= 1.05f;


                color.a = .1f;

            }
            void rainbowLightTheme()
            {
                if (colorIndex < greyColorsCount) return;
                if (isDarkTheme) return;

                color = HSLToRGB((colorIndex - greyColorsCount.ToFloat()) / rainbowColorsCount, .62f, .8f);

                color.a = .1f;

            }

            grey();
            rainbowDarkTheme();
            rainbowLightTheme();

            return color;

        }

        public static int greyColorsCount = 1;
        public static int rainbowColorsCount = 8;
        public static int colorsCount => greyColorsCount + rainbowColorsCount;




        public List<IconRow> iconRows = new List<IconRow>();

        [System.Serializable]
        public class IconRow
        {
            public List<string> builtinIcons = new List<string>(); // names
            public List<string> customIcons = new List<string>(); // guids

            public bool enabled = true;

            public bool isCustom => !builtinIcons.Any() || customIcons.Any();
            public bool isEmpty => !builtinIcons.Any() && !customIcons.Any();
            public int iconCount => builtinIcons.Count + customIcons.Count;

            public IconRow(string[] builtinIcons) => this.builtinIcons = builtinIcons.ToList();
            public IconRow() { }

        }

        public void ResetIcons()
        {
            iconRows.Clear();

            iconRows.Add(new IconRow(new[]
            {
                "Folder Icon",
                "Canvas Icon",
                "AvatarMask On Icon",
                "cs Script Icon",
                "StandaloneInputModule Icon",
                "EventSystem Icon",
                "Terrain Icon",
                "ScriptableObject Icon",

            }));
            iconRows.Add(new IconRow(new[]
            {
                "Camera Icon",
                "ParticleSystem Icon",
                "TrailRenderer Icon",
                "Material Icon",
                "ReflectionProbe Icon",

            }));
            iconRows.Add(new IconRow(new[]
            {
                "Light Icon",
                "DirectionalLight Icon",
                "LightmapParameters Icon",
                "LightProbes Icon",

            }));
            iconRows.Add(new IconRow(new[]
            {
                "Rigidbody Icon",
                "BoxCollider Icon",
                "SphereCollider Icon",
                "CapsuleCollider Icon",
                "WheelCollider Icon",
                "MeshCollider Icon",

            }));
            iconRows.Add(new IconRow(new[]
            {
                "_Help",
                "AudioClip Icon",
                "AudioListener Icon",
                "AudioEchoFilter Icon",
                "AudioReverbZone Icon",

            }));

            iconRows.Add(new IconRow(new[]
            {
                "_Help",
                "_Menu",
                "_Popup",
                "aboutwindow.mainheader",
                "ageialogo",
                "AlphabeticalSorting",
                "Animation.AddEvent",
                "Animation.AddKeyframe"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Animation.EventMarker",
                "Animation.FilterBySelection",
                "Animation.FirstKey",
                "Animation.LastKey",
                "Animation.NextKey",
                "Animation.Play",
                "Animation.PrevKey",
                "Animation.Record"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Animation.SequencerLink",
                "animationanimated",
                "animationdopesheetkeyframe",
                "animationkeyframe",
                "animationnocurve",
                "animationvisibilitytoggleoff",
                "animationvisibilitytoggleon",
                "AnimationWrapModeMenu"
            }));

iconRows.Add(new IconRow(new[]
            {
                "AssemblyLock",
                "Asset Store",
                "Unity-AssetStore-Originals-Logo-White",
                "Audio Mixer",
                "AutoLightbakingOff",
                "AutoLightbakingOn",
                "AvatarCompass",
                "AvatarController.Layer"
            }));

iconRows.Add(new IconRow(new[]
            {
                "AvatarController.LayerHover",
                "AvatarController.LayerSelected",
                "BodyPartPicker",
                "BodySilhouette",
                "DotFill",
                "DotFrame",
                "DotFrameDotted",
                "DotSelection"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Head",
                "HeadIk",
                "HeadZoom",
                "HeadZoomSilhouette",
                "LeftArm",
                "LeftFeetIk",
                "LeftFingers",
                "LeftFingersIk"
            }));

iconRows.Add(new IconRow(new[]
            {
                "LeftHandZoom",
                "LeftHandZoomSilhouette",
                "LeftLeg",
                "MaskEditor_Root",
                "RightArm",
                "RightFeetIk",
                "RightFingers",
                "RightFingersIk"
            }));

iconRows.Add(new IconRow(new[]
            {
                "RightHandZoom",
                "RightHandZoomSilhouette",
                "RightLeg",
                "Torso",
                "AvatarPivot",
                "AvatarSelector",
                "back",
                "beginButton-On"
            }));

iconRows.Add(new IconRow(new[]
            {
                "beginButton",
                "blendKey",
                "blendKeyOverlay",
                "blendKeySelected",
                "blendSampler",
                "blueGroove",
                "BuildSettings.Android On",
                "BuildSettings.Android"
            }));

iconRows.Add(new IconRow(new[]
            {
                "BuildSettings.Android.Small",
                "BuildSettings.Broadcom",
                "BuildSettings.Editor",
                "BuildSettings.Editor.Small",
                "BuildSettings.Facebook On",
                "BuildSettings.Facebook",
                "BuildSettings.Facebook.Small",
                "BuildSettings.FlashPlayer"
            }));

iconRows.Add(new IconRow(new[]
            {
                "BuildSettings.FlashPlayer.Small",
                "BuildSettings.iPhone On",
                "BuildSettings.iPhone",
                "BuildSettings.iPhone.Small",
                "BuildSettings.Lumin On",
                "BuildSettings.Lumin",
                "BuildSettings.Lumin.small",
                "BuildSettings.Metro On"
            }));

iconRows.Add(new IconRow(new[]
            {
                "BuildSettings.Metro",
                "BuildSettings.Metro.Small",
                "BuildSettings.N3DS On",
                "BuildSettings.N3DS",
                "BuildSettings.N3DS.Small",
                "BuildSettings.PS4 On",
                "BuildSettings.PS4",
                "BuildSettings.PS4.Small"
            }));

iconRows.Add(new IconRow(new[]
            {
                "BuildSettings.PSM",
                "BuildSettings.PSM.Small",
                "BuildSettings.PSP2",
                "BuildSettings.PSP2.Small",
                "BuildSettings.SelectedIcon",
                "BuildSettings.Stadia On",
                "BuildSettings.Stadia",
                "BuildSettings.Stadia.small"
            }));

iconRows.Add(new IconRow(new[]
            {
                "BuildSettings.Standalone On",
                "BuildSettings.Standalone",
                "BuildSettings.Standalone.Small",
                "BuildSettings.StandaloneBroadcom.Small",
                "BuildSettings.StandaloneGLES20Emu.Small",
                "BuildSettings.StandaloneGLESEmu",
                "BuildSettings.StandaloneGLESEmu.Small",
                "BuildSettings.Switch On"
            }));

iconRows.Add(new IconRow(new[]
            {
                "BuildSettings.Switch",
                "BuildSettings.Switch.Small",
                "BuildSettings.tvOS On",
                "BuildSettings.tvOS",
                "BuildSettings.tvOS.Small",
                "BuildSettings.Web",
                "BuildSettings.Web.Small",
                "BuildSettings.WebGL On"
            }));

iconRows.Add(new IconRow(new[]
            {
                "BuildSettings.WebGL",
                "BuildSettings.WebGL.Small",
                "BuildSettings.WP8",
                "BuildSettings.WP8.Small",
                "BuildSettings.Xbox360",
                "BuildSettings.Xbox360.Small",
                "BuildSettings.XboxOne On",
                "BuildSettings.XboxOne"
            }));

iconRows.Add(new IconRow(new[]
            {
                "BuildSettings.XboxOne.Small",
                "CacheServerConnected",
                "CacheServerDisabled",
                "CacheServerDisconnected",
                "CheckerFloor",
                "Clipboard",
                "ClothInspector.PaintTool",
                "ClothInspector.PaintValue"
            }));

iconRows.Add(new IconRow(new[]
            {
                "ClothInspector.SelectTool",
                "ClothInspector.SettingsTool",
                "ClothInspector.ViewValue",
                "CloudConnect",
                "Collab.Build",
                "Collab.BuildFailed",
                "Collab.BuildSucceeded",
                "Collab.FileAdded"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Collab.FileConflict",
                "Collab.FileDeleted",
                "Collab.FileIgnored",
                "Collab.FileMoved",
                "Collab.FileUpdated",
                "Collab.FolderAdded",
                "Collab.FolderConflict",
                "Collab.FolderDeleted"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Collab.FolderIgnored",
                "Collab.FolderMoved",
                "Collab.FolderUpdated",
                "Collab.NoInternet",
                "Collab",
                "Collab.Warning",
                "CollabConflict",
                "CollabError"
            }));

iconRows.Add(new IconRow(new[]
            {
                "CollabNew",
                "CollabOffline",
                "CollabProgress",
                "CollabPull",
                "CollabPush",
                "ColorPicker.ColorCycle",
                "ColorPicker.CycleColor",
                "ColorPicker.CycleSlider"
            }));

iconRows.Add(new IconRow(new[]
            {
                "ColorPicker.SliderCycle",
                "console.erroricon.inactive.sml",
                "console.erroricon",
                "console.erroricon.sml",
                "console.infoicon.inactive.sml",
                "console.infoicon",
                "console.infoicon.sml",
                "console.warnicon.inactive.sml"
            }));

iconRows.Add(new IconRow(new[]
            {
                "console.warnicon",
                "console.warnicon.sml",
                "CreateAddNew",
                "CrossIcon",
                "curvekeyframe",
                "curvekeyframeselected",
                "curvekeyframeselectedoverlay",
                "curvekeyframesemiselectedoverlay"
            }));

iconRows.Add(new IconRow(new[]
            {
                "curvekeyframeweighted",
                "CustomSorting",
                "CustomTool",
                "d__Help",
                "d__Menu",
                "d__Popup",
                "d_aboutwindow.mainheader",
                "d_ageialogo"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_AlphabeticalSorting",
                "d_Animation.AddEvent",
                "d_Animation.AddKeyframe",
                "d_Animation.EventMarker",
                "d_Animation.FilterBySelection",
                "d_Animation.FirstKey",
                "d_Animation.LastKey",
                "d_Animation.NextKey"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_Animation.Play",
                "d_Animation.PrevKey",
                "d_Animation.Record",
                "d_Animation.SequencerLink",
                "d_animationanimated",
                "d_animationkeyframe",
                "d_animationnocurve",
                "d_animationvisibilitytoggleoff"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_animationvisibilitytoggleon",
                "d_AnimationWrapModeMenu",
                "d_AS Badge Delete",
                "d_AS Badge New",
                "d_AssemblyLock",
                "d_Asset Store",
                "d_Audio Mixer",
                "d_AutoLightbakingOff"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_AutoLightbakingOn",
                "d_AvatarBlendBackground",
                "d_AvatarBlendLeft",
                "d_AvatarBlendLeftA",
                "d_AvatarBlendRight",
                "d_AvatarBlendRightA",
                "d_AvatarCompass",
                "d_AvatarPivot"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_AvatarSelector",
                "d_back",
                "d_beginButton-On",
                "d_beginButton",
                "d_blueGroove",
                "d_BuildSettings.Android",
                "d_BuildSettings.Android.Small",
                "d_BuildSettings.Broadcom"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_BuildSettings.Facebook",
                "d_BuildSettings.Facebook.Small",
                "d_BuildSettings.FlashPlayer",
                "d_BuildSettings.FlashPlayer.Small",
                "d_BuildSettings.iPhone",
                "d_BuildSettings.iPhone.Small",
                "d_BuildSettings.Lumin",
                "d_BuildSettings.Lumin.small"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_BuildSettings.Metro",
                "d_BuildSettings.Metro.Small",
                "d_BuildSettings.N3DS",
                "d_BuildSettings.N3DS.Small",
                "d_BuildSettings.PS4",
                "d_BuildSettings.PS4.Small",
                "d_BuildSettings.PSP2",
                "d_BuildSettings.PSP2.Small"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_BuildSettings.SelectedIcon",
                "d_BuildSettings.Stadia",
                "d_BuildSettings.Stadia.Small",
                "d_BuildSettings.Standalone",
                "d_BuildSettings.Standalone.Small",
                "d_BuildSettings.Switch",
                "d_BuildSettings.Switch.Small",
                "d_BuildSettings.tvOS"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_BuildSettings.tvOS.Small",
                "d_BuildSettings.Web",
                "d_BuildSettings.Web.Small",
                "d_BuildSettings.WebGL",
                "d_BuildSettings.WebGL.Small",
                "d_BuildSettings.Xbox360",
                "d_BuildSettings.Xbox360.Small",
                "d_BuildSettings.XboxOne"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_BuildSettings.XboxOne.Small",
                "d_BuildSettings.Xiaomi",
                "d_BuildSettings.Xiaomi.Small",
                "d_CacheServerConnected",
                "d_CacheServerDisabled",
                "d_CacheServerDisconnected",
                "d_CheckerFloor",
                "d_CloudConnect"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_Collab.FileAdded",
                "d_Collab.FileConflict",
                "d_Collab.FileDeleted",
                "d_Collab.FileIgnored",
                "d_Collab.FileMoved",
                "d_Collab.FileUpdated",
                "d_Collab.FolderAdded",
                "d_Collab.FolderConflict"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_Collab.FolderDeleted",
                "d_Collab.FolderIgnored",
                "d_Collab.FolderMoved",
                "d_Collab.FolderUpdated",
                "d_Collab",
                "d_ColorPicker.CycleColor",
                "d_ColorPicker.CycleSlider",
                "d_console.erroricon.inactive.sml"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_console.erroricon",
                "d_console.erroricon.sml",
                "d_console.infoicon.inactive.sml",
                "d_console.infoicon",
                "d_console.infoicon.sml",
                "d_console.warnicon.inactive.sml",
                "d_console.warnicon",
                "d_console.warnicon.sml"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_CreateAddNew",
                "d_curvekeyframe",
                "d_curvekeyframeselected",
                "d_curvekeyframeselectedoverlay",
                "d_curvekeyframesemiselectedoverlay",
                "d_curvekeyframeweighted",
                "d_CustomSorting",
                "d_CustomTool"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_DebuggerAttached",
                "d_DebuggerDisabled",
                "d_DebuggerEnabled",
                "d_DefaultSorting",
                "d_EditCollider",
                "d_editcollision_16",
                "d_editcollision_32",
                "d_editconstraints_16"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_editconstraints_32",
                "d_editicon.sml",
                "d_endButton-On",
                "d_endButton",
                "d_Exposure",
                "d_eyeDropper.Large",
                "d_eyeDropper.sml",
                "d_Favorite"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_FilterByLabel",
                "d_FilterByType",
                "d_FilterSelectedOnly",
                "d_forward",
                "d_FrameCapture",
                "d_GEAR",
                "d_Grid.BoxTool",
                "d_Grid.Default"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_Grid.EraserTool",
                "d_Grid.FillTool",
                "d_Grid.MoveTool",
                "d_Grid.PaintTool",
                "d_Grid.PickingTool",
                "d_Groove",
                "d_HorizontalSplit",
                "d_icon dropdown"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_Import",
                "d_InspectorLock",
                "d_Invalid",
                "d_JointAngularLimits",
                "d_leftBracket",
                "d_Lighting",
                "d_LightmapEditor.WindowTitle",
                "d_Linked"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_MainStageView",
                "d_Mirror",
                "d_model large",
                "d_monologo",
                "d_MoreOptions",
                "d_MoveTool on",
                "d_MoveTool",
                "d_Navigation"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_Occlusion",
                "d_Package Manager",
                "d_Particle Effect",
                "d_ParticleShapeTool On",
                "d_ParticleShapeTool On@3x",
                "d_ParticleShapeTool On@4x",
                "d_ParticleShapeTool",
                "d_ParticleShapeTool@3x"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_ParticleShapeTool@4x",
                "d_PauseButton On",
                "d_PauseButton",
                "d_PlayButton On",
                "d_PlayButton",
                "d_PlayButtonProfile On",
                "d_PlayButtonProfile",
                "d_playLoopOff"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_playLoopOn",
                "d_preAudioAutoPlayOff",
                "d_preAudioAutoPlayOn",
                "d_preAudioLoopOff",
                "d_preAudioLoopOn",
                "d_preAudioPlayOff",
                "d_preAudioPlayOn",
                "d_PreMatCube"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_PreMatCylinder",
                "d_PreMatLight0",
                "d_PreMatLight1",
                "d_PreMatQuad",
                "d_PreMatSphere",
                "d_PreMatTorus",
                "d_Preset.Context",
                "d_PreTexA"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_PreTexB",
                "d_PreTexG",
                "d_PreTexR",
                "d_PreTexRGB",
                "d_PreTextureAlpha",
                "d_PreTextureMipMapHigh",
                "d_PreTextureMipMapLow",
                "d_PreTextureRGB"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_Profiler.Audio",
                "d_Profiler.CPU",
                "d_Profiler.FirstFrame",
                "d_Profiler.GlobalIllumination",
                "d_Profiler.GPU",
                "d_Profiler.LastFrame",
                "d_Profiler.Memory",
                "d_Profiler.Network"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_Profiler.NetworkMessages",
                "d_Profiler.NetworkOperations",
                "d_Profiler.NextFrame",
                "d_Profiler.Physics",
                "d_Profiler.Physics2D",
                "d_Profiler.PrevFrame",
                "d_Profiler.Record",
                "d_Profiler.Rendering"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_Profiler.UI",
                "d_Profiler.UIDetails",
                "d_Profiler.Video",
                "d_ProfilerColumn.WarningCount",
                "d_Progress",
                "d_Project",
                "d_Record Off",
                "d_Record On"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_RectTool On",
                "d_RectTool",
                "d_RectTransformBlueprint",
                "d_RectTransformRaw",
                "d_redGroove",
                "d_ReflectionProbeSelector",
                "d_Refresh",
                "d_rightBracket"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_RotateTool On",
                "d_RotateTool",
                "d_SaveAs",
                "d_ScaleTool On",
                "d_ScaleTool",
                "d_scenepicking_notpickable-mixed",
                "d_scenepicking_notpickable-mixed_hover",
                "d_scenepicking_notpickable"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_scenepicking_notpickable_hover",
                "d_scenepicking_pickable-mixed",
                "d_scenepicking_pickable-mixed_hover",
                "d_scenepicking_pickable",
                "d_scenepicking_pickable_hover",
                "d_SceneView2D",
                "d_SceneViewAlpha",
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_SceneViewAudio",
                "d_SceneViewCamera",
                "d_SceneViewFx",
                "d_SceneViewLighting",
                "d_SceneViewOrtho",
                "d_SceneViewRGB",
                "d_SceneViewTools"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_SceneViewVisibility",
                "d_scenevis_hidden-mixed",
                "d_scenevis_hidden-mixed_hover",
                "d_scenevis_hidden",
                "d_scenevis_hidden_hover",
                "d_scenevis_scene_hover",
                "d_scenevis_visible-mixed",
                "d_scenevis_visible-mixed_hover"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_scenevis_visible",
                "d_scenevis_visible_hover",
                "d_ScrollShadow",
                "d_Settings",
                "d_SettingsIcon",
                "d_SocialNetworks.FacebookShare",
                "d_SocialNetworks.LinkedInShare",
                "d_SocialNetworks.Tweet"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_SocialNetworks.UDNOpen",
                "d_SpeedScale",
                "d_StepButton On",
                "d_StepButton",
                "d_StepLeftButton-On",
                "d_StepLeftButton",
                "d_tab_next",
                "d_tab_prev"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_TerrainInspector.TerrainToolLower On",
                "d_TerrainInspector.TerrainToolLowerAlt",
                "d_TerrainInspector.TerrainToolPlants On",
                "d_TerrainInspector.TerrainToolPlants",
                "d_TerrainInspector.TerrainToolPlantsAlt On",
                "d_TerrainInspector.TerrainToolPlantsAlt",
                "d_TerrainInspector.TerrainToolRaise On",
                "d_TerrainInspector.TerrainToolRaise"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_TerrainInspector.TerrainToolSetheight On",
                "d_TerrainInspector.TerrainToolSetheight",
                "d_TerrainInspector.TerrainToolSetheightAlt On",
                "d_TerrainInspector.TerrainToolSetheightAlt",
                "d_TerrainInspector.TerrainToolSettings On",
                "d_TerrainInspector.TerrainToolSettings",
                "d_TerrainInspector.TerrainToolSmoothHeight On",
                "d_TerrainInspector.TerrainToolSmoothHeight"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_TerrainInspector.TerrainToolSplat On",
                "d_TerrainInspector.TerrainToolSplat",
                "d_TerrainInspector.TerrainToolSplatAlt On",
                "d_TerrainInspector.TerrainToolSplatAlt",
                "d_TerrainInspector.TerrainToolTrees On",
                "d_TerrainInspector.TerrainToolTrees",
                "d_TerrainInspector.TerrainToolTreesAlt On",
                "d_TerrainInspector.TerrainToolTreesAlt"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_ToggleUVOverlay",
                "d_Toolbar Minus",
                "d_Toolbar Plus More",
                "d_Toolbar Plus",
                "d_ToolHandleCenter",
                "d_ToolHandleGlobal",
                "d_ToolHandleLocal",
                "d_ToolHandlePivot"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_ToolsIcon",
                "d_tranp",
                "d_TransformTool On",
                "d_TransformTool",
                "d_tree_icon",
                "d_tree_icon_branch",
                "d_tree_icon_branch_frond",
                "d_tree_icon_frond"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_tree_icon_leaf",
                "d_TreeEditor.AddBranches",
                "d_TreeEditor.AddLeaves",
                "d_TreeEditor.Branch On",
                "d_TreeEditor.Branch",
                "d_TreeEditor.BranchFreeHand On",
                "d_TreeEditor.BranchFreeHand",
                "d_TreeEditor.BranchRotate On"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_TreeEditor.BranchRotate",
                "d_TreeEditor.BranchScale On",
                "d_TreeEditor.BranchScale",
                "d_TreeEditor.BranchTranslate On",
                "d_TreeEditor.BranchTranslate",
                "d_TreeEditor.Distribution On",
                "d_TreeEditor.Distribution",
                "d_TreeEditor.Duplicate"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_TreeEditor.Geometry On",
                "d_TreeEditor.Geometry",
                "d_TreeEditor.Leaf On",
                "d_TreeEditor.Leaf",
                "d_TreeEditor.LeafFreeHand On",
                "d_TreeEditor.LeafFreeHand",
                "d_TreeEditor.LeafRotate On",
                "d_TreeEditor.LeafRotate"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_TreeEditor.LeafScale On",
                "d_TreeEditor.LeafScale",
                "d_TreeEditor.LeafTranslate On",
                "d_TreeEditor.LeafTranslate",
                "d_TreeEditor.Material On",
                "d_TreeEditor.Material",
                "d_TreeEditor.Refresh",
                "d_TreeEditor.Trash"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_TreeEditor.Wind On",
                "d_TreeEditor.Wind",
                "d_UnityEditor.AnimationWindow",
                "d_UnityEditor.ConsoleWindow",
                "d_UnityEditor.DebugInspectorWindow",
                "d_UnityEditor.FindDependencies",
                "d_UnityEditor.GameView",
                "d_UnityEditor.Graphs.AnimatorControllerTool"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_UnityEditor.HierarchyWindow",
                "d_UnityEditor.InspectorWindow",
                "d_UnityEditor.ProfilerWindow",
                "d_UnityEditor.SceneHierarchyWindow",
                "d_UnityEditor.SceneView",
                "d_UnityEditor.Timeline.TimelineWindow",
                "d_UnityEditor.VersionControl",
                "d_UnityLogo"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_Unlinked",
                "d_Valid",
                "d_VerticalSplit",
                "d_ViewToolMove On",
                "d_ViewToolMove",
                "d_ViewToolOrbit On",
                "d_ViewToolOrbit",
                "d_ViewToolZoom On"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_ViewToolZoom",
                "d_VisibilityOff",
                "d_VisibilityOn",
                "d_VUMeterTextureHorizontal",
                "d_VUMeterTextureVertical",
                "d_WaitSpin00",
                "d_WaitSpin01",
                "d_WaitSpin02"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_WaitSpin03",
                "d_WaitSpin04",
                "d_WaitSpin05",
                "d_WaitSpin06",
                "d_WaitSpin07",
                "d_WaitSpin08",
                "d_WaitSpin09",
                "d_WaitSpin10"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_WaitSpin11",
                "d_WelcomeScreen.AssetStoreLogo",
                "DebuggerAttached"
            }));

iconRows.Add(new IconRow(new[]
            {
                "DebuggerDisabled",
                "DebuggerEnabled",
                "DefaultSorting",
                "EditCollider",
                "editcollision_16",
                "editcollision_32",
                "editconstraints_16",
                "editconstraints_32"
            }));


iconRows.Add(new IconRow(new[]
            {
                "editicon.sml",
                "endButton-On",
                "endButton",
                "Exposure",
                "eyeDropper.Large",
                "eyeDropper.sml",
                "Favorite",
                "FilterByLabel"
            }));

iconRows.Add(new IconRow(new[]
            {
                "FilterByType",
                "FilterSelectedOnly",
                "forward",
                "FrameCapture",
                "GEAR",
                "Grid.BoxTool",
                "Grid.Default",
                "Grid.EraserTool"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Grid.FillTool",
                "Grid.MoveTool",
                "Grid.PaintTool",
                "Grid.PickingTool",
                "Groove",
                "align_horizontally",
                "align_horizontally_center",
                "align_horizontally_center_active"
            }));

iconRows.Add(new IconRow(new[]
            {
                "align_horizontally_left",
                "align_horizontally_left_active",
                "align_horizontally_right",
                "align_horizontally_right_active",
                "align_vertically",
                "align_vertically_bottom",
                "align_vertically_bottom_active",
                "align_vertically_center"
            }));

iconRows.Add(new IconRow(new[]
            {
                "align_vertically_center_active",
                "align_vertically_top",
                "align_vertically_top_active",
                "d_align_horizontally",
                "d_align_horizontally_center",
                "d_align_horizontally_center_active",
                "d_align_horizontally_left",
                "d_align_horizontally_left_active"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_align_horizontally_right",
                "d_align_horizontally_right_active",
                "d_align_vertically",
                "d_align_vertically_bottom",
                "d_align_vertically_bottom_active",
                "d_align_vertically_center",
                "d_align_vertically_center_active",
                "d_align_vertically_top"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_align_vertically_top_active",
                "HorizontalSplit",
                "icon dropdown",
                "Import",
                "InspectorLock",
                "Invalid",
                "JointAngularLimits",
                "KnobCShape"
            }));

iconRows.Add(new IconRow(new[]
            {
                "KnobCShapeMini",
                "leftBracket",
                "Lighting",
                "LightmapEditor.WindowTitle",
                "Lightmapping",
                "d_greenLight",
                "d_lightOff",
                "d_lightRim"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_orangeLight",
                "d_redLight",
                "greenLight",
                "lightOff",
                "lightRim",
                "orangeLight",
                "redLight",
                "Linked"
            }));

iconRows.Add(new IconRow(new[]
            {
                "LockIcon-On",
                "LockIcon",
                "loop",
                "MainStageView",
                "Mirror",
                "monologo",
                "MoreOptions",
                "MoveTool on"
            }));

iconRows.Add(new IconRow(new[]
            {
                "MoveTool",
                "Navigation",
                "Occlusion",
                "Package Manager",
                "PackageBadgeNew",
                "Add-Available",
                "Download-Available"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Error",
                "Import-Available",
                "Installed",
                "Loading",
                "Refresh",
                "Update-Available",
                "Warning"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Update-Available",
                "Warning",
                "Particle Effect",
                "ParticleShapeTool On",
                "ParticleShapeTool On@3x",
                "ParticleShapeTool On@4x",
                "ParticleShapeTool",
                "ParticleShapeTool@3x"
            }));

iconRows.Add(new IconRow(new[]
            {
                "ParticleShapeTool@4x",
                "PauseButton On",
                "PauseButton",
                "PlayButton On",
                "PlayButton",
                "PlayButtonProfile On",
                "PlayButtonProfile",
                "playLoopOff"
            }));

iconRows.Add(new IconRow(new[]
            {
                "playLoopOn",
                "playSpeed",
                "preAudioAutoPlayOff",
                "preAudioAutoPlayOn",
                "preAudioLoopOff",
                "preAudioLoopOn",
                "preAudioPlayOff",
                "preAudioPlayOn"
            }));

iconRows.Add(new IconRow(new[]
            {
                "PreMatCube",
                "PreMatCylinder",
                "PreMatLight0",
                "PreMatLight1",
                "PreMatQuad",
                "PreMatSphere",
                "PreMatTorus",
                "Preset.Context"
            }));

iconRows.Add(new IconRow(new[]
            {
                "PreTexA",
                "PreTexB",
                "PreTexG",
                "PreTexR",
                "PreTexRGB",
                "PreTextureAlpha",
                "PreTextureArrayFirstSlice",
                "PreTextureArrayLastSlice"
            }));

iconRows.Add(new IconRow(new[]
            {
                "PreTextureMipMapHigh",
                "PreTextureMipMapLow",
                "PreTextureRGB",
                "PreviewPackageInUse",
                "AreaLight Gizmo",
                "AreaLight Icon",
                "Assembly Icon",
                "AssetStore Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "AudioMixerView Icon",
                "AudioSource Gizmo",
                "boo Script Icon",
                "Camera Gizmo",
                "ChorusFilter Icon",
                "CollabChanges Icon",
                "CollabChangesConflict Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "CollabChangesDeleted Icon",
                "CollabConflict Icon",
                "CollabCreate Icon",
                "CollabDeleted Icon",
                "CollabEdit Icon",
                "CollabExclude Icon",
                "CollabMoved Icon",
                "cs Script Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_AreaLight Icon",
                "d_Assembly Icon",
                "d_AssetStore Icon",
                "d_AudioMixerView Icon",
                "d_boo Script Icon",
                "d_CollabChanges Icon",
                "d_CollabChangesConflict Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_CollabChangesDeleted Icon",
                "d_CollabConflict Icon",
                "d_CollabCreate Icon",
                "d_CollabDeleted Icon",
                "d_CollabEdit Icon",
                "d_CollabExclude Icon",
                "d_CollabMoved Icon",
                "d_cs Script Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_DirectionalLight Icon",
                "d_Favorite Icon",
                "d_Favorite On Icon",
                "d_Folder Icon",
                "d_FolderEmpty Icon",
                "d_FolderEmpty On Icon",
                "d_FolderFavorite Icon",
                "d_FolderFavorite On Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_FolderOpened Icon",
                "d_GridLayoutGroup Icon",
                "d_HorizontalLayoutGroup Icon",
                "d_Js Script Icon",
                "d_LightingDataAssetParent Icon",
                "d_Microphone Icon",
                "d_Prefab Icon",
                "d_Prefab On Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_PrefabModel Icon",
                "d_PrefabModel On Icon",
                "d_PrefabVariant Icon",
                "d_PrefabVariant On Icon",
                "d_RaycastCollider Icon",
                "d_Search Icon",
                "d_Spotlight Icon",
                "d_VerticalLayoutGroup Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "DefaultSlate Icon",
                "DirectionalLight Gizmo",
                "DirectionalLight Icon",
                "DiscLight Icon",
                "dll Script Icon",
                "EchoFilter Icon",
                "Favorite Icon",
                "Favorite On Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Folder Icon",
                "Folder On Icon",
                "FolderEmpty Icon",
                "FolderEmpty On Icon",
                "FolderFavorite Icon",
                "FolderFavorite On Icon",
                "FolderOpened Icon",
                "FolderOpened On Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "GameManager Icon",
                "GridBrush Icon",
                "HighPassFilter Icon",
                "HorizontalLayoutGroup Icon",
                "Js Script Icon",
                "LensFlare Gizmo",
                "LightingDataAssetParent Icon",
                "LightProbeGroup Gizmo"
            }));

iconRows.Add(new IconRow(new[]
            {
                "LightProbeProxyVolume Gizmo",
                "LowPassFilter Icon",
                "Main Light Gizmo",
                "MetaFile Icon",
                "Microphone Icon",
                "MuscleClip Icon",
                "ParticleSystem Gizmo",
                "ParticleSystemForceField Gizmo"
            }));

iconRows.Add(new IconRow(new[]
            {
                "PointLight Gizmo",
                "Prefab Icon",
                "Prefab On Icon",
                "PrefabModel Icon",
                "PrefabModel On Icon",
                "PrefabOverlayAdded Icon",
                "PrefabOverlayModified Icon",
                "PrefabOverlayRemoved Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "PrefabVariant Icon",
                "PrefabVariant On Icon",
                "Projector Gizmo",
                "RaycastCollider Icon",
                "ReflectionProbe Gizmo",
                "ReverbFilter Icon",
                "SceneSet Icon",
                "Search Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Search On Icon",
                "SoftlockProjectBrowser Icon",
                "SpeedTreeModel Icon",
                "SpotLight Gizmo",
                "Spotlight Icon",
                "SpriteCollider Icon",
                "sv_icon_dot0_pix16_gizmo",
                "sv_icon_dot10_pix16_gizmo"
            }));

iconRows.Add(new IconRow(new[]
            {
                "sv_icon_dot11_pix16_gizmo",
                "sv_icon_dot12_pix16_gizmo",
                "sv_icon_dot13_pix16_gizmo",
                "sv_icon_dot14_pix16_gizmo",
                "sv_icon_dot15_pix16_gizmo",
                "sv_icon_dot1_pix16_gizmo",
                "sv_icon_dot2_pix16_gizmo",
                "sv_icon_dot3_pix16_gizmo"
            }));

iconRows.Add(new IconRow(new[]
            {
                "sv_icon_dot4_pix16_gizmo",
                "sv_icon_dot5_pix16_gizmo",
                "sv_icon_dot6_pix16_gizmo",
                "sv_icon_dot7_pix16_gizmo",
                "sv_icon_dot8_pix16_gizmo",
                "sv_icon_dot9_pix16_gizmo",
                "AnimatorController Icon",
                "AnimatorController On Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "AnimatorState Icon",
                "AnimatorStateMachine Icon",
                "AnimatorStateTransition Icon",
                "BlendTree Icon",
                "d_AnimatorController Icon",
                "d_AnimatorController On Icon",
                "d_AnimatorState Icon",
                "d_AnimatorStateMachine Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_AnimatorStateTransition Icon",
                "d_BlendTree Icon",
                "AnimationWindowEvent Icon",
                "AudioMixerController Icon",
                "AudioMixerController On Icon",
                "d_AudioMixerController Icon",
                "d_AudioMixerController On Icon",
                "AudioImporter Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_AudioImporter Icon",
                "d_DefaultAsset Icon",
                "d_IHVImageFormatImporter Icon",
                "d_LightingDataAsset Icon",
                "d_LightmapParameters Icon",
                "d_LightmapParameters On Icon",
                "d_ModelImporter Icon",
                "d_SceneAsset Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_ShaderImporter Icon",
                "d_TextScriptImporter Icon",
                "d_TextureImporter Icon",
                "d_TrueTypeFontImporter Icon",
                "DefaultAsset Icon",
                "EditorSettings Icon",
                "AnyStateNode Icon",
                "d_AnyStateNode Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "HumanTemplate Icon",
                "IHVImageFormatImporter Icon",
                "LightingDataAsset Icon",
                "LightmapParameters Icon",
                "LightmapParameters On Icon",
                "ModelImporter Icon",
                "Preset Icon",
                "SceneAsset Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "SceneAsset On Icon",
                "ShaderImporter Icon",
                "SpeedTreeImporter Icon",
                "SubstanceArchive Icon",
                "TextScriptImporter Icon",
                "TextureImporter Icon",
                "TrueTypeFontImporter Icon",
                "d_SpriteAtlasAsset Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_SpriteAtlasImporter Icon",
                "SpriteAtlasAsset Icon",
                "SpriteAtlasImporter Icon",
                "d_VisualEffectSubgraphBlock Icon",
                "d_VisualEffectSubgraphOperator Icon",
                "VisualEffectSubgraphBlock Icon",
                "VisualEffectSubgraphOperator Icon",
                "VideoClipImporter Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "AssemblyDefinitionAsset Icon",
                "AssemblyDefinitionReferenceAsset Icon",
                "d_AssemblyDefinitionAsset Icon",
                "d_AssemblyDefinitionReferenceAsset Icon",
                "d_NavMeshAgent Icon",
                "d_NavMeshData Icon",
                "d_NavMeshObstacle Icon",
                "d_OffMeshLink Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "NavMeshAgent Icon",
                "NavMeshData Icon",
                "NavMeshObstacle Icon",
                "OffMeshLink Icon",
                "AnalyticsTracker Icon",
                "d_AnalyticsTracker Icon",
                "Animation Icon",
                "AnimationClip Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "AnimationClip On Icon",
                "AimConstraint Icon",
                "d_AimConstraint Icon",
                "d_LookAtConstraint Icon",
                "d_ParentConstraint Icon",
                "d_PositionConstraint Icon",
                "d_RotationConstraint Icon",
                "d_ScaleConstraint Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "LookAtConstraint Icon",
                "ParentConstraint Icon",
                "PositionConstraint Icon",
                "RotationConstraint Icon",
                "ScaleConstraint Icon",
                "Animator Icon",
                "AnimatorOverrideController Icon",
                "AnimatorOverrideController On Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "AreaEffector2D Icon",
                "AudioMixerGroup Icon",
                "AudioMixerSnapshot Icon",
                "AudioSpatializerMicrosoft Icon",
                "d_AudioMixerGroup Icon",
                "d_AudioMixerSnapshot Icon",
                "d_AudioSpatializerMicrosoft Icon",
                "AudioChorusFilter Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "AudioClip Icon",
                "AudioClip On Icon",
                "AudioDistortionFilter Icon",
                "AudioEchoFilter Icon",
                "AudioHighPassFilter Icon",
                "AudioListener Icon",
                "AudioLowPassFilter Icon",
                "AudioReverbFilter Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "AudioReverbZone Icon",
                "AudioSource Icon",
                "Avatar Icon",
                "AvatarMask Icon",
                "AvatarMask On Icon",
                "BillboardAsset Icon",
                "BillboardRenderer Icon",
                "BoxCollider Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "BoxCollider2D Icon",
                "BuoyancyEffector2D Icon",
                "Camera Icon",
                "Canvas Icon",
                "CanvasGroup Icon",
                "CanvasRenderer Icon",
                "CapsuleCollider Icon",
                "CapsuleCollider2D Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "CharacterController Icon",
                "CharacterJoint Icon",
                "CircleCollider2D Icon",
                "Cloth Icon",
                "CompositeCollider2D Icon",
                "ComputeShader Icon",
                "ConfigurableJoint Icon",
                "ConstantForce Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "ConstantForce2D Icon",
                "Cubemap Icon",
                "d_Animation Icon",
                "d_AnimationClip Icon",
                "d_AnimationClip On Icon",
                "d_Animator Icon",
                "d_AnimatorOverrideController Icon",
                "d_AnimatorOverrideController On Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_AreaEffector2D Icon",
                "d_AudioChorusFilter Icon",
                "d_AudioClip Icon",
                "d_AudioClip On Icon",
                "d_AudioDistortionFilter Icon",
                "d_AudioEchoFilter Icon",
                "d_AudioHighPassFilter Icon",
                "d_AudioListener Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_AudioLowPassFilter Icon",
                "d_AudioReverbFilter Icon",
                "d_AudioReverbZone Icon",
                "d_AudioSource Icon",
                "d_Avatar Icon",
                "d_AvatarMask Icon",
                "d_AvatarMask On Icon",
                "d_BillboardAsset Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_BillboardRenderer Icon",
                "d_BoxCollider Icon",
                "d_BoxCollider2D Icon",
                "d_BuoyancyEffector2D Icon",
                "d_Camera Icon",
                "d_Canvas Icon",
                "d_CanvasGroup Icon",
                "d_CanvasRenderer Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_CapsuleCollider Icon",
                "d_CapsuleCollider2D Icon",
                "d_CharacterController Icon",
                "d_CharacterJoint Icon",
                "d_CircleCollider2D Icon",
                "d_Cloth Icon",
                "d_CompositeCollider2D Icon",
                "d_ComputeShader Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_ConfigurableJoint Icon",
                "d_ConstantForce Icon",
                "d_ConstantForce2D Icon",
                "d_Cubemap Icon",
                "d_DistanceJoint2D Icon",
                "d_EdgeCollider2D Icon",
                "d_FixedJoint Icon",
                "d_Flare Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_Flare On Icon",
                "d_FlareLayer Icon",
                "d_Font Icon",
                "d_FrictionJoint2D Icon",
                "d_GameObject Icon",
                "d_Grid Icon",
                "d_GUISkin Icon",
                "d_GUISkin On Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_Halo Icon",
                "d_HingeJoint Icon",
                "d_HingeJoint2D Icon",
                "d_Light Icon",
                "d_LightingSettings Icon",
                "d_LightProbeGroup Icon",
                "d_LightProbeProxyVolume Icon",
                "d_LightProbes Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_LineRenderer Icon",
                "d_LODGroup Icon",
                "d_Material Icon",
                "d_Material On Icon",
                "d_Mesh Icon",
                "d_MeshCollider Icon",
                "d_MeshFilter Icon",
                "d_MeshRenderer Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_Motion Icon",
                "d_OcclusionArea Icon",
                "d_OcclusionPortal Icon",
                "d_ParticleSystem Icon",
                "d_ParticleSystemForceField Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_PlatformEffector2D Icon",
                "d_PointEffector2D Icon",
                "d_PolygonCollider2D Icon",
                "d_ProceduralMaterial Icon",
                "d_Projector Icon",
                "d_RayTracingShader Icon",
                "d_RectTransform Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_ReflectionProbe Icon",
                "d_RelativeJoint2D Icon",
                "d_RenderTexture Icon",
                "d_RenderTexture On Icon",
                "d_Rigidbody Icon",
                "d_Rigidbody2D Icon",
                "d_ScriptableObject Icon",
                "d_ScriptableObject On Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_Shader Icon",
                "d_ShaderVariantCollection Icon",
                "d_SkinnedMeshRenderer Icon",
                "d_Skybox Icon",
                "d_SliderJoint2D Icon",
                "d_SphereCollider Icon",
                "d_SpringJoint Icon",
                "d_SpringJoint2D Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_Sprite Icon",
                "d_SpriteMask Icon",
                "d_SpriteRenderer Icon",
                "d_StreamingController Icon",
                "d_SurfaceEffector2D Icon",
                "d_TargetJoint2D Icon",
                "d_Terrain Icon",
                "d_TerrainCollider Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_TerrainData Icon",
                "d_TextAsset Icon",
                "d_Texture Icon",
                "d_Texture2D Icon",
                "d_TrailRenderer Icon",
                "d_Transform Icon",
                "d_WheelCollider Icon",
                "d_WheelJoint2D Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_WindZone Icon",
                "DistanceJoint2D Icon",
                "EdgeCollider2D Icon",
                "d_EventSystem Icon",
                "d_EventTrigger Icon",
                "d_HoloLensInputModule Icon",
                "d_Physics2DRaycaster Icon",
                "d_PhysicsRaycaster Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_StandaloneInputModule Icon",
                "d_TouchInputModule Icon",
                "EventSystem Icon",
                "EventTrigger Icon",
                "HoloLensInputModule Icon",
                "Physics2DRaycaster Icon",
                "PhysicsRaycaster Icon",
                "StandaloneInputModule Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "TouchInputModule Icon",
                "d_RaytracingShader Icon",
                "RayTracingShader Icon",
                "FixedJoint Icon",
                "FixedJoint2D Icon",
                "Flare Icon",
                "Flare On Icon",
                "FlareLayer Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Font Icon",
                "Font On Icon",
                "FrictionJoint2D Icon",
                "GameObject Icon",
                "GameObject On Icon",
                "Grid Icon",
                "GUILayer Icon",
                "GUISkin Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "GUISkin On Icon",
                "GUIText Icon",
                "GUITexture Icon",
                "Halo Icon",
                "HingeJoint Icon",
                "HingeJoint2D Icon",
                "LensFlare Icon",
                "Light Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "LightingSettings Icon",
                "LightProbeGroup Icon",
                "LightProbeProxyVolume Icon",
                "LightProbes Icon",
                "LineRenderer Icon",
                "LODGroup Icon",
                "Material Icon",
                "Material On Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Mesh Icon",
                "MeshCollider Icon",
                "MeshFilter Icon",
                "MeshRenderer Icon",
                "Motion Icon",
                "MovieTexture Icon",
                "d_NetworkAnimator Icon",
                "d_NetworkDiscovery Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_NetworkIdentity Icon",
                "d_NetworkLobbyManager Icon",
                "d_NetworkLobbyPlayer Icon",
                "d_NetworkManager Icon",
                "d_NetworkManagerHUD Icon",
                "d_NetworkMigrationManager Icon",
                "d_NetworkProximityChecker Icon",
                "d_NetworkStartPosition Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_NetworkTransform Icon",
                "d_NetworkTransformChild Icon",
                "d_NetworkTransformVisualizer Icon",
                "NetworkAnimator Icon",
                "NetworkDiscovery Icon",
                "NetworkIdentity Icon",
                "NetworkLobbyManager Icon",
                "NetworkLobbyPlayer Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "NetworkManager Icon",
                "NetworkManagerHUD Icon",
                "NetworkMigrationManager Icon",
                "NetworkProximityChecker Icon",
                "NetworkStartPosition Icon",
                "NetworkTransform Icon",
                "NetworkTransformChild Icon",
                "NetworkTransformVisualizer Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "NetworkView Icon",
                "OcclusionArea Icon",
                "OcclusionPortal Icon",
                "ParticleSystem Icon",
                "ParticleSystemForceField Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "PlatformEffector2D Icon",
                "d_PlayableDirector Icon",
                "PlayableDirector Icon",
                "PointEffector2D Icon",
                "PolygonCollider2D Icon",
                "ProceduralMaterial Icon",
                "Projector Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "RayTracingShader Icon",
                "RectTransform Icon",
                "ReflectionProbe Icon",
                "RelativeJoint2D Icon",
                "d_SortingGroup Icon",
                "SortingGroup Icon",
                "RenderTexture Icon",
                "RenderTexture On Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Rigidbody Icon",
                "Rigidbody2D Icon",
                "ScriptableObject Icon",
                "ScriptableObject On Icon",
                "Shader Icon",
                "ShaderVariantCollection Icon",
                "SkinnedMeshRenderer Icon",
                "Skybox Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "SliderJoint2D Icon",
                "TrackedPoseDriver Icon",
                "SphereCollider Icon",
                "SpringJoint Icon",
                "SpringJoint2D Icon",
                "Sprite Icon",
                "SpriteMask Icon",
                "SpriteRenderer Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "StreamingController Icon",
                "SurfaceEffector2D Icon",
                "TargetJoint2D Icon",
                "Terrain Icon",
                "TerrainCollider Icon",
                "TerrainData Icon",
                "TextAsset Icon",
                "TextMesh Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Texture Icon",
                "Texture2D Icon",
                "d_Tile Icon",
                "d_Tilemap Icon",
                "d_TilemapCollider2D Icon",
                "d_TilemapRenderer Icon",
                "Tile Icon",
                "Tilemap Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "TilemapCollider2D Icon",
                "TilemapRenderer Icon",
                "d_SignalAsset Icon",
                "d_SignalEmitter Icon",
                "d_SignalReceiver Icon",
                "d_TimelineAsset Icon",
                "d_TimelineAsset On Icon",
                "SignalAsset Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "SignalEmitter Icon",
                "SignalReceiver Icon",
                "TimelineAsset Icon",
                "TimelineAsset On Icon",
                "TrailRenderer Icon",
                "Transform Icon",
                "d_SpriteAtlas Icon",
                "d_SpriteAtlas On Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_SpriteShapeRenderer Icon",
                "SpriteAtlas Icon",
                "SpriteAtlas On Icon",
                "SpriteShapeRenderer Icon",
                "AspectRatioFitter Icon",
                "Button Icon",
                "CanvasScaler Icon",
                "ContentSizeFitter Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_AspectRatioFitter Icon",
                "d_Button Icon",
                "d_CanvasScaler Icon",
                "d_ContentSizeFitter Icon",
                "d_Dropdown Icon",
                "d_FreeformLayoutGroup Icon",
                "d_GraphicRaycaster Icon",
                "d_GridLayoutGroup Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_HorizontalLayoutGroup Icon",
                "d_Image Icon",
                "d_InputField Icon",
                "d_LayoutElement Icon",
                "d_Mask Icon",
                "d_Outline Icon",
                "d_PhysicalResolution Icon",
                "d_PositionAsUV1 Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_RawImage Icon",
                "d_RectMask2D Icon",
                "d_Scrollbar Icon",
                "d_ScrollRect Icon",
                "d_ScrollViewArea Icon",
                "d_Selectable Icon",
                "d_SelectionList Icon",
                "d_SelectionListItem Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_SelectionListTemplate Icon",
                "d_Shadow Icon",
                "d_Slider Icon",
                "d_Text Icon",
                "d_Toggle Icon",
                "d_ToggleGroup Icon",
                "d_VerticalLayoutGroup Icon",
                "Dropdown Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "FreeformLayoutGroup Icon",
                "GraphicRaycaster Icon",
                "GridLayoutGroup Icon",
                "HorizontalLayoutGroup Icon",
                "Image Icon",
                "InputField Icon",
                "LayoutElement Icon",
                "Mask Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Outline Icon",
                "PositionAsUV1 Icon",
                "RawImage Icon",
                "RectMask2D Icon",
                "Scrollbar Icon",
                "ScrollRect Icon",
                "Selectable Icon",
                "Shadow Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Slider Icon",
                "Text Icon",
                "Toggle Icon",
                "ToggleGroup Icon",
                "VerticalLayoutGroup Icon",
                "d_StyleSheet Icon",
                "d_VisualTreeAsset Icon",
                "StyleSheet Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "VisualTreeAsset Icon",
                "d_VisualEffect Icon",
                "d_VisualEffectAsset Icon",
                "VisualEffect Icon",
                "VisualEffectAsset Icon",
                "d_VideoPlayer Icon",
                "VideoClip Icon",
                "VideoPlayer Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "VisualEffect Icon",
                "VisualEffectAsset Icon",
                "WheelCollider Icon",
                "WheelJoint2D Icon",
                "WindZone Icon",
                "d_SpatialMappingCollider Icon",
                "SpatialMappingCollider Icon",
                "SpatialMappingRenderer Icon"
            }));

iconRows.Add(new IconRow(new[]
            {
                "UssScript Icon",
                "UxmlScript Icon",
                "VerticalLayoutGroup Icon",
                "VideoEffect Icon",
                "VisualEffect Gizmo",
                "VisualEffectAsset Icon",
                "WindZone Gizmo"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Profiler.Audio",
                "Profiler.CPU",
                "Profiler.FirstFrame",
                "Profiler.GlobalIllumination",
                "Profiler.GPU",
                "Profiler.Instrumentation",
                "Profiler.LastFrame",
                "Profiler.Memory"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Profiler.NetworkMessages",
                "Profiler.NetworkOperations",
                "Profiler.NextFrame",
                "Profiler.Physics",
                "Profiler.Physics2D",
                "Profiler.PrevFrame",
                "Profiler.Record",
                "Profiler.Rendering"
            }));

iconRows.Add(new IconRow(new[]
            {
                "Profiler.UI",
                "Profiler.UIDetails",
                "Profiler.Video",
                "ProfilerColumn.WarningCount",
                "Progress",
                "Project",
                "Record Off",
                "Record On"
            }));

iconRows.Add(new IconRow(new[]
            {
                "RectTool On",
                "RectTool",
                "RectTransformBlueprint",
                "RectTransformRaw",
                "redGroove",
                "ReflectionProbeSelector",
                "Refresh",
                "rightBracket"
            }));

iconRows.Add(new IconRow(new[]
            {
                "RotateTool On",
                "RotateTool",
                "RotateTool@4x",
                "SaveActive",
                "SaveAs",
                "SaveFromPlay",
                "SavePassive",
                "ScaleTool On"
            }));

iconRows.Add(new IconRow(new[]
            {
                "ScaleTool",
                "SceneLoadIn",
                "SceneLoadOut",
                "scenepicking_notpickable-mixed",
                "scenepicking_notpickable-mixed_hover",
                "scenepicking_notpickable",
                "scenepicking_notpickable_hover",
                "scenepicking_pickable-mixed"
            }));

iconRows.Add(new IconRow(new[]
            {
                "scenepicking_pickable-mixed_hover",
                "scenepicking_pickable",
                "scenepicking_pickable_hover",
                "SceneSave",
                "SceneSaveGrey",
                "SceneView2D",
                "SceneViewAlpha",
            }));

iconRows.Add(new IconRow(new[]
            {
                "SceneViewAudio",
                "SceneViewCamera",
                "SceneViewFx",
                "SceneViewLighting",
                "SceneViewOrtho",
                "SceneViewRGB",
                "SceneViewTools"
            }));

iconRows.Add(new IconRow(new[]
            {
                "SceneViewVisibility",
                "scenevis_hidden-mixed",
                "scenevis_hidden-mixed_hover",
                "scenevis_hidden",
                "scenevis_hidden_hover",
                "scenevis_scene_hover",
                "scenevis_visible-mixed",
                "scenevis_visible-mixed_hover"
            }));

iconRows.Add(new IconRow(new[]
            {
                "scenevis_visible",
                "scenevis_visible_hover",
                "ScrollShadow",
                "Settings",
                "SettingsIcon",
                "alertDialog",
                "conflict-icon",
                "d_GridAxisX"
            }));

iconRows.Add(new IconRow(new[]
            {
                "d_GridAxisY",
                "d_GridAxisZ",
                "GridAxisX",
                "GridAxisY",
                "GridAxisZ"
            }));

iconRows.Add(new IconRow(new[]
            {
                "SocialNetworks.FacebookShare",
                "SocialNetworks.LinkedInShare",
                "SocialNetworks.Tweet",
                "SocialNetworks.UDNLogo",
                "SocialNetworks.UDNOpen",
                "SoftlockInline",
                "SpeedScale"
            }));

iconRows.Add(new IconRow(new[]
            {
                "StateMachineEditor.ArrowTip",
                "StateMachineEditor.ArrowTipSelected",
                "StateMachineEditor.Background",
                "StateMachineEditor.State",
                "StateMachineEditor.StateHover",
                "StateMachineEditor.StateSelected",
                "StateMachineEditor.StateSub",
                "StateMachineEditor.StateSubHover"
            }));

iconRows.Add(new IconRow(new[]
            {
                "StateMachineEditor.StateSubSelected",
                "StateMachineEditor.UpButton",
                "StateMachineEditor.UpButtonHover",
                "StepButton On",
                "StepButton",
                "StepLeftButton-On",
                "StepLeftButton",
                "sv_icon_dot0_sml"
            }));

iconRows.Add(new IconRow(new[]
            {
                "sv_icon_dot10_sml",
                "sv_icon_dot11_sml",
                "sv_icon_dot12_sml",
                "sv_icon_dot13_sml",
                "sv_icon_dot14_sml",
                "sv_icon_dot15_sml",
                "sv_icon_dot1_sml",
                "sv_icon_dot2_sml"
            }));

iconRows.Add(new IconRow(new[]
            {
                "sv_icon_dot3_sml",
                "sv_icon_dot4_sml",
                "sv_icon_dot5_sml",
                "sv_icon_dot6_sml",
                "sv_icon_dot7_sml",
                "sv_icon_dot8_sml",
                "sv_icon_dot9_sml",
                "sv_icon_name0"
            }));

iconRows.Add(new IconRow(new[]
            {
                "sv_icon_name1",
                "sv_icon_name2",
                "sv_icon_name3",
                "sv_icon_name4",
                "sv_icon_name5",
                "sv_icon_name6",
                "sv_icon_name7",
                "sv_icon_none"
            }));

iconRows.Add(new IconRow(new[]
            {
                "sv_label_0",
                "sv_label_1",
                "sv_label_2",
                "sv_label_3",
                "sv_label_4",
                "sv_label_5",
                "sv_label_6",
                "sv_label_7"
            }));

iconRows.Add(new IconRow(new[]
            {
                "tab_next",
                "tab_prev",
                "TerrainInspector.TerrainToolAdd",
                "TerrainInspector.TerrainToolLower On",
                "TerrainInspector.TerrainToolLower",
                "TerrainInspector.TerrainToolLowerAlt",
                "TerrainInspector.TerrainToolPlants On",
                "TerrainInspector.TerrainToolPlants"
            }));

iconRows.Add(new IconRow(new[]
            {
                "TerrainInspector.TerrainToolPlantsAlt On",
                "TerrainInspector.TerrainToolPlantsAlt",
                "TerrainInspector.TerrainToolRaise On",
                "TerrainInspector.TerrainToolRaise",
                "TerrainInspector.TerrainToolSculpt On",
                "TerrainInspector.TerrainToolSculpt",
                "TerrainInspector.TerrainToolSetheight On",
                "TerrainInspector.TerrainToolSetheight"
            }));

iconRows.Add(new IconRow(new[]
            {
                "TerrainInspector.TerrainToolSetheightAlt On",
                "TerrainInspector.TerrainToolSetheightAlt",
                "TerrainInspector.TerrainToolSettings On",
                "TerrainInspector.TerrainToolSettings",
                "TerrainInspector.TerrainToolSmoothHeight On",
                "TerrainInspector.TerrainToolSmoothHeight",
                "TerrainInspector.TerrainToolSplat On",
                "TerrainInspector.TerrainToolSplat"
            }));

iconRows.Add(new IconRow(new[]
            {
                "TerrainInspector.TerrainToolSplatAlt On",
                "TerrainInspector.TerrainToolSplatAlt",
                "TerrainInspector.TerrainToolTrees On",
                "TerrainInspector.TerrainToolTrees",
                "TerrainInspector.TerrainToolTreesAlt On",
                "TerrainInspector.TerrainToolTreesAlt",
                "TestFailed",
                "TestIgnored"
            }));

iconRows.Add(new IconRow(new[]
            {
                "TestInconclusive",
                "TestNormal",
                "TestPassed",
                "TestStopwatch",
                "ToggleUVOverlay",
                "Toolbar Minus",
                "Toolbar Plus More",
                "Toolbar Plus"
            }));

iconRows.Add(new IconRow(new[]
            {
                "ToolHandleCenter",
                "ToolHandleGlobal",
                "ToolHandleLocal",
                "ToolHandlePivot",
                "ToolsIcon",
                "tranp",
                "TransformTool On",
                "TransformTool"
            }));

iconRows.Add(new IconRow(new[]
            {
                "tree_icon",
                "tree_icon_branch",
                "tree_icon_branch_frond",
                "tree_icon_frond",
                "tree_icon_leaf",
                "TreeEditor.AddBranches",
                "TreeEditor.AddLeaves",
                "TreeEditor.Branch On"
            }));

iconRows.Add(new IconRow(new[]
            {
                "TreeEditor.Branch",
                "TreeEditor.BranchFreeHand On",
                "TreeEditor.BranchFreeHand",
                "TreeEditor.BranchRotate On",
                "TreeEditor.BranchRotate",
                "TreeEditor.BranchScale On",
                "TreeEditor.BranchScale",
                "TreeEditor.BranchTranslate On"
            }));

iconRows.Add(new IconRow(new[]
            {
                "TreeEditor.BranchTranslate",
                "TreeEditor.Distribution On",
                "TreeEditor.Distribution",
                "TreeEditor.Duplicate",
                "TreeEditor.Geometry On",
                "TreeEditor.Geometry",
                "TreeEditor.Leaf On",
                "TreeEditor.Leaf"
            }));

iconRows.Add(new IconRow(new[]
            {
                "TreeEditor.LeafFreeHand On",
                "TreeEditor.LeafFreeHand",
                "TreeEditor.LeafRotate On",
                "TreeEditor.LeafRotate",
                "TreeEditor.LeafScale On",
                "TreeEditor.LeafScale",
                "TreeEditor.LeafTranslate On",
                "TreeEditor.LeafTranslate"
            }));

iconRows.Add(new IconRow(new[]
            {
                "TreeEditor.Material On",
                "TreeEditor.Material",
                "TreeEditor.Refresh",
                "TreeEditor.Trash",
                "TreeEditor.Wind On",
                "TreeEditor.Wind",
                "UnityEditor.AnimationWindow",
                "UnityEditor.ConsoleWindow"
            }));

iconRows.Add(new IconRow(new[]
            {
                "UnityEditor.DebugInspectorWindow",
                "UnityEditor.FindDependencies",
                "UnityEditor.GameView",
                "UnityEditor.Graphs.AnimatorControllerTool",
                "UnityEditor.HierarchyWindow",
                "UnityEditor.InspectorWindow",
                "UnityEditor.ProfilerWindow",
                "UnityEditor.SceneHierarchyWindow"
            }));

iconRows.Add(new IconRow(new[]
            {
                "UnityEditor.SceneView",
                "UnityEditor.Timeline.TimelineWindow",
                "UnityEditor.VersionControl",
                "UnityLogo",
                "UnityLogoLarge",
                "UnLinked",
                "UpArrow",
                "Valid"
            }));

iconRows.Add(new IconRow(new[]
            {
                "P4_AddedLocal",
                "P4_AddedRemote",
                "P4_BlueLeftParenthesis",
                "P4_BlueRightParenthesis",
                "P4_CheckOutLocal"
            }));

iconRows.Add(new IconRow(new[]
            {
                "P4_CheckOutRemote",
                "P4_Conflicted",
                "P4_DeletedLocal",
                "P4_DeletedRemote",
                "P4_Local",
                "P4_LockedLocal",
                "P4_LockedRemote",
                "P4_OutOfSync"
            }));

iconRows.Add(new IconRow(new[]
            {
                "P4_RedLeftParenthesis",
                "P4_RedRightParenthesis",
                "P4_Updating",
                "VerticalSplit",
                "ViewToolMove On",
                "ViewToolMove",
                "ViewToolOrbit On",
                "ViewToolOrbit"
            }));

iconRows.Add(new IconRow(new[]
            {
                "ViewToolZoom On",
                "ViewToolZoom",
                "VisibilityOff",
                "VisibilityOn",
                "VUMeterTextureHorizontal",
                "VUMeterTextureVertical",
                "WaitSpin00",
                "WaitSpin01"
            }));

iconRows.Add(new IconRow(new[]
            {
                "WaitSpin02",
                "WaitSpin03",
                "WaitSpin04",
                "WaitSpin05",
                "WaitSpin06",
                "WaitSpin07",
                "WaitSpin08",
                "WaitSpin09"
            }));

iconRows.Add(new IconRow(new[]
            {
                "WaitSpin10",
                "WaitSpin11",
                "WelcomeScreen.AssetStoreLogo"
            }));
            this.Dirty();

        }




        [ContextMenu("Export palette")]
        public void Export()
        {
            var packagePath = EditorUtility.SaveFilePanel("Export vHierarchy Palette", "", this.GetPath().GetFilename(withExtension: false), "unitypackage");

            var iconPaths = iconRows.SelectMany(r => r.customIcons).Select(r => r.ToPath()).Where(r => !r.IsNullOrEmpty());

            AssetDatabase.ExportPackage(iconPaths.Append(this.GetPath()).ToArray(), packagePath);

            EditorUtility.RevealInFinder(packagePath);

        }




        void Reset() { ResetColors(); ResetIcons(); }

    }
}
#endif