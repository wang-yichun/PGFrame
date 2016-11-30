using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

public class PGCodeSubGenerator_ElementEditor: IPGCodeSubGenerator
{
	public PGCodeSubGenerator_ElementEditor ()
	{
	}

	public PGCodeSubGenerator_ElementEditor (string templateFileName)
	{
		templateFileInfo = new FileInfo (templateFileName);
	}

	FileInfo templateFileInfo;

	#region IPGCodeSubGenerator implementation

	public bool CanGenerate (JObject jo)
	{
		return true;
	}

	public void GenerateCode (JObject jo, IList<string> filesGenerated)
	{
		string workspaceName = jo ["Workspace"].Value<string> ();
		string elementName = jo ["Common"] ["Name"].Value<string> ();
		string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/Editor");
		string code = File.ReadAllText (templateFileInfo.FullName);
		code = code.Replace ("__XXX__", elementName);
		code = code.Replace (VIEWMODEL_GUI, GetViewModelGUICode (jo));
		string file = Path.Combine (targetPath, string.Format ("{0}ElementEditor.cs", elementName));
		File.WriteAllText (file, code);
		filesGenerated.Add (file);
	}

	#endregion

	public static readonly string VIEWMODEL_GUI = @"/****VIEWMODEL_GUI****/";

	public string GetViewModelGUICode (JObject jo)
	{
		string baseType = jo ["Common"] ["Type"].Value<string> ();

		StringBuilder sb = new StringBuilder ();

		if (string.IsNullOrEmpty (baseType) == false) {
			sb.Append (string.Format (@"
		EditorGUILayout.BeginVertical (""box"");
		{0}ElementEditor baseElementEditor = new {0}ElementEditor ();
		baseElementEditor.VM = VM as {0}ViewModel;
		baseElementEditor.InspectorGUI_ViewModel ();
		EditorGUILayout.EndVertical ();", baseType));
		}

		sb.Append ("\n\n\t\tstring vmk;");

		JArray ja = (JArray)jo ["Member"];
		for (int i = 0; i < ja.Count; i++) {
			JObject jom = (JObject)ja [i];
			sb.Append (jom.GenEditorCode ());
		}

		return sb.ToString ();
	}
}

public static class GenCode_ElementEditor
{
	public static string GenEditorCode (this JObject jom)
	{
		string result = string.Empty;
		switch (jom ["RxType"].Value<string> ()) {
		case "Property":
			result = jom.GenEditorGUIProperty ();
			break;
		case "Collection":
			result = jom.GenEditorGUICollection ();
			break;
		case "Dictionary":
			result = jom.GenEditorGUIDictionary ();
			break;
		case "Command":
			result = jom.GenEditorGUICommand ();
			break;
		default:
			break;
		}
		return result;
	}

	public static string GenEditorGUIProperty (this JObject jom)
	{
		string name = jom ["Name"].Value<string> ();
		string type = jom ["Type"].Value<string> ();
		string result = "";

		switch (type) {
		case "int":
			result = string.Format (@"

		vmk = ""{0}"";
		int temp{0} = EditorGUILayout.DelayedIntField (vmk, VM.{0});
		if (temp{0} != VM.{0}) {{
			VM.{0} = temp{0};
		}}", name);
			break;
		case "long":
			result = string.Format (@"

		vmk = ""{0}"";
		int temp{0} = EditorGUILayout.DelayedIntField (vmk, (int)VM.{0});
		if ((long)temp{0} != VM.{0}) {{
			VM.{0} = (long)temp{0};
		}}", name);
			break;
		case "float":
			result = string.Format (@"

		vmk = ""{0}"";
		float temp{0} = EditorGUILayout.DelayedFloatField (vmk, VM.{0});
		if (temp{0} != VM.{0}) {{
			VM.{0} = temp{0};
		}}", name);
			break;
		case "double":
			result = string.Format (@"

		vmk = ""{0}"";
		double temp{0} = EditorGUILayout.DelayedDoubleField (vmk, VM.{0});
		if (temp{0} != VM.{0}) {{
			VM.{0} = temp{0};
		}}", name);
			break;
		case "string":
			result = string.Format (@"

		vmk = ""{0}"";
		string temp{0} = EditorGUILayout.DelayedTextField (vmk, VM.{0});
		if (temp{0} != VM.{0}) {{
			VM.{0} = temp{0};
		}}", name);
			break;
//		case "DateTime":
//		case "JObject":
//		case "JArray":
//			break;
		default:
			result = string.Format (@"
		vmk = ""{0}"";
		EditorGUILayout.DelayedTextField (vmk, ""({1})"");", name, type);
			break;
		}

		return result;
	}

	public static string GenEditorGUICollection (this JObject jom)
	{
		string name = jom ["Name"].Value<string> ();
		string type = jom ["Type"].Value<string> ();

		string result = string.Format (@"

		vmk = ""{0}"";
		EditorGUILayout.BeginHorizontal ();
		string {0}Json = JsonConvert.SerializeObject (VM.{0});
		string temp{0}Json = EditorGUILayout.DelayedTextField (vmk, {0}Json);
		if (temp{0}Json != {0}Json) {{
			if (string.IsNullOrEmpty (temp{0}Json)) {{
				VM.{0} = null;
			}} else {{
				VM.{0} = JsonConvert.DeserializeObject<ReactiveCollection<{1}>> (temp{0}Json);
			}}
		}}
		if (GUILayout.Button (""..."", GUILayout.MaxWidth (20))) {{
			PopupWindow.Show (
				new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
				new ReactiveCollectionEditorPopupWindow<{1}> (this, VM.{0})
			);
		}}
		EditorGUILayout.EndHorizontal ();", name, type);

		return result;
	}

	public static string GenEditorGUIDictionary (this JObject jom)
	{
		string name = jom ["Name"].Value<string> ();
		string type = jom ["Type"].Value<string> ();
		string[] types = type.Split (new char[]{ ',' });

		string result = string.Format (@"

		vmk = ""{0}"";
		EditorGUILayout.BeginHorizontal ();
		string {0} = JsonConvert.SerializeObject (VM.{0});
		string temp{0} = EditorGUILayout.DelayedTextField (vmk, {0});
		if (temp{0} != {0}) {{
			if (string.IsNullOrEmpty (temp{0})) {{
				VM.{0} = null;
			}} else {{
				VM.{0} = JsonConvert.DeserializeObject<ReactiveDictionary<{1},{2}>> (temp{0});
			}}
		}}
		if (GUILayout.Button (""..."", GUILayout.MaxWidth (20))) {{
			PopupWindow.Show (
				new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
				new ReactiveDictionaryEditorPopupWindow<{1},{2}> (this, VM.{0})
			);
		}}
		EditorGUILayout.EndHorizontal ();", name, types [0], types [1]);

		return result;
	}

	public static string GenEditorGUICommand (this JObject jom)
	{
		string name = jom ["Name"].Value<string> ();
		JArray para = jom ["Params"] as JArray;

		string result;

		if (para != null && para.Count > 0) {
			result = string.Format (@"

		vmk = ""{0}"";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button (""Params"")) {{
			if (CommandParams.ContainsKey (vmk)) {{
				CommandParams.Remove (vmk);
			}} else {{
				CommandParams [vmk] = JsonConvert.SerializeObject (new {0}Command (), Formatting.Indented);
			}}
		}}
		if (GUILayout.Button (""Invoke"")) {{
			if (CommandParams.ContainsKey (vmk) == false) {{
				VM.RC_{0}.Execute (new {0}Command () {{ Sender = VM }});
			}} else {{
				{0}Command command = JsonConvert.DeserializeObject<{0}Command> (CommandParams [vmk]);
				command.Sender = VM;
				VM.RC_{0}.Execute (command);
			}}
		}}
		EditorGUILayout.EndHorizontal ();
		if (CommandParams.ContainsKey (vmk)) {{
			CommandParams [vmk] = EditorGUILayout.TextArea (CommandParams [vmk]);
			EditorGUILayout.Space ();
		}}", name);
		} else {
			result = string.Format (@"

		vmk = ""{0}"";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button (""Invoke"")) {{
			VM.RC_{0}.Execute ();
		}}
		EditorGUILayout.EndHorizontal ();", name);
		}

		return result;
	}
}