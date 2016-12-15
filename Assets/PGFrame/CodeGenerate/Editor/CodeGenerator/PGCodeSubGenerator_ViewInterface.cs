using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PGFrame
{
	using Newtonsoft.Json.Linq;

	public class PGCodeSubGenerator_ViewInterface: IPGCodeSubGenerator
	{
		public PGCodeSubGenerator_ViewInterface ()
		{
		}

		public PGCodeSubGenerator_ViewInterface (string templateFileName)
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
			string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/View/_Interface");
			string code = File.ReadAllText (templateFileInfo.FullName);
			code = code.Replace ("__XXX__", elementName);
			code = code.Replace ("__WWW__", workspaceName);
			code = code.Replace (VM_PROPERTY_VIEW, GetVMPropertyViewCode (jo));
			string file = Path.Combine (targetPath, string.Format ("I{0}View.cs", elementName));
			File.WriteAllText (file, code);
			filesGenerated.Add (file);
		}

		#endregion

		public static readonly string VM_PROPERTY_VIEW = @"/****vm_property_view****/";

		public string GetVMPropertyViewCode (JObject jo)
		{
			StringBuilder sb = new StringBuilder ();

			string ws_name = jo ["Workspace"].Value<string> ();
			JArray ja_members = jo ["Member"] as JArray;
			for (int i = 0; i < ja_members.Count; i++) {
				JObject jo_member = ja_members [i] as JObject;
				RxType rt = (RxType)Enum.Parse (typeof(RxType), jo_member ["RxType"].Value<string> ());
				if (rt == RxType.Property) {
					string member_name = jo_member ["Name"].Value<string> ();
					string member_type = jo_member ["Type"].Value<string> ();
					DocType? dt = PGFrameTools.GetDocTypeByWorkspaceAndType (ws_name, member_type);
					if (dt.HasValue && dt.Value == DocType.Element) {
						string element_name = member_type.ConvertToElementName ();
						sb.AppendFormat (@"
	I{1}View {0}View {{ get; set; }}", member_name, element_name);
					}
				}
			}

			return sb.ToString ();
		}
	}
}