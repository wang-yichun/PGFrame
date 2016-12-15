using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace PGFrame
{
	using Newtonsoft.Json.Linq;

	public class PGCodeSubGenerator_SimpleClass: IPGCodeSubGenerator
	{
		public PGCodeSubGenerator_SimpleClass ()
		{
		}

		public PGCodeSubGenerator_SimpleClass (string templateFileName)
		{
			templateFileInfo = new FileInfo (templateFileName);
		}

		FileInfo templateFileInfo;

		#region IPGCodeSubGenerator implementation

		public bool CanGenerate (JObject jo)
		{
			string workspaceName = jo ["Workspace"].Value<string> ();
			string elementName = jo ["Common"] ["Name"].Value<string> ();
			string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/SimpleClass");
			string file = Path.Combine (targetPath, string.Format ("{0}.cs", elementName));
			return !File.Exists (file);
		}

		public void GenerateCode (JObject jo, IList<string> filesGenerated)
		{
			string workspaceName = jo ["Workspace"].Value<string> ();
			string elementName = jo ["Common"] ["Name"].Value<string> ();
			string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/SimpleClass");
			string code = File.ReadAllText (templateFileInfo.FullName);
			code = code.Replace ("__XXX__", elementName);

			// PR_TODO:
//		code = code.Replace (REACTIVE_MEMBERS, GetReactiveMembers (jo));
//		code = code.Replace (COMMAND_CLASS, GetCommandClass (jo));
//		code = code.Replace (INITIALIZE_CODE, GetInitializeCode (jo));
//		code = code.Replace (CLASS_COMMENT, GetClassDescription (jo));

			string file = Path.Combine (targetPath, string.Format ("{0}.cs", elementName));
			File.WriteAllText (file, code);
			filesGenerated.Add (file);
		}

		#endregion
	}
}