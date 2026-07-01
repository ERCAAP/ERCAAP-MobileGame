#if UNITY_IOS
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;
using System.Collections;

public class XcodeSettingsProcesser : MonoBehaviour
{
     [PostProcessBuildAttribute (0)]
    public static void OnPostprocessBuild (BuildTarget buildTarget, string pathToBuiltProject)
    {

        // Stop processing if targe is NOT iOS
        if (buildTarget != BuildTarget.iOS)
            return; 

        var plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistPath));
        plist.root.SetString("UIUserInterfaceStyle", "Dark");
        File.WriteAllText(plistPath, plist.WriteToString());

        // Initialize PbxProject
        var projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject pbxProject = new PBXProject ();
        pbxProject.ReadFromFile (projectPath);
        string targetMain = pbxProject.GetUnityMainTargetGuid();
        string targetFrmwk = pbxProject.GetUnityFrameworkTargetGuid();

      

        // Sample of setting build property
        pbxProject.SetBuildProperty(targetMain, "ENABLE_BITCODE", "NO");
        pbxProject.SetBuildProperty(targetMain, "DEFINES_MODULE", "YES");
        pbxProject.SetBuildProperty(targetMain, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");


        pbxProject.SetBuildProperty(targetFrmwk, "ENABLE_BITCODE", "NO");
        pbxProject.SetBuildProperty(targetFrmwk, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
        
       
        // Apply settings
        File.WriteAllText (projectPath, pbxProject.WriteToString ());

    

    }
}
#endif