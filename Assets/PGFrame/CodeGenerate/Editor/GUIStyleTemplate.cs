using UnityEngine;
using System.Collections;

public static class GUIStyleTemplate
{
	public static GUIStyle FSMTitleStyle ()
	{
		GUIStyle s = new GUIStyle (GUI.skin.label);
		s.fontSize = 20;
		s.fontStyle = FontStyle.Bold;
		return s;
	}

	public static GUIStyle GreenDescStyle ()
	{
		GUIStyle s = new GUIStyle ();
		s.normal.textColor = (Color)(new Color32 (50, 177, 108, 255));
		return s;
	}

	public static GUIStyle GreenDescStyle2 ()
	{
		GUIStyle s = new GUIStyle (GUI.skin.textArea);
		s.normal.textColor = (Color)(new Color32 (50, 177, 108, 255));
		s.focused.textColor = (Color)(new Color32 (50, 177, 108, 255));
		return s;
	}

	public static GUIStyle BlackCommandLink ()
	{
		GUIStyle s = new GUIStyle (GUI.skin.label);
		s.normal.textColor = Color.black;
		return s;
	}

	public static GUIStyle LabelStyle ()
	{
		GUIStyle s = new GUIStyle (GUI.skin.label);
		return s;
	}

	public static GUIStyle ButtonStyleAlignmentLeft ()
	{
		GUIStyle s = new GUIStyle (GUI.skin.button);
		s.alignment = TextAnchor.MiddleLeft;
		return s;
	}
}
