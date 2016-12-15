using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PGFrame
{
	using Newtonsoft.Json.Linq;
	using Newtonsoft.Json;

	public class PGCodeSubGenerator_ViewModelBase: IPGCodeSubGenerator
	{
		public PGCodeSubGenerator_ViewModelBase ()
		{
		}

		public PGCodeSubGenerator_ViewModelBase (string templateFileName)
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
			string baseName = jo ["Common"] ["Type"].Value<string> ();
			string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/ViewModel/_Base");
			string code = File.ReadAllText (templateFileInfo.FullName);
			code = code.Replace ("__XXX__", elementName);
			code = code.Replace ("__YYY__", string.IsNullOrEmpty (baseName) ? "ViewModelBase" : baseName + "ViewModel");
			code = code.Replace (REACTIVE_MEMBERS, GetReactiveMembers (jo));
			code = code.Replace (COMMAND_CLASS, GetCommandClass (jo));
			code = code.Replace (INITIALIZE_CODE, GetInitializeCode (jo));
			code = code.Replace (CLASS_COMMENT, GetClassDescription (jo));
			string file = Path.Combine (targetPath, string.Format ("{0}ViewModelBase.cs", elementName));
			File.WriteAllText (file, code);
			filesGenerated.Add (file);
		}

		#endregion

		public static readonly string INITIALIZE_CODE = @"/****initialize_code****/";
		public static readonly string REACTIVE_MEMBERS = @"/****reactive_members****/";
		public static readonly string COMMAND_CLASS = @"/****command_class****/";
		public static readonly string COMMAND_MEMBER = @"/****command_member****/";
		public static readonly string CLASS_COMMENT = @"/****class_comment****/";

		public string GetReactiveMembers (JObject jo)
		{
			StringBuilder sb = new StringBuilder ();
			JArray ja = (JArray)jo ["Member"];
			for (int i = 0; i < ja.Count; i++) {
				JObject jom = (JObject)ja [i];
				sb.Append ("\n");
				sb.Append (jom.GenReactiveMemberCode (jo));
//			PogoTools.PRDebug.TagLog ("GetReactiveMembers", Color.white, JsonConvert.SerializeObject (jom));
			}
			return sb.ToString ();
		}

		public string GetCommandClass (JObject jo)
		{
			StringBuilder sb = new StringBuilder ();
			JArray ja = (JArray)jo ["Member"];
			for (int i = 0; i < ja.Count; i++) {
				JObject jom = (JObject)ja [i];
				if (jom ["RxType"].Value<string> () == "Command") {
					string template = @"
public class {NAME}Command : ViewModelCommandBase
{
/****command_member****/
}
";
					template = template.Replace ("{NAME}", jom ["Name"].Value<string> ());

					StringBuilder sbc = new StringBuilder ();
					JArray jac = (JArray)jom ["Params"];
					if (jac != null) {
						for (int i2 = 0; i2 < jac.Count; i2++) {
							JObject jocm = (JObject)jac [i2];
							sbc.Append (jocm.GenCommandMember ());
						}
					}

					template = template.Replace (COMMAND_MEMBER, sbc.ToString ());
					sb.Append (template);
				}
			}
			return sb.ToString ();
		}

		public string GetInitializeCode (JObject jo)
		{
			StringBuilder sb = new StringBuilder ();
			JArray ja = (JArray)jo ["Member"];
			for (int i = 0; i < ja.Count; i++) {
				JObject jom = (JObject)ja [i];
				sb.Append (jom.GenInitializeCode ());
//			PogoTools.PRDebug.TagLog ("GenInitializeCode", Color.white, JsonConvert.SerializeObject (jom));
			}
			return sb.ToString ();
		}

		public string GetClassDescription (JObject jo)
		{
			return jo ["Common"] ["Desc"].Value<string> ();
		}
	}

	public static class GenCode_ViewModelBase
	{
		public static string GenReactiveMemberCode (this JObject jom, JObject jo)
		{
			string result = string.Empty;
			switch (jom ["RxType"].Value<string> ()) {
			case "Property":
				result = jom.GenReactiveMemberProperty (jo);
				break;
			case "Collection":
				result = jom.GenReactiveMemberCollection ();
				break;
			case "Dictionary":
				result = jom.GenReactiveMemberDictionary ();
				break;
			case "Command":
				result = jom.GenReactiveMemberCommand ();
				break;
			default:
				break;
			}
			return result;
		}

		public static string GenReactiveMemberProperty (this JObject jom, JObject jo)
		{
			string template = @"
	/* {DESC} */
	public ReactiveProperty<{TYPE}> RP_{NAME};

	{JSONPROPERTY}{JSONCONVERTER}
	public {TYPE} {NAME} {
		get {
			return RP_{NAME}.Value;
		}
		set {
			RP_{NAME}.Value = value;
		}
	}";
	
			DocType? dt = PGFrameTools.GetDocTypeByWorkspaceAndType (jo ["Workspace"].Value<string> (), jom ["Type"].Value<string> ());
			if (dt.HasValue && dt.Value == DocType.Element) {
				template = template.Replace ("{JSONPROPERTY}", string.Empty);
			} else {
				template = template.Replace ("{JSONPROPERTY}", "[JsonProperty]");
			}

			template = template.Replace ("{NAME}", jom ["Name"].Value<string> ());
			template = template.Replace ("{TYPE}", jom ["Type"].Value<string> ());
			template = template.Replace ("{DESC}", jom ["Desc"].Value<string> ());
			template = template.Replace ("{JSONCONVERTER}", GetAttributeCode (jom ["Type"].Value<string> ()));


			return template;
		}

		public static string GetAttributeCode (string type)
		{
			string code = "";
			switch (type) {
			case "UnityEngine.Color":
				code = "\n\t[JsonConverter (typeof(ColorJsonConverter))]";
				break;
			case "UnityEngine.Rect":
				code = "\n\t[JsonConverter (typeof(RectJsonConverter))]";
				break;
			case "UnityEngine.Bounds":
				code = "\n\t[JsonConverter (typeof(BoundsJsonConverter))]";
				break;
			case "UnityEngine.Quaternion":
				code = "\n\t[JsonConverter (typeof(QuaternionJsonConverter))]";
				break;
			default:
				break;
			}
			return code;
		}

		public static string GenReactiveMemberCollection (this JObject jom)
		{
			string template = @"
	/* {DESC} */
	[JsonProperty] public ReactiveCollection<{TYPE}> {NAME};";
			template = template.Replace ("{NAME}", jom ["Name"].Value<string> ());
			template = template.Replace ("{TYPE}", jom ["Type"].Value<string> ());
			template = template.Replace ("{DESC}", jom ["Desc"].Value<string> ());
			return template;
		}

		public static string GenReactiveMemberDictionary (this JObject jom)
		{
			string template = @"
	/* {DESC} */
	[JsonProperty] public ReactiveDictionary<{TYPE0}, {TYPE1}> {NAME};";
		
			template = template.Replace ("{NAME}", jom ["Name"].Value<string> ());
			string[] types = jom ["Type"].Value<string> ().Split (new char[]{ ',' });
			template = template.Replace ("{TYPE0}", types [0].Trim ());
			template = template.Replace ("{TYPE1}", types [1].Trim ());
			template = template.Replace ("{DESC}", jom ["Desc"].Value<string> ());
			return template;
		}

		public static string GenReactiveMemberCommand (this JObject jom)
		{
			JArray ja = (JArray)jom ["Params"];
			string template = string.Empty;
			if (ja != null && ja.Count > 0) {
				template = @"
	/* {DESC} */
	public ReactiveCommand<{NAME}Command> RC_{NAME};
	";
				template = template.Replace ("{NAME}", jom ["Name"].Value<string> ());
				template = template.Replace ("{DESC}", jom ["Desc"].Value<string> ());
			} else {
				template = @"
	/* {DESC} */
	public ReactiveCommand RC_{NAME};
	";
				template = template.Replace ("{NAME}", jom ["Name"].Value<string> ());
				template = template.Replace ("{DESC}", jom ["Desc"].Value<string> ());
			}
			return template;
		}

		public static string GenCommandMember (this JObject jom)
		{
			string template = @"
	/* {PARAMDESCRIPTION} */
	public {PARAMTYPE} {PARAMNAME};
	";
			template = template.Replace ("{PARAMDESCRIPTION}", jom ["Desc"].Value<string> ());
			template = template.Replace ("{PARAMTYPE}", jom ["Type"].Value<string> ());
			template = template.Replace ("{PARAMNAME}", jom ["Name"].Value<string> ());
			return template;
		}

		public static string GenInitializeCode (this JObject jom)
		{
			string result = string.Empty;
			switch (jom ["RxType"].Value<string> ()) {
			case "Property":
				result = jom.GenInitializeCodeProperty ();
				break;
			case "Collection":
				result = jom.GenInitializeCodeCollection ();
				break;
			case "Dictionary":
				result = jom.GenInitializeCodeDictionary ();
				break;
			case "Command":
				result = jom.GenInitializeCodeCommand ();
				break;
			default:
				break;
			}
			return result;
		}

		public static string GenInitializeCodeProperty (this JObject jom)
		{
			string template = @"
		RP_{NAME} = new ReactiveProperty<{TYPE}> ();";

			template = template.Replace ("{NAME}", jom ["Name"].Value<string> ());
			template = template.Replace ("{TYPE}", jom ["Type"].Value<string> ());
			return template;
		}

		public static string GenInitializeCodeCollection (this JObject jom)
		{
			string template = @"
		{NAME} = new ReactiveCollection<{TYPE}> ();";

			template = template.Replace ("{NAME}", jom ["Name"].Value<string> ());
			template = template.Replace ("{TYPE}", jom ["Type"].Value<string> ());
			return template;
		}

		public static string GenInitializeCodeDictionary (this JObject jom)
		{
			string template = @"
		{NAME} = new ReactiveDictionary<{TYPE0}, {TYPE1}> ();";

			template = template.Replace ("{NAME}", jom ["Name"].Value<string> ());
			string[] types = jom ["Type"].Value<string> ().Split (new char[]{ ',' });
			template = template.Replace ("{TYPE0}", types [0].Trim ());
			template = template.Replace ("{TYPE1}", types [1].Trim ());
			return template;
		}

		public static string GenInitializeCodeCommand (this JObject jom)
		{
			JArray ja = (JArray)jom ["Params"];
			string template = string.Empty;
			if (ja != null && ja.Count > 0) {
				template = @"
		RC_{NAME} = new ReactiveCommand<{NAME}Command> ();";
				template = template.Replace ("{NAME}", jom ["Name"].Value<string> ());
			} else {
				template = @"
		RC_{NAME} = new ReactiveCommand ();";
				template = template.Replace ("{NAME}", jom ["Name"].Value<string> ());
			}
			return template;
		}
	}
}