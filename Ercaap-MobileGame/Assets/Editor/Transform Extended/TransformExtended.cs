using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;
using System;
using Object = UnityEngine.Object;
using System.Text.RegularExpressions;

namespace LunaGames.Editor
{
    [CustomEditor(typeof(Transform))]
    [CanEditMultipleObjects]
    public class TransformExtended : UnityEditor.Editor
    {
        [SerializeField]
        private VisualTreeAsset UI = default;

        private TransformData Data = new TransformData(Vector3.one, Quaternion.identity, Vector3.one);

        private Action DefaultUI;

        private UnityEditor.Editor defaultGUI;

        [System.Serializable]
        public class TransformData
        {
            public Vector3 Pos;
            public Quaternion Rot;
            public Vector3 Scale;

            public TransformData(Vector3 Pos, Quaternion Rot, Vector3 Scale)
            {
                this.Pos = Pos;
                this.Rot = Rot;
                this.Scale = Scale;
            }
        }
        public Vector3 StringToVector3(string sVector)
        {
            string input = sVector;
            string pattern = @"\((.*?)\)";

            Match match = Regex.Match(input, pattern);
            string[] sArray;
            if (match.Success)
            {
                string extractedText = match.Groups[1].Value;
                sArray = extractedText.Split(',');
            }
            else
            {
                return new Vector3(0, 0, 0);
            }
            Vector3 result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));

