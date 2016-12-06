using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public class TextFieldPopupDialog : PopupWindowContent
{
	public TextFieldPopupDialog (string title, Action<string> confirmAction)
	{
		this.Title = title;
		this.ConfirmAction = confirmAction;
	}

	public string Title;
	public Action<string> ConfirmAction;
	public string Value;

	public override Vector2 GetWindowSize ()
	{
		return new Vector2 (200, 150);
	}

	public override void OnGUI (Rect rect)
	{
		GUILayout.Label (this.Title, EditorStyles.boldLabel);
		Value = EditorGUILayout.TextField (Value);
		if (GUILayout.Button ("OK")) {
			ConfirmAction.Invoke (Value);
			this.editorWindow.Close ();
		}
		if (GUILayout.Button ("Cancel")) {
			this.editorWindow.Close ();
		}
	}

	public override void OnOpen ()
	{
		Debug.Log ("Popup opened: " + this);
	}

	public override void OnClose ()
	{
		Debug.Log ("Popup closed: " + this);
	}
}
