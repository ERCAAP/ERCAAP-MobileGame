using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LunaGames.Editor
{
    public class PlayerPrefsEditor : EditorWindow
    {
        private SerializedObject ThisSO;

        public List<PrefEntry> SystemPreferences = new List<PrefEntry>();

        private List<PrefEntry> PlayerPreferences = new List<PrefEntry>();
        public List<PrefEntry> FilteredPlayerPreferences = new List<PrefEntry>();

        private readonly string[] UNITY_HIDDEN_SETTINGS = new string[]
        {
            "UnityGraphicsQuality",
            "unity.cloud_userid",
            "unity.player_session_background_time",
            "unity.player_session_elapsed_time",
            "unity.player_sessionid",
            "unity.player_session_count"
        };

        [SerializeField] private bool debugMode;
        [SerializeField] private string search = "";
        [SerializeField] private string keyInput;
        [SerializeField] private ValueTypes valueTypeInput;
        public enum ValueTypes { String, Integer, Float }

        //[MenuItem("Luna Games/Player Prefs", priority = 1)]
        public static void Init()
        {
            
            PlayerPrefsEditor Window = GetWindow<PlayerPrefsEditor>();
            Window.titleContent = new GUIContent("Player Prefs");
            Window.Show();
        }

        private void Update()
        {
            if(GetAllKeys().Length != (PlayerPreferences.Count + SystemPreferences.Count))
                Refresh();

        }

        [System.Serializable]
        public class PrefEntry
        {
            public string Key;
            public string S_Value;
            public int I_Value;
            public float F_Value;
            public ValueTypes valueType;

            public void SaveKey()
            {
                if (valueType == ValueTypes.String) PlayerPrefs.SetString(Key, S_Value);
                else if (valueType == ValueTypes.Integer) PlayerPrefs.SetInt(Key, I_Value);
                else if (valueType == ValueTypes.Float) PlayerPrefs.SetFloat(Key, F_Value);
            }
            public PrefEntry(string Key)
            {
                this.Key = Key;
                if (PlayerPrefs.GetString(Key, "EMPTY_STRING") != "EMPTY_STRING")
                {
                    valueType = ValueTypes.String;
                    S_Value = PlayerPrefs.GetString(Key);
                }
                else if (PlayerPrefs.GetInt(Key, 456852) != 456852)
                {
                    valueType = ValueTypes.Integer;
                    I_Value = PlayerPrefs.GetInt(Key);
                }
                else if (PlayerPrefs.GetFloat(Key, 123.321f) != 123.321f)
                {
                    valueType = ValueTypes.Float;
                    F_Value = PlayerPrefs.GetFloat(Key);
                }
            }
        }

        [SerializeField, HideInInspector] private VisualTreeAsset UI;
        [SerializeField, HideInInspector] private VisualTreeAsset PrefEntryUI;

        ListView Player;
        ListView System;

        private void CreateGUI()
        {
            VisualElement Root = UI.Instantiate();
            Root.StretchToParentSize();
            ThisSO = new SerializedObject(this);

            ToolbarButton Button_Refresh = Root.Q<ToolbarButton>(nameof(Button_Refresh));
            Button_Refresh.clicked += Refresh;

            ToolbarSearchField SearchField = Root.Q<ToolbarSearchField>(nameof(SearchField));
            SearchField.RegisterValueChangedCallback<string>(SearchUpdate);


            Player = Root.Q<ListView>(nameof(Player));
            Player.makeItem = PrefEntryUI.Instantiate;
            Player.bindingPath = "FilteredPlayerPreferences";
            Player.bindItem = BindPlayerItem;

            System = Root.Q<ListView>(nameof(System));
            System.makeItem = PrefEntryUI.Instantiate;
            System.itemsSource = SystemPreferences;
            System.bindItem = BindSystemItem;
            System.SetEnabled(false);

            Refresh();


            void ToggleSystemPrefs(ChangeEvent<bool> evt)
            {
                if (evt.newValue == true)
                {
                    System.AddToClassList("system__active");
                }
                else
                {
                    System.RemoveFromClassList("system__active");
                }
            }
            ToolbarToggle DebugToggle = Root.Q<ToolbarToggle>("Toggle_SystemPrefs");
            DebugToggle.RegisterValueChangedCallback(ToggleSystemPrefs);

            

            void ResetPrefs()
            {
                PlayerPrefs.DeleteAll();
                Refresh();
            }
            ToolbarButton Button_ClearPrefs = Root.Q<ToolbarButton>(nameof(Button_ClearPrefs));
            Button_ClearPrefs.clicked += ResetPrefs;

            TextField Field_Search = Root.Q<TextField>(nameof(Field_Search));
            Field_Search.RegisterValueChangedCallback(Search);

            void Search(ChangeEvent<string> evt)
            {
                search = evt.newValue;
                Refresh();
            }

            TextField KeyInput = Root.Q<TextField>("Field_Key");
            TextField S_Input = Root.Q<TextField>("Field_StringValue");
            IntegerField I_Input = Root.Q<IntegerField>("Field_IntValue");
            FloatField F_Input = Root.Q<FloatField>("Field_FloatValue");

            KeyInput.bindingPath = "keyInput";

            EnumField Field_DataType = Root.Q<EnumField>(nameof(Field_DataType));
            Field_DataType.RegisterValueChangedCallback(updateValueType);

            void updateValueType(ChangeEvent<Enum> evt)
            {
                S_Input.style.display = DisplayStyle.None;
                I_Input.style.display = DisplayStyle.None;
                F_Input.style.display = DisplayStyle.None;

                ValueTypes v = (ValueTypes)evt.newValue;
                if (v == ValueTypes.String) S_Input.style.display = DisplayStyle.Flex;
                else if (v == ValueTypes.Integer) I_Input.style.display = DisplayStyle.Flex;
                else if (v == ValueTypes.Float) F_Input.style.display = DisplayStyle.Flex;
            }

            ToolbarButton Button_AddKey = Root.Q<ToolbarButton>(nameof(Button_AddKey));
            Button_AddKey.clicked += AddKey;

            void AddKey()
            {
                Debug.Log(KeyInput.value + " " + Field_DataType.value.ToString());
                if (KeyInput.value == null || KeyInput.value.Trim() == "" || PlayerPrefs.HasKey(KeyInput.value)) return;
                if (Field_DataType.text == "String") PlayerPrefs.SetString(KeyInput.value, S_Input.value);
                else if (Field_DataType.text == "Integer") PlayerPrefs.SetInt(KeyInput.value, I_Input.value);
                else if (Field_DataType.text == "Float") PlayerPrefs.SetFloat(KeyInput.value, F_Input.value);

                Field_DataType.value = ValueTypes.String;
                KeyInput.value = "";
                Refresh();

                Field_Search.MarkDirtyRepaint();
            }

            EditorApplication.update -= Update;
            EditorApplication.update += Update;

            rootVisualElement.Add(Root);
        }

        private void SearchUpdate(ChangeEvent<string> evt)
        {
            Debug.Log("TEST0");
        }

        private void BindSystemItem(VisualElement element, int i)
        {
            PrefEntry prefEntry = SystemPreferences[i];

            Label Key = element.Q<Label>("Key");
            Key.text = SystemPreferences[i].Key;

            Label ValueType = element.Q<Label>("valueType");
            ValueType.text = SystemPreferences[i].valueType.ToString();

            PropertyField value = element.Q<PropertyField>("Value");

            VisualElement ValueArea = element.Q<VisualElement>("ValueArea");

            if (ValueArea.childCount > 1) return;

            if (prefEntry.valueType == ValueTypes.String)
            {
                TextField S = new TextField();
                Action A = () => S.value = PlayerPrefs.GetString(prefEntry.Key);
                S.bindingPath = "PrefEntry.S_Value";
                ValueArea.Add(S);
                S.schedule.Execute(A).Every(10);
                S.RegisterValueChangedCallback((e) => PlayerPrefs.SetString(prefEntry.Key, e.newValue));
            }
            else if (prefEntry.valueType == ValueTypes.Integer)
            {
                IntegerField I = new IntegerField();
                Action A = () => I.value = PlayerPrefs.GetInt(prefEntry.Key);
                I.bindingPath = "PrefEntry.I_Value";
                ValueArea.Add(I);
                I.schedule.Execute(A).Every(10);
                I.RegisterValueChangedCallback((e) => PlayerPrefs.SetInt(prefEntry.Key, e.newValue));
            }
            else if (prefEntry.valueType == ValueTypes.Float)
            {
                FloatField F = new FloatField();
                Action A = () => F.value = PlayerPrefs.GetFloat(prefEntry.Key);
                F.bindingPath = "PrefEntry.F_Value";
                ValueArea.Add(F);
                F.schedule.Execute(A).Every(10);
                F.RegisterValueChangedCallback((e) => PlayerPrefs.SetFloat(prefEntry.Key, e.newValue));
            }
        }

        private void BindPlayerItem(VisualElement element, int i)
        {
            PrefEntry prefEntry = FilteredPlayerPreferences[i];

            Label Key = element.Q<Label>("Key");
            Key.text = prefEntry.Key;

            Label ValueType = element.Q<Label>("valueType");
            ValueType.text = prefEntry.valueType.ToString();

            PropertyField value = element.Q<PropertyField>("Value");

            VisualElement ValueArea = element.Q<VisualElement>("ValueArea");

            if (ValueArea.childCount > 1) return;

            if (prefEntry.valueType == ValueTypes.String)
            {
                TextField S = new TextField();
                Action A = () => S.SetValueWithoutNotify(PlayerPrefs.GetString(prefEntry.Key));
                S.bindingPath = "PrefEntry.S_Value";
                ValueArea.Add(S);
                S.schedule.Execute(A).Every(10);
                S.RegisterValueChangedCallback((e) => PlayerPrefs.SetString(prefEntry.Key, e.newValue));
            }
            else if (prefEntry.valueType == ValueTypes.Integer)
            {
                IntegerField I = new IntegerField();
                Action A = () => I.SetValueWithoutNotify(PlayerPrefs.GetInt(prefEntry.Key));
                I.bindingPath = "PrefEntry.I_Value";
                ValueArea.Add(I);
                I.schedule.Execute(A).Every(10);
                I.RegisterValueChangedCallback((e) => PlayerPrefs.SetInt(prefEntry.Key, e.newValue));
            }
            else if (prefEntry.valueType == ValueTypes.Float)
            {
                FloatField F = new FloatField();
                Action A = () => F.SetValueWithoutNotify(PlayerPrefs.GetFloat(prefEntry.Key));
                F.bindingPath = "PrefEntry.F_Value";
                ValueArea.Add(F);
                F.schedule.Execute(A).Every(10);
                F.RegisterValueChangedCallback((e) => PlayerPrefs.SetFloat(prefEntry.Key, e.newValue));
            }
            Button Button_RemoveElement = element.Q<Button>("Button_RemoveElement");
            Button_RemoveElement.clicked += (() =>
            {
                string key = element.Q<Label>("Key").text;
                Debug.Log(key);
                PlayerPrefs.DeleteKey(key);
                Refresh();
            });
        }

        #region PROCCESS_DATA
        public void Refresh()
        {
            List<PrefEntry> entries = new List<PrefEntry>();
            List<PrefEntry> hidden = new List<PrefEntry>();
            List<string> Hidden = UNITY_HIDDEN_SETTINGS.ToList();
            foreach (string entry in GetAllKeys())
            {
                if (Hidden.Contains(entry)) hidden.Add(new PrefEntry(entry));
                else entries.Add(new PrefEntry(entry));
            }
            PlayerPreferences = entries;
            SystemPreferences = hidden;
            FilteredPlayerPreferences = PlayerPreferences.Where((Pref) => Pref.Key.ToLower().Contains(search.ToLower())).OrderBy((pref) => pref.Key).ToList();
            ReList();
        }
        private void ReList()
        {
            Player.itemsSource = FilteredPlayerPreferences;
            System.itemsSource = SystemPreferences;
            Player.Rebuild();
            Player.MarkDirtyRepaint();
        }
        #endregion

        #region RETRIEVE_DATA
        public string[] GetAllKeys()
        {
            List<string> result = new List<string>();

            if (Application.platform == RuntimePlatform.WindowsEditor)
                result.AddRange(GetAllWindowsKeys());
            else if (Application.platform == RuntimePlatform.OSXEditor)
                result.AddRange(GetAllMacKeys());
            else if (Application.platform == RuntimePlatform.LinuxEditor)
                result.AddRange(GetAllLinuxKeys());
            else
                Debug.LogError("Unsupported platform detected, please contact support@rejected-games.com and let us know.");
            return result.ToArray();
        }

        /// <summary>
        /// On Mac OS X PlayerPrefs are stored in ~/Library/Preferences folder, in a file named unity.[company name].[product name].plist, where company and product names are the names set up in Project Settings. The same .plist file is used for both Projects run in the Editor and standalone players. 
        /// </summary>
        private string[] GetAllMacKeys()
        {
            string plistPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Preferences/unity." + PlayerSettings.companyName + "." + PlayerSettings.productName + ".plist";
            string[] keys = new string[0];

            if (File.Exists(plistPath))
            {
                FileInfo fi = new FileInfo(plistPath);
                Dictionary<string, object> plist = (Dictionary<string, object>)Plist.readPlist(fi.FullName);

                keys = new string[plist.Count];
                plist.Keys.CopyTo(keys, 0);
            }

            return keys;
        }

        /// <summary>
        /// On Windows, PlayerPrefs are stored in the registry under HKCU\Software\[company name]\[product name] key, where company and product names are the names set up in Project Settings.
        /// </summary>
        private string[] GetAllWindowsKeys()
        {
            RegistryKey cuKey = Registry.CurrentUser;
            RegistryKey unityKey;
            unityKey = cuKey.CreateSubKey("Software\\Unity\\UnityEditor\\" + PlayerSettings.companyName + "\\" + PlayerSettings.productName);

            string[] values = unityKey.GetValueNames();
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Substring(0, values[i].LastIndexOf("_"));
            }

            return values;
        }

        /// <summary>
        /// On Linux, PlayersPrefs are stored in /home/lukas/.config/unity3d/[companyName]/[productName]/prefs, where company and product names are the names set up in Project Settings.
        /// </summary>
        private string[] GetAllLinuxKeys()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/.config/unity3d/" + PlayerSettings.companyName + "/" + PlayerSettings.productName + "/prefs";
            List<string> keys = new List<string>();

            XmlDocument xmlDoc = new XmlDocument();
            if (File.Exists(path))
            {
                xmlDoc.LoadXml(File.ReadAllText(path));
            }

            foreach (XmlElement node in xmlDoc.SelectNodes("unity_prefs/pref"))
            {
                keys.Add(node.GetAttribute("name"));
            }

            return keys.ToArray();
        }
        #endregion
    }
}
