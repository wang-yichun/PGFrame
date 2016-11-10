using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;

public class PGCodeSubGenerator_ControllerBase: IPGCodeSubGenerator
{
	public PGCodeSubGenerator_ControllerBase ()
	{
	}

	public PGCodeSubGenerator_ControllerBase (string templateFileName)
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
		string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/Controller/_Base");
		string code = File.ReadAllText (templateFileInfo.FullName);
		code = code.Replace ("__XXX__", elementName);
		code = code.Replace (ATTACH_CODE, GetAttachCode (jo));
		string mf = GetMemberFunction (jo);
		code = code.Replace (MEMBER_FUNCTION, mf);
		string file = Path.Combine (targetPath, string.Format ("{0}ControllerBase.cs", elementName));
		File.WriteAllText (file, code);
		filesGenerated.Add (file);
	}

	#endregion

	public static readonly string ATTACH_CODE = @"/****attach_code****/";
	public static readonly string MEMBER_FUNCTION = @"/****member_function****/";

	public string GetAttachCode (JObject jo)
	{
		StringBuilder sb = new StringBuilder ();
		string elementName = jo ["Common"] ["Name"].Value<string> ();
		JArray ja = (JArray)jo ["Member"];
		for (int i = 0; i < ja.Count; i++) {
			JObject jom = (JObject)ja [i];
			if (jom ["RxType"].Value<string> () == "Command") {
				string template;
				JArray jap = (JArray)jom ["Params"];
				if (jap.Count > 0) {
					template = @"
		vm.RC_{NAME}.Subscribe<{NAME}Command> (command => {
			command.Sender = viewModel;
			{NAME} (({ELEMENTNAME}ViewModel)viewModel, command);
		});";
				} else {
					template = @"
		vm.RC_{NAME}.Subscribe (_ => {
			{NAME} (({ELEMENTNAME}ViewModel)viewModel);
		});";
				}
				template = template.Replace ("{ELEMENTNAME}", elementName);
				template = template.Replace ("{NAME}", jom ["Name"].Value<string> ());
				sb.Append (template);
			}
		}
		return sb.ToString ();
	}

	public string GetMemberFunction (JObject jo)
	{
		StringBuilder sb = new StringBuilder ();
		string elementName = jo ["Common"] ["Name"].Value<string> ();
		JArray ja = (JArray)jo ["Member"];
		for (int i = 0; i < ja.Count; i++) {
			JObject jom = (JObject)ja [i];
			if (jom ["RxType"].Value<string> () == "Command") {
				string template;
				JArray jap = (JArray)jom ["Params"];
				if (jap.Count > 0) {
					template = "\n\t/* {DESC} */\n\tpublic virtual void {NAME} ({ELEMENTNAME}ViewModel viewModel, {NAME}Command command)\n\t{\n\t}";
				} else {
					template = "\n\t/* {DESC} */\n\tpublic virtual void {NAME} ({ELEMENTNAME}ViewModel viewModel)\n\t{\n\t}";
				}
				template = template.Replace ("{ELEMENTNAME}", elementName);
				template = template.Replace ("{NAME}", jom ["Name"].Value<string> ());
				template = template.Replace ("{DESC}", jom ["Desc"].Value<string> ());
				sb.Append (template);
			}
		}
		return sb.ToString ();
	}
}
