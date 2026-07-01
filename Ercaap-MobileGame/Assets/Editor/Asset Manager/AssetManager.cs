using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System;

using UnityEditorInternal;
using UnityEditor;

using UnityEngine.Networking;
using UnityEngine;

using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;

namespace LunaGames.Editor
{
    public class AssetManager : OdinEditorWindow
    {
        private string search = "";
        private string settingsPath = "Assets/-SETTINGS/Asset Manager Setting.asset";
        private static AssetManagerSetting Settings;
        private string IP;
        private static int category = 0;
        private Task RefreshOP;
        public static void OpenWindow()
        {
            AssetManager Window = GetWindow<AssetManager>();
            Window.minSize = new Vector2(665f, 400f);
            Window.Show();
        }

        [HideInInspector] public List<AssetInfo> AssetListUnfiltered = new List<AssetInfo>();

        [TableList(ShowIndexLabels = false, AlwaysExpanded = true, HideToolbar = false, IsReadOnly = true, ShowPaging = true), ShowInInspector]
        public List<AssetInfo> AssetList
        {
            get => AssetListUnfiltered.Where((asset) => asset.Name.ToLower().Contains(search.ToLower())).ToList();
            set => AssetListUnfiltered = value;
        }

        [Serializable]
        public class AssetInfo
        {
            public bool Downloaded => IsFileExists(this);
            private string ButtonName => Downloaded ? "Import" : "Download";
            [TableColumnWidth(50, false), VerticalGroup(""), EnableIf("Downloaded"), Button("", Icon = SdfIconType.Trash), PropertyOrder(0)] public void Remove() => RemoveFile(this);
            [TableColumnWidth(100, false), VerticalGroup("Action"), Button("$ButtonName"), PropertyOrder(0)] public void Download() => DownloadAsset(this);

            [TableColumnWidth(200), ReadOnly, PropertyOrder(1)] public string Name;
            [TableColumnWidth(75), ReadOnly, PropertyOrder(2)] public string Version;
            [HideInInspector] public string Path;
            private float size;
            [TableColumnWidth(75, false), ReadOnly, PropertyOrder(3), ShowInInspector] public string Size => $"{size.ToString("0.00")} MB";

            [TableColumnWidth(150, false), ReadOnly, ProgressBar(0, "size"), PropertyOrder(4)] public float Progress;

