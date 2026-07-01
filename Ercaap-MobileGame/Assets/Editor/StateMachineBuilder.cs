using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace LunaGames.Editor
{
    public class StateMachineBuilder : EditorWindow
    {
        private string Directory;
        private string Namespace;
        private string StateMachineName;
        private string ReferenceName;
        [SerializeField] private List<string> StateNames = new List<string>();
        private SerializedObject serializedObject;
        private SerializedProperty itemListProperty;
        public string OverviewState()
        {
            if (StateNames.Count > 0)
                return NewState(StateNames[0]);
            else
                return NewState("EXAMPLE_STATE");
        }
        public static void OpenWindow()
        {
            StateMachineBuilder Window = GetWindow<StateMachineBuilder>();
            Window.titleContent = new GUIContent("State Machine Builder");
            Window.minSize = new Vector2(500, 500);
            Window.Show();
        }
        private void OnEnable()
        {
            // Create a serialized object and property for the list
            serializedObject = new SerializedObject(this);
            itemListProperty = serializedObject.FindProperty("StateNames");
        }

        private Vector2 MainScroll;
        private void OnGUI()
        {
            DrawHeader();
            DrawMain();
            DrawList();
            DrawPreviews();
            GUILayout.FlexibleSpace();
            DrawFooter();
        }
        public string NewStateMachine()
        {
            string template = "using LunaGames.Tools.States;\r\nusing UnityEngine;\r\n\r\nnamespace NAMESPACE\r\n{\r\n    public class STATEMACHINE : StateMachine\r\n    {\r\n        void Start()\r\n        {\r\n        \r\n        }\r\n    }\r\n}\r\n";
            template = template.Replace("NAMESPACE", Namespace);
            template = template.Replace("STATEMACHINE", StateMachineName);
            return template;
        }
        public string NewState(string StateName)
        {
            string template = "using LunaGames.Tools.States;\r\nusing UnityEngine;\r\n\r\nnamespace NAMESPACE\r\n{\r\n    public class STATENAME : State\r\n    {\r\n        private STATEMACHINE StateMachine;\r\n        public STATENAME(STATEMACHINE StateMachine)\r\n        {\r\n            this.StateMachine = StateMachine;\r\n        }\r\n\r\n        public override void OnStart()\r\n        {\r\n            \r\n        }\r\n\r\n        public override void OnUpdate()\r\n        {\r\n\r\n        }\r\n\r\n        public override void OnStop()\r\n        {\r\n\r\n        }\r\n    }\r\n}";
            template = template.Replace("NAMESPACE", Namespace);
            template = template.Replace("STATEMACHINE", StateMachineName);
            template = template.Replace("StateMachine", ReferenceName);
            template = template.Replace("STATENAME", StateName);
            return template;
        }
        private void DrawHeader()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            GUILayout.Label("Directory", GUILayout.Width(80));
            Directory = EditorGUILayout.TextField(Directory, EditorStyles.toolbarTextField, GUILayout.ExpandWidth(true));

            if (GUILayout.Button("Select", EditorStyles.toolbarButton, GUILayout.Width(60)))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    Directory = selectedPath.Replace(Application.dataPath, "Assets");
                }
            }

            GUILayout.EndHorizontal();
        }
        private void DrawMain()
        {
            GUILayout.BeginVertical(GUI.skin.box);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Namespace", GUILayout.Width(130));
            Namespace = EditorGUILayout.TextField(Namespace);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("State Machine Name", GUILayout.Width(130));
            StateMachineName = EditorGUILayout.TextField(StateMachineName);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Reference Name", GUILayout.Width(130));
            ReferenceName = EditorGUILayout.TextField(ReferenceName);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
        private void DrawList()
        {
            MainScroll = EditorGUILayout.BeginScrollView(MainScroll);
            GUILayout.BeginVertical();
            EditorGUILayout.PropertyField(itemListProperty);
            serializedObject.ApplyModifiedProperties();
            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
        private Vector2 previewScroll1, previewScroll2;
        private void DrawPreviews()
        {
            GUILayout.BeginHorizontal(GUI.skin.box, GUILayout.Height(202));
            GUILayout.BeginVertical(GUILayout.Width((position.size.x / 2) - 5));
            GUILayout.Label("Example State Machine", GUILayout.Width(150));
            previewScroll1 = EditorGUILayout.BeginScrollView(previewScroll1);
            EditorGUILayout.HelpBox(NewStateMachine(), MessageType.None);
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(GUILayout.Width((position.size.x / 2) - 5));
            GUILayout.Label("Example State", GUILayout.Width(150));
            previewScroll2 = EditorGUILayout.BeginScrollView(previewScroll2);
            EditorGUILayout.HelpBox(OverviewState(), MessageType.None);
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
        private void DrawFooter()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Build State Machine", GUILayout.Height(50)))
            {
                BuildStateMachine();
            }
            GUILayout.EndHorizontal();
        }
        public void CreateFile(string fileName, string content)
        {
            string path = Directory + $"/{fileName}.cs";
            File.WriteAllText(path, content);
        }
        public void BuildStateMachine()
        {
            CreateFile(StateMachineName, NewStateMachine());
            foreach (string stateName in StateNames)
            {
                CreateFile(stateName, NewState(stateName));
            }
            AssetDatabase.Refresh();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<MonoScript>(Directory + $"/{StateMachineName}.cs");
        }
    }
}

