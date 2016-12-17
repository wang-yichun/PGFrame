using UnityEngine;
using UnityEditor;
using System.Collections;

public class FSMWindow : EditorWindow
{
	public static readonly string lt = "PGFrame";
	public static readonly Color lc = new Color32 (0, 162, 255, 255);
	public static readonly Color lcr = new Color32 (255, 162, 162, 255);

	public static FSMWindow Current;

	public Texture2D pgf_window_title_icon;

	public void SetIcons ()
	{
		pgf_window_title_icon = Resources.Load<Texture2D> ("pgf_fsm_window_title_icon");
	}

	[MenuItem ("PogoRock/PGFrame/Finite State Machine... %_F2")]
	static void Init ()
	{
		FSMWindow window = (FSMWindow)EditorWindow.GetWindow (typeof(FSMWindow));
		window.SetIcons ();
		window.titleContent = new GUIContent (window.pgf_window_title_icon);
		window.Show ();
	}

	void OnGUI ()
	{
		GUI.Box (new Rect (0, 0, 200, 100), "");
		EditorGUI.DrawRect (new Rect (300, 0, 200, 100), Color.gray);
	}
}