            return result;
        }
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement UI = this.UI.Instantiate();
            if (targets.Length <= 0) return base.CreateInspectorGUI();
            defaultGUI = CreateEditor(targets, Type.GetType("UnityEditor.TransformInspector, UnityEditor"));
            IMGUIContainer DefaultTransform = UI.Q<IMGUIContainer>("DefaultTransform");
            //InspectorElement.FillDefaultInspector(DefaultTransform, serializedObject, this);
            DefaultTransform.onGUIHandler += () =>
            {
                if (targets.Length <= 0 || defaultGUI == null) return;
                defaultGUI.OnInspectorGUI();
                EditorGUIUtility.labelWidth = 60;
            };
            Transform ThisT = serializedObject.targetObject as Transform;

            #region Position
            Button position_copy_button = UI.Q<VisualElement>("PositionCP").Q<Button>("Copy");
            Button position_paste_button = UI.Q<VisualElement>("PositionCP").Q<Button>("Paste");
            Button position_reset_button = UI.Q<VisualElement>("PositionR").Q<Button>("Reset");

            position_copy_button.clicked += delegate { Copy("localPosition"); };
            position_paste_button.clicked += delegate { Paste("localPosition"); };
            position_reset_button.clicked += delegate { Reset("localPosition"); };
            #endregion

            #region Rotation
            Button rotation_copy_button = UI.Q<VisualElement>("RotationCP").Q<Button>("Copy");
            Button rotation_paste_button = UI.Q<VisualElement>("RotationCP").Q<Button>("Paste");
            Button rotation_reset_button = UI.Q<VisualElement>("RotationR").Q<Button>("Reset");

            rotation_copy_button.clicked += delegate { Copy("localEulerAngles"); };
            rotation_paste_button.clicked += delegate { Paste("localEulerAngles"); };
            rotation_reset_button.clicked += delegate { Reset("localEulerAngles"); };
            #endregion

            #region Rotation
            Button scale_copy_button = UI.Q<VisualElement>("ScaleCP").Q<Button>("Copy");
            Button scale_paste_button = UI.Q<VisualElement>("ScaleCP").Q<Button>("Paste");
            Button scale_reset_button = UI.Q<VisualElement>("ScaleR").Q<Button>("Reset");

            scale_copy_button.clicked += delegate { Copy("localScale"); };
            scale_paste_button.clicked += delegate { Paste("localScale"); };
            scale_reset_button.clicked += delegate { Reset("localScale"); };
            #endregion

            #region Component
            Button component_copy_button = UI.Q<VisualElement>("ComponentCPR").Q<Button>("Copy");
            Button component_paste_button = UI.Q<VisualElement>("ComponentCPR").Q<Button>("Paste");
            Button component_reset_button = UI.Q<VisualElement>("ComponentCPR").Q<Button>("Reset");

            component_copy_button.clicked += delegate { Copy("Component"); };
            component_paste_button.clicked += delegate { Paste("Component"); };
            component_reset_button.clicked += delegate { Reset("Component"); };
            #endregion
            return UI;
        }
        private void OnDestroy()
        {
            DestroyImmediate(defaultGUI);
        }
        private void Copy(string propertyName)
        {
            if (serializedObject.targetObjects.Length <= 0) return;

            if (propertyName == "Component")
            {
                Undo.RegisterCompleteObjectUndo(serializedObject.targetObjects, "Copy Objects Transform");
                foreach (Object target in serializedObject.targetObjects)
                {
                    Transform targetT = target as Transform;
                    targetT.GetPositionAndRotation(out Vector3 Pos, out Quaternion Rot);
                    Data = new TransformData(Pos, Rot, targetT.localScale);
                    string buffer = $"[{propertyName}]{JsonUtility.ToJson(Data)}";
                    EditorGUIUtility.systemCopyBuffer = buffer;
                }
            }
            else
            {
                Undo.RegisterCompleteObjectUndo(serializedObject.targetObjects, $"Copy Objects {propertyName}");
                foreach (Object target in serializedObject.targetObjects)
                {
                    Transform targetT = target as Transform;
                    string s_Vector3 = GetPropertyValue(targetT, propertyName).ToString();
                    string buffer = $"[{propertyName}]{s_Vector3}";
                    EditorGUIUtility.systemCopyBuffer = buffer;
                }
            }
        }
        private void Paste(string propertyName)
        {
            if (serializedObject.targetObjects.Length <= 0) return;

            if (propertyName == "Component" && EditorGUIUtility.systemCopyBuffer.Contains("Component"))
            {
                Undo.RegisterCompleteObjectUndo(serializedObject.targetObjects, $"Paste Objects Transform");
                foreach (Object target in serializedObject.targetObjects)
                {
                    Transform targetT = target as Transform;
                    JsonUtility.FromJsonOverwrite(EditorGUIUtility.systemCopyBuffer.Replace("[Component]", ""), Data);
                    targetT.SetPositionAndRotation(Data.Pos, Data.Rot);
                    targetT.localScale = Data.Scale;
                }
            }

            if (!EditorGUIUtility.systemCopyBuffer.Contains(propertyName)) return;
            Undo.RegisterCompleteObjectUndo(serializedObject.targetObjects, $"Paste Objects {propertyName}");
            foreach (Object target in serializedObject.targetObjects)
            {
                Transform targetT = target as Transform;
                SetPropertyValue(targetT, propertyName, StringToVector3(EditorGUIUtility.systemCopyBuffer));
            }

        }
        private void Reset(string propertyName)
        {
            if (serializedObject.targetObjects.Length <= 0) return;

            if (propertyName == "Component")
            {
                Undo.RegisterCompleteObjectUndo(serializedObject.targetObjects, $"Reset Objects Transform");
                foreach (Object target in serializedObject.targetObjects)
                {
                    Transform targetT = target as Transform;
                    targetT.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                    targetT.localScale = Vector3.one;
                }
            }
            else
            {
                Undo.RegisterCompleteObjectUndo(serializedObject.targetObjects, $"Reset Objects {propertyName}");
                foreach (Object target in serializedObject.targetObjects)
                {
                    Transform targetT = target as Transform;
                    switch (propertyName)
                    {
                        case "localPosition":
                            SetPropertyValue(targetT, propertyName, Vector3.zero);
                            break;
                        case "localEulerAngles":
                            SetPropertyValue(targetT, propertyName, Vector3.zero);
                            break;
                        case "localScale":
                            SetPropertyValue(targetT, propertyName, Vector3.one);
                            break;
                    }
                }
            }


        }
        public object GetPropertyValue(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
        }

        public void SetPropertyValue(object obj, string propertyName, object value)
        {
            obj.GetType().GetProperty(propertyName)?.SetValue(obj, value);
        }
    }

}