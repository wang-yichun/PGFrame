using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;
using System;

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
		string file = Path.Combine (targetPath, string.Format ("I{0}View.cs", elementName));
		File.WriteAllText (file, code);
		filesGenerated.Add (file);
	}

	#endregion

}