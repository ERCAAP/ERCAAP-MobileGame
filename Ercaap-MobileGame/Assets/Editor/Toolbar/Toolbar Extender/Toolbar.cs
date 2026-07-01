using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEngine.UIElements;

namespace ToolbarToolkit
{
	public static class Toolbar
	{
		static Type m_toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
		static ScriptableObject m_currentToolbar;

		public static VisualElement LeftUI;
		public static VisualElement RightUI;

		static Toolbar()
		{
			EditorApplication.update -= OnUpdateUITK;
			EditorApplication.update += OnUpdateUITK;
		}
		static void OnUpdateUITK()
		{
			if(LeftUI == null || RightUI == null) return;
			// Relying on the fact that toolbar is ScriptableObject and gets deleted when layout changes
			if (m_currentToolbar == null)
			{
				// Find toolbar
				var toolbars = Resources.FindObjectsOfTypeAll(m_toolbarType);
				m_currentToolbar = toolbars.Length > 0 ? (ScriptableObject)toolbars[0] : null;
				if (m_currentToolbar != null)
				{
					var root = m_currentToolbar.GetType().GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
					var rawRoot = root.GetValue(m_currentToolbar);
					var mRoot = rawRoot as VisualElement;
					RegisterCallback("ToolbarZoneLeftAlign", LeftUI);
					MidFix("ToolbarZonePlayMode");
					RegisterCallback("ToolbarZoneRightAlign", RightUI);

					void RegisterCallback(string root, VisualElement VisualElement)
					{
						var toolbarZone = mRoot.Q(root);

						var parent = new VisualElement()
						{
							style = {
								flexGrow = 1,
								flexDirection = FlexDirection.Row,
							}
						};
						parent.Add(VisualElement);
						toolbarZone.Add(parent);
					}
					void MidFix(string root)
					{
						var toolbarButtonsArea = mRoot.Q(root);
						var toolbarButtons = mRoot.Q(root).ElementAt(0);
						toolbarButtonsArea.style.paddingBottom = 4;
						toolbarButtonsArea.style.paddingTop = 4;
						toolbarButtons.style.height = new StyleLength(StyleKeyword.Auto);
					}
				}
			}
		}
	}
}
