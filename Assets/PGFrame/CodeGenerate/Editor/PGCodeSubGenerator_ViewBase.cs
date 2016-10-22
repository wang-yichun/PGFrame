using UnityEngine;
using System.Collections;
using System.IO;

public class PGCodeSubGenerator_ViewBase: IPGCodeSubGenerator
{
	public PGCodeSubGenerator_ViewBase ()
	{
	}

	public PGCodeSubGenerator_ViewBase (string templateFileName)
	{
		templateFileInfo = new FileInfo (templateFileName);
	}

	FileInfo templateFileInfo;

	public bool CanGenerate (string workspaceName, string elementName)
	{
		string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/View/_Base");
		string file = Path.Combine (targetPath, string.Format ("{0}ViewBase.cs", elementName));
		return !File.Exists (file);
	}

	public void GenerateCode (string workspaceName, string elementName, System.Collections.Generic.IList<string> filesGenerated)
	{
		string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/View/_Base");

		string code = File.ReadAllText (templateFileInfo.FullName);
		code = code.Replace ("__XXX__", elementName);

		string file = Path.Combine (targetPath, string.Format ("{0}ViewBase.cs", elementName));
		File.WriteAllText (file, code);

		filesGenerated.Add (file);
	}
}
