using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PGFrame
{
	using Newtonsoft.Json.Linq;

	public class PGCodeSubGenerator_Controller: IPGCodeSubGenerator
	{
		public PGCodeSubGenerator_Controller ()
		{
		}

		public PGCodeSubGenerator_Controller (string templateFileName)
		{
			templateFileInfo = new FileInfo (templateFileName);
		}

		FileInfo templateFileInfo;

		#region IPGCodeSubGenerator implementation

		public bool CanGenerate (JObject jo)
		{
			string workspaceName = jo ["Workspace"].Value<string> ();
			string elementName = jo ["Common"] ["Name"].Value<string> ();
			string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/Controller");
			string file = Path.Combine (targetPath, string.Format ("{0}Controller.cs", elementName));
			return !File.Exists (file);
		}

		public void GenerateCode (JObject jo, IList<string> filesGenerated)
		{
			string workspaceName = jo ["Workspace"].Value<string> ();
			string elementName = jo ["Common"] ["Name"].Value<string> ();
			string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/Controller");
			string code = File.ReadAllText (templateFileInfo.FullName);
			code = code.Replace ("__XXX__", elementName);
			code = code.Replace ("__WWW__", workspaceName);
			code = code.Replace (MEMBER_FUNCTION, GetMemberFunction (jo));

			if (!Directory.Exists (targetPath))
				Directory.CreateDirectory (targetPath);

			string file = Path.Combine (targetPath, string.Format ("{0}Controller.cs", elementName));
			File.WriteAllText (file, code);
			filesGenerated.Add (file);
		}

		#endregion

		public static readonly string MEMBER_FUNCTION = @"/****member_function****/";

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
					if (jap != null && jap.Count > 0) {
						template = @"
		/* {DESC} */
		public override void {NAME} ({ELEMENTNAME}ViewModel viewModel, {NAME}Command command)
		{
			base.{NAME} (viewModel, command);
		}";
					} else {
						template = @"
		/* {DESC} */
		public override void {NAME} ({ELEMENTNAME}ViewModel viewModel)
		{
			base.{NAME} (viewModel);
		}";
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
}