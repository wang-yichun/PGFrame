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

		string[] ts = type.Split (new char[]{ '.' });
		string workspace = "";
		string single_name = "";
		if (ts.Length == 1) {
			workspace = PGFrameWindow.Current.SelectedWorkspace.Name;
			single_name = ts [0];
		} else if (ts.Length == 2) {
			workspace = ts [0];
			single_name = ts [1];
		}

		bool is_ese = false;
		if (string.IsNullOrEmpty (workspace) == false && string.IsNullOrEmpty (single_name) == false) {
			if (PGFrameWindow.Current != null) {
				DocType? dt = PGFrameWindow.Current.CommonManager.GetTheDocTypeByName (workspace, single_name);
				if (dt != null) {
					switch (dt.Value) {
					case DocType.Element:
						// PR_TODO:
//						result = string.Format (@"
//
//		vmk = ""{0}"";
//		ViewBase {0}View = (target as GameCoreView).{0}View;
//		ViewBase temp{0}View = (ViewBase)EditorGUILayout.ObjectField (vmk, {0}View, typeof(PlayerInfoView), true);
//		if ({0}View != temp{0}View) {{
//			(target as GameCoreView).{0}View = tempMyInfoView;
//			VM.MyInfo = ((PlayerInfoView)tempMyInfoView).VM;
//						}}", name, type);
//						is_ese = true;
						break;
					case DocType.SimpleClass:
						result = string.Format (@"
						
		vmk = ""{0}"";
		EditorGUILayout.BeginHorizontal ();
		if (VM.{0} == null) {{
			EditorGUILayout.PrefixLabel (vmk);
			if (GUILayout.Button (""Create"")) {{
				VM.{0} = new {1} ();
			}}
		}} else {{
			string {0} = JsonConvert.SerializeObject (VM.{0});
			string temp{0} = EditorGUILayout.DelayedTextField (vmk, {0});
			if (temp{0} != {0}) {{
				if (string.IsNullOrEmpty (temp{0})) {{
					VM.{0} = null;
				}} else {{
					VM.{0} = JsonConvert.DeserializeObject<{1}> ({0});
				}}
			}}
			if (GUILayout.Button (""..."", GUILayout.MaxWidth (20))) {{
				PopupWindow.Show (
					new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
					new SimpleClassReactivePropertyEditorPopupWindow<{1}> (this, VM.RP_{0})
				);
			}}
		}}
		EditorGUILayout.EndHorizontal ();", name, type);
						is_ese = true;
						break;
					case DocType.Enum:
						result = string.Format (@"

		vmk = ""{0}"";
		VM.{0} = ({1})EditorGUILayout.EnumPopup (vmk, VM.{0});", name, type);
						is_ese = true;
						break;
					default:
						throw new System.ArgumentOutOfRangeException ();
					}
				}
			}
		} 

		if (is_ese == false) {
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
			case "UnityEngine.Vector2":
				result = string.Format (@"

		vmk = ""{0}"";
		VM.{0} = EditorGUILayout.Vector2Field (vmk, VM.{0});", name);
				break;
			case "UnityEngine.Vector3":
				result = string.Format (@"

		vmk = ""{0}"";
		VM.{0} = EditorGUILayout.Vector3Field (vmk, VM.{0});", name);
				break;
			case "UnityEngine.Vector4":
				result = string.Format (@"

		vmk = ""{0}"";
		VM.{0} = EditorGUILayout.Vector4Field (vmk, VM.{0});", name);
				break;

			case "UnityEngine.Quaternion":
				result = string.Format (@"

		vmk = ""{0}"";
		Vector3 temp{0}Vector3 = VM.{0}.eulerAngles;
		temp{0}Vector3 = EditorGUILayout.Vector3Field (vmk, temp{0}Vector3);
		VM.{0} = Quaternion.Euler (temp{0}Vector3);", name);
				break;

			case "UnityEngine.Rect":
				result = string.Format (@"

		vmk = ""{0}"";
		VM.{0} = EditorGUILayout.RectField (vmk, VM.{0});", name);
				break;

			case "UnityEngine.Bounds":
				result = string.Format (@"

		vmk = ""{0}"";
		VM.{0} = EditorGUILayout.BoundsField (vmk, VM.{0});", name);
				break;

			case "UnityEngine.Color":
				result = string.Format (@"

		vmk = ""{0}"";
		VM.{0} = EditorGUILayout.ColorField (vmk, VM.{0});", name);
				break;

			case "UnityEngine.AnimationCurve":
				result = string.Format (@"
			
		vmk = ""{0}"";
		EditorGUILayout.BeginHorizontal ();
		if (VM.{0} == null) {{
			EditorGUILayout.PrefixLabel (vmk);
			if (GUILayout.Button (""Create"")) {{
				VM.{0} = AnimationCurve.Linear (0f, 0f, 1f, 1f);
			}}
		}} else {{
			VM.{0} = EditorGUILayout.CurveField (vmk, VM.{0});
			if (GUILayout.Button (""-"", GUILayout.MaxWidth (20f))) {{
				VM.{0} = null;
			}}
		}}
		EditorGUILayout.EndHorizontal ();", name);
				break;

			case "DateTime":
				result = string.Format (@"

		vmk = ""{0}"";
		DateTime temp{0};
		if (DateTime.TryParse (EditorGUILayout.DelayedTextField (vmk, VM.{0}.ToString ()), out temp{0})) {{
			if (VM.{0} != temp{0}) {{
				VM.{0} = temp{0};
			}}
		}}", name);
				break;

			case "TimeSpan":
				result = string.Format (@"

		vmk = ""{0}"";
		TimeSpan temp{0};
		if (TimeSpan.TryParse (EditorGUILayout.DelayedTextField (vmk, VM.{0}.ToString ()), out temp{0})) {{
			if (VM.{0} != temp{0}) {{
				VM.{0} = temp{0};
			}}
		}}", name);
				break;

			case "JObject":
				result = string.Format (@"

		vmk = ""{0}"";
		string temp{0}String = JsonConvert.SerializeObject (VM.{0});
		string temp2{0}String = EditorGUILayout.DelayedTextField (vmk, temp{0}String);
		if (temp{0}String != temp2{0}String) {{
			try {{
				VM.{0} = JsonConvert.DeserializeObject<JObject> (temp2{0}String);
			}} catch {{
				VM.{0} = JsonConvert.DeserializeObject<JObject> (temp{0}String);
			}}
		}}", name);
				break;

			case "JArray":
				result = string.Format (@"

		vmk = ""{0}"";
		string temp{0}String = JsonConvert.SerializeObject (VM.{0});
		string temp2{0}String = EditorGUILayout.DelayedTextField (vmk, temp{0}String);
		if (temp{0}String != temp2{0}String) {{
			try {{
				VM.{0} = JsonConvert.DeserializeObject<JArray> (temp2{0}String);
			}} catch {{
				VM.{0} = JsonConvert.DeserializeObject<JArray> (temp{0}String);
			}}
		}}", name);
				break;

			default:
				result = string.Format (@"

		vmk = ""{0}"";
		EditorGUILayout.DelayedTextField (vmk, VM.{0} != null ? VM.{0}.ToString () : ""null ({1})"");", name, type);
				break;
			}
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