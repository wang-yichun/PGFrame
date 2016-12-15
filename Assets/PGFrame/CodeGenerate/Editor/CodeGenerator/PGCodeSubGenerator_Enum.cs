using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PGFrame
{

	using Newtonsoft.Json.Linq;

	public class PGCodeSubGenerator_Enum: IPGCodeSubGenerator
	{
		public PGCodeSubGenerator_Enum ()
		{
		}

		public PGCodeSubGenerator_Enum (string templateFileName)
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
			string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/Enum");
			string code = File.ReadAllText (templateFileInfo.FullName);
			code = code.Replace ("__XXX__", elementName);
			code = code.Replace ("__WWW__", workspaceName);

			code = code.Replace (E_I, GetItemCode (jo));
			code = code.Replace (E_C, GetCommentCode (jo));

			if (!Directory.Exists (targetPath))
				Directory.CreateDirectory (targetPath);
			
			string file = Path.Combine (targetPath, string.Format ("{0}.cs", elementName));
			File.WriteAllText (file, code);
			filesGenerated.Add (file);
		}

		#endregion

		public static readonly string E_I = @"/****enum_item****/";
		public static readonly string E_C = @"/****enum_comment****/";

		public string GetItemCode (JObject jo)
		{
			StringBuilder sb_sc_member = new StringBuilder ();

			string sc_name = jo ["Common"] ["Name"].Value<string> ();
			JArray ja_members = jo ["Member"] as JArray;

			for (int i = 0; i < ja_members.Count; i++) {
				JObject jo_member = ja_members [i] as JObject;
				string name = jo_member ["Name"].Value<string> ();
				int integer = jo_member ["Int"].Value<int> ();
				string desc = jo_member ["Desc"].Value<string> ();

				if (integer >= 0) {
					sb_sc_member.AppendFormat (@"

	/* {2} */
	{0} = {1}{3}", name, integer, desc, i == ja_members.Count - 1 ? "" : ",");
				} else {
					sb_sc_member.AppendFormat (@"

	/* {2} */
	{0}{3}", name, integer, desc, i == ja_members.Count - 1 ? "" : ",");
				}
			}

			return sb_sc_member.ToString ();
		}

		public string GetCommentCode (JObject jo)
		{
			return  jo ["Common"] ["Desc"].Value<string> ();
		}
	}
}