using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PGFrame
{
	using Newtonsoft.Json.Linq;

	public class PGCodeSubGenerator_SimpleClassBase: IPGCodeSubGenerator
	{
		public PGCodeSubGenerator_SimpleClassBase ()
		{
		}

		public PGCodeSubGenerator_SimpleClassBase (string templateFileName)
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
			string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/SimpleClass/_Base");
			string code = File.ReadAllText (templateFileInfo.FullName);
			code = code.Replace ("__XXX__", elementName);
			code = code.Replace ("__YYY__", string.IsNullOrEmpty (baseName) ? "SimpleClassBase" : baseName);
			code = code.Replace ("__WWW__", workspaceName);

			code = code.Replace (SC_MEMBER, GetMemberCode (jo));
			code = code.Replace (SC_COMMENT, GetCommentCode (jo));

			if (!Directory.Exists (targetPath))
				Directory.CreateDirectory (targetPath);

			string file = Path.Combine (targetPath, string.Format ("{0}Base.cs", elementName));
			File.WriteAllText (file, code);
			filesGenerated.Add (file);
		}

		#endregion

		public static readonly string SC_MEMBER = @"/****simple_class_member****/";
		public static readonly string SC_COMMENT = @"/****simple_class_comment****/";

		public string GetMemberCode (JObject jo)
		{
			StringBuilder sb_sc_member = new StringBuilder ();

//			string sc_name = jo ["Common"] ["Name"].Value<string> ();
			JArray ja_members = jo ["Member"] as JArray;

			for (int i = 0; i < ja_members.Count; i++) {
				JObject jo_member = ja_members [i] as JObject;
				string name = jo_member ["Name"].Value<string> ();
				string type = jo_member ["Type"].Value<string> ();
				string desc = jo_member ["Desc"].Value<string> ();

				sb_sc_member.AppendFormat (@"

	/* {2} */
	[UnityEngine.SerializeField, UnityEngine.Tooltip(""{2}"")]
	private {1} _{0};

	public {1} {0} {{ get {{ return _{0}; }} set {{ _{0} = value; }} }}", name, type, desc);
			}

			return sb_sc_member.ToString ();
		}

		public string GetCommentCode (JObject jo)
		{
			return  jo ["Common"] ["Desc"].Value<string> ();
		}
	}
}