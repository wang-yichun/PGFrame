using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

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
		string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/ViewModel/_Base");
		string code = File.ReadAllText (templateFileInfo.FullName);
		code = code.Replace ("__XXX__", elementName);
		code = code.Replace (REACTIVE_MEMBERS, GetReactiveMembers (jo));
		string file = Path.Combine (targetPath, string.Format ("{0}ViewModelBase.cs", elementName));
		File.WriteAllText (file, code);
		filesGenerated.Add (file);
	}

	#endregion

	public static readonly string REACTIVE_MEMBERS = @"/****reactive_members****/";

	public string GetReactiveMembers (JObject jo)
	{
		StringBuilder sb = new StringBuilder ();
		JArray ja = (JArray)jo ["Member"];
		for (int i = 0; i < ja.Count; i++) {
			JObject jom = (JObject)ja [i];
			sb.Append ("\n");
			sb.Append (jom.GenReactiveMemberCode ());
			PogoTools.PRDebug.TagLog ("GetReactiveMembers", Color.white, JsonConvert.SerializeObject (jom));
		}
		return sb.ToString ();
	}
}

public static class GenCode_ViewModelBase
{
	public static string GenReactiveMemberCode (this JObject jom)
	{
		string result = string.Empty;
		switch (jom ["RxType"].Value<string> ()) {
		case "Property":
			result = jom.GenReactiveMemberProperty ();
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

	public static string GenReactiveMemberProperty (this JObject jom)
	{
		string template = @"
	public ReactiveProperty<{TYPE}> RP_{NAME};
	
	[JsonProperty]
	public {TYPE} {NAME} {
		get {
			return RP_{NAME}.Value;
		}
		set {
			RP_{NAME}.Value = value;
		}
	}";
	
		template = template.Replace ("{NAME}", jom ["Name"].Value<string> ());
		template = template.Replace ("{TYPE}", jom ["Type"].Value<string> ());
		return template;
	}

	public static string GenReactiveMemberCollection (this JObject jom)
	{
		string template = @"
	[JsonProperty]
	public ReactiveCollection<{TYPE}> {NAME};
	";
		template = template.Replace ("{NAME}", jom ["Name"].Value<string> ());
		template = template.Replace ("{TYPE}", jom ["Type"].Value<string> ());
		return template;
	}

	public static string GenReactiveMemberDictionary (this JObject jom)
	{
		string template = @"
	[JsonProperty]
	public ReactiveDictionary<{TYPE}> {NAME};
	";
		
		template = template.Replace ("{NAME}", jom ["Name"].Value<string> ());
		template = template.Replace ("{TYPE}", jom ["Type"].Value<string> ());
		return template;
	}

	public static string GenReactiveMemberCommand (this JObject jom)
	{
		JArray ja = (JArray)jom ["Params"];
		string template = string.Empty;
		if (ja.Count > 0) {
			template = @"
	public ReactiveCommand<{NAME}Command> RC_{NAME};
	";
			template = template.Replace ("{NAME}", jom ["Name"].Value<string> ());
		} else {
			template = @"
	public ReactiveCommand RC_{NAME};
	";
			template = template.Replace ("{NAME}", jom ["Name"].Value<string> ());
		}

		return template;
	}
}
