using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace PGFrame
{
	using Newtonsoft.Json.Linq;

	public class PGCodeSubGenerator_ElementViewEditor: IPGCodeSubGenerator
	{
		public PGCodeSubGenerator_ElementViewEditor ()
		{
		}

		public PGCodeSubGenerator_ElementViewEditor (string templateFileName)
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
			JArray ja_views = jo ["Views"] as JArray;
			for (int i = 0; i < ja_views.Count; i++) {
				JObject jo_view = ja_views [i] as JObject;
				string viewName = jo_view ["Name"].Value<string> ();
				string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/Editor");
				string code = File.ReadAllText (templateFileInfo.FullName);
				code = code.Replace ("__YYY__", viewName);
				code = code.Replace ("__XXX__", elementName);
				code = code.Replace ("__WWW__", workspaceName);
				string file = Path.Combine (targetPath, string.Format ("{0}Editor.cs", viewName));
				File.WriteAllText (file, code);
				filesGenerated.Add (file);
			}
		}

		#endregion
	}
}
