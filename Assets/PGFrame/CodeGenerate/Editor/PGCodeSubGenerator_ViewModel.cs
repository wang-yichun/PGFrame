using UnityEngine;
using System.Collections;
using System.IO;

public class PGCodeSubGenerator_ViewModel: IPGCodeSubGenerator
{
	public PGCodeSubGenerator_ViewModel ()
	{
	}

	public PGCodeSubGenerator_ViewModel (string templateFileName)
	{
		templateFileInfo = new FileInfo (templateFileName);
	}

	FileInfo templateFileInfo;

	public bool CanGenerate (string workspaceName, string elementName)
	{
		string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/ViewModel");
		string file = Path.Combine (targetPath, string.Format ("{0}ViewModel.cs", elementName));
		return !File.Exists (file);
	}

	public void GenerateCode (string workspaceName, string elementName, System.Collections.Generic.IList<string> filesGenerated)
	{
		string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/ViewModel");

		string code = File.ReadAllText (templateFileInfo.FullName);
		code = code.Replace ("__XXX__", elementName);

		string file = Path.Combine (targetPath, string.Format ("{0}ViewModel.cs", elementName));
		File.WriteAllText (file, code);

		filesGenerated.Add (file);
	}
}
