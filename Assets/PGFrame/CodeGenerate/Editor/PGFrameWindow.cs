using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PogoTools;
using Newtonsoft.Json;

public class PGFrameWindow : EditorWindow
{
	public static readonly string lt = "PGFrame";
	public static readonly Color lc = new Color (.6f, .4f, 1f);

	[MenuItem ("PogoRock/PGFrame...")]
	static void Init ()
	{
		PGFrameWindow window = (PGFrameWindow)EditorWindow.GetWindow (typeof(PGFrameWindow));
		window.Show ();
	}

	void OnGUI ()
	{
		GUILayout.Label ("PGFrame", EditorStyles.boldLabel);
		if (GUILayout.Button ("刷新")) {
			RefreshFiles ();
		}
		DesignList ();

		if (GUILayout.Button ("发布代码")) {
			Generator.GenerateCode ("WS1", "Second");
			AssetDatabase.Refresh ();
		}

		if (GUILayout.Button ("删除代码")) {
			Generator.DeleteCode ("WS1", "Second");
			AssetDatabase.Refresh ();
		}
	}

	FileInfo[] files;

	PGCodeGenerater Generator;

	void RefreshFiles ()
	{
		Generator = new PGCodeGenerater ();
		Generator.Init ();

		string[] fileNames = Directory.GetFiles (Path.Combine (Application.dataPath, "PGFrameDesign"), "*.xlsx");
		files = new FileInfo[fileNames.Length];
		for (int i = 0; i < fileNames.Length; i++) {
			files [i] = new FileInfo (fileNames [i]);
		}
	}

	void DesignList ()
	{
		GUILayout.BeginVertical ();

		if (files != null) {
			for (int i = 0; i < files.Length; i++) {
				string fileName = files [i].Name;
				EditorGUILayout.LabelField (fileName);
			}
		} else {
			RefreshFiles ();
		}

		GUILayout.EndVertical ();
	}


}