            public AssetInfo(string Name, float Size, string Path)
            {
                this.Version = Name.Split('[')[1].Split(']')[0];
                this.Name = Name.Replace($"[{Version}]", "");
                this.size = Size;
                this.Path = Path;
                this.Progress = IsFileExists(this) ? Size : 0;
            }
        }
        private void GetSettings()
        {
            if (AssetDatabase.LoadAssetAtPath(settingsPath, typeof(AssetManagerSetting)) as AssetManagerSetting == null)
            {
                AssetManagerSetting NewSetting = ScriptableObject.CreateInstance<AssetManagerSetting>();
                AssetDatabase.CreateAsset(NewSetting, settingsPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
                Settings = AssetDatabase.LoadAssetAtPath(settingsPath, typeof(AssetManagerSetting)) as AssetManagerSetting;
            IP = Settings.Categories[category].URL;
        }
        public void Browser()
        {
            System.Diagnostics.Process.Start($"http://{IP}/");
        }
        public void Folder()
        {
            System.Diagnostics.Process.Start(InternalEditorUtility.unityPreferencesFolder + Path.DirectorySeparatorChar + $"../../Asset Store-5.x/");
        }
        private static string Authenticate()
        {
            string auth = Settings.Categories[category].UserName + ":" + Settings.Categories[category].Password;
            auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
            auth = "Basic " + auth;
            return auth;
        }
        public async Task Refresh()
        {
            AssetListUnfiltered.Clear();
            List<string> assetNames = new List<string>();

            UnityWebRequest pageRequest = UnityWebRequest.Get("http://" + IP + "/");
            string authorization = Authenticate();
            pageRequest.SetRequestHeader("AUTHORIZATION", authorization);
            pageRequest.SendWebRequest();
            while (pageRequest.result == UnityWebRequest.Result.InProgress) await Task.Yield();

            string results = pageRequest.downloadHandler.text;

            string input = results;
            string startSubstring = ".unitypackage\">";
            string endSubstring = "<";

            // Use regular expression to match the pattern
            string pattern = $"{startSubstring}(.*?){endSubstring}";
            MatchCollection matches = Regex.Matches(input, pattern);

            foreach (Match match in matches)
            {
                string assetName = match.Value;
                if (assetName == ".unitypackage\"><") continue;
                assetNames.Add(assetName.Replace(startSubstring, "").Replace(endSubstring, ""));
            }
            pageRequest.Dispose();

            string[] files = assetNames.ToArray();

            foreach (string fileName in files)
            {
                string filePath = "http://" + IP + "/" + fileName;

                UnityWebRequest fileRequest = UnityWebRequest.Head(filePath);
                fileRequest.SetRequestHeader("AUTHORIZATION", authorization);
                fileRequest.SendWebRequest();
                while (fileRequest.result == UnityWebRequest.Result.InProgress) await Task.Yield();

                float size = float.Parse(fileRequest.GetResponseHeader("Content-Length")) / 1000f / 1000f;
                AssetListUnfiltered.Add(new AssetInfo(fileName.Replace(".unitypackage", ""), size, filePath));
            }
        }
        public static bool IsFileExists(AssetInfo asset)
        {
            return File.Exists(InternalEditorUtility.unityPreferencesFolder + Path.DirectorySeparatorChar + $"../../Asset Store-5.x/{asset.Name}[{asset.Version}].unitypackage");
        }
        public static void RemoveFile(AssetInfo asset)
        {
            File.Delete(InternalEditorUtility.unityPreferencesFolder + Path.DirectorySeparatorChar + $"../../Asset Store-5.x/{asset.Name}[{asset.Version}].unitypackage");
            asset.Progress = 0;
        }
        public void ClearFolder()
        {
            foreach (AssetInfo asset in AssetListUnfiltered)
            {
                if (asset.Downloaded) RemoveFile(asset);
            }
        }
        public static async void DownloadAsset(AssetInfo asset)
        {
            if (IsFileExists(asset))
            {
                AssetDatabase.ImportPackage(InternalEditorUtility.unityPreferencesFolder + Path.DirectorySeparatorChar + $"../../Asset Store-5.x/{asset.Name}[{asset.Version}].unitypackage", true);

            }
            else
            {
                UnityWebRequest dowloadRequest = new UnityWebRequest(asset.Path);
                dowloadRequest.downloadHandler = new DownloadHandlerFile(InternalEditorUtility.unityPreferencesFolder + Path.DirectorySeparatorChar + $"../../Asset Store-5.x/{asset.Name}[{asset.Version}].unitypackage");
                string authorization = Authenticate();
                dowloadRequest.SetRequestHeader("AUTHORIZATION", authorization);
                UnityWebRequestAsyncOperation downloadOperation = dowloadRequest.SendWebRequest();

                while (!downloadOperation.isDone)
                {
                    asset.Progress = dowloadRequest.downloadedBytes / 1000f / 1000f;
                    await Task.Yield();
                }

                if (dowloadRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(dowloadRequest.error);
                }

                dowloadRequest.Dispose();

                AssetDatabase.ImportPackage(InternalEditorUtility.unityPreferencesFolder + Path.DirectorySeparatorChar + $"../../Asset Store-5.x/{asset.Name}[{asset.Version}].unitypackage", true);
            }
        }
        private void OnInspectorUpdate() => Repaint();
        private void DrawHeader()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            EditorGUI.BeginDisabledGroup(RefreshOP != null && !RefreshOP.IsCompleted);
            if (GUILayout.Button("Reload", EditorStyles.toolbarButton))
            {
                RefreshOP = Refresh();
            }
            EditorGUI.EndDisabledGroup();
            category = EditorGUILayout.Popup(category, Settings.Categories.Select(o => o.Name).ToArray(), EditorStyles.toolbarDropDown, GUILayout.Width(100));
            search = EditorGUILayout.TextField(search, EditorStyles.toolbarSearchField, GUILayout.MaxWidth(350));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Open Local Folder", EditorStyles.toolbarButton))
            {
                Folder();
            }
            if (GUILayout.Button("Clear Local Folder", EditorStyles.toolbarButton))
            {
                ClearFolder();
            }
            if (GUILayout.Button("Open in Browser", EditorStyles.toolbarButton))
            {
                Browser();
            }
            EditorGUILayout.EndHorizontal();
        }

        protected override void OnGUI()
        {
            GetSettings();
            DrawHeader();
            base.OnGUI();
        }
    }
}

