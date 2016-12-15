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
			code = code.Replace ("__WWW__", workspaceName);

			if (!Directory.Exists (targetPath))
				Directory.CreateDirectory (targetPath);

			string file = Path.Combine (targetPath, string.Format ("{0}.cs", elementName));
			File.WriteAllText (file, code);
			filesGenerated.Add (file);
		}

		#endregion
	}
}