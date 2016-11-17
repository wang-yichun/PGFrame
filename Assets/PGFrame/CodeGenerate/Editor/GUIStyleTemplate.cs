using UnityEngine;
using System.Collections;

public static class GUIStyleTemplate
{
	public static GUIStyle GreenDescStyle ()
	{
		GUIStyle s = new GUIStyle ();
		s.normal.textColor = (Color)(new Color32 (50, 177, 108, 255));
		return s;
	}
}
