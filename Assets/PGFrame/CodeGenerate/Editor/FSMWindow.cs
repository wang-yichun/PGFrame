using UnityEngine;
using UnityEditor;
using UnityEditor.Graphs;
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

	Rect windowRect = new Rect (100 + 100, 100, 100, 100);
	Rect windowRect2 = new Rect (100, 100, 100, 100);

	Vector2 scrollPosition;

	private void OnGUI ()
	{
		scrollPosition = GUILayout.BeginScrollView (scrollPosition);

		Handles.BeginGUI ();
		Handles.DrawBezier (windowRect.center, windowRect2.center, new Vector2 (windowRect.xMax + 50f, windowRect.center.y), new Vector2 (windowRect2.xMin - 50f, windowRect2.center.y), Color.red, null, 5f);
		Handles.EndGUI ();

		BeginWindows ();
		windowRect = GUI.Window (0, windowRect, WindowFunction, "Box1");
		windowRect2 = GUI.Window (1, windowRect2, WindowFunction, "Box2");

		EndWindows ();

		GUILayout.EndScrollView ();
	}

	void WindowFunction (int windowID)
	{
		GUILayout.BeginVertical ();
		GUILayout.Label ("Hello World");
		GUILayout.EndVertical ();

		GUI.DragWindow ();
	}
}
