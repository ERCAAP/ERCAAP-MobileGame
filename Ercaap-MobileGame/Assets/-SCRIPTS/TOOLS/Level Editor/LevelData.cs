using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaGames.Tools.LevelEditor
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Level Data/New Level", order = 1)]
    [HideMonoScript]
    public class LevelData : ScriptableObject
    {
        [TableList(ShowIndexLabels = true)] public List<LevelObject> levelObjects;
    }
    [Serializable] public struct LevelObject
    {
        [TableColumnWidth(100, false), PreviewField(Height = 100), AssetsOnly, VerticalGroup("Prefab"), HideLabel]
        [SerializeField] public GameObject Object;
        [HideInInspector] public bool ActiveSelf;
        [VerticalGroup("Properties"), ShowInInspector, TextArea, LabelText("@Object.name")] public string ParametersJSON;
        [VerticalGroup("Properties")] public TransformInfo RootTransfrom;
        [VerticalGroup("Properties")] public TransformInfo ChildTransfrom;
    }
    [Serializable] public struct TransformInfo
    {
        public Vector3 LocalPosition;
        public Quaternion LocalRotation;
        public Vector3 LocalScale;
    }
}