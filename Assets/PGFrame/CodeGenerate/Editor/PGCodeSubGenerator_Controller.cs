using UnityEngine;
using System.Collections;
using System.IO;

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

	public bool CanGenerate (string workspaceName, string elementName)
	{
		string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/Controller");
		string file = Path.Combine (targetPath, string.Format ("{0}Controller.cs", elementName));
		return !File.Exists (file);
	}

	public void GenerateCode (string workspaceName, string elementName, System.Collections.Generic.IList<string> filesGenerated)
	{
		string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/Controller");

		string code = File.ReadAllText (templateFileInfo.FullName);
		code = code.Replace ("__XXX__", elementName);

		string file = Path.Combine (targetPath, string.Format ("{0}Controller.cs", elementName));
		File.WriteAllText (file, code);

		filesGenerated.Add (file);
	}
}
