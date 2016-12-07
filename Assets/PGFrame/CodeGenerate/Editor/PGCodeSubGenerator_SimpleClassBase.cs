using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

		// PR_TODO:
//		code = code.Replace (REACTIVE_MEMBERS, GetReactiveMembers (jo));
//		code = code.Replace (COMMAND_CLASS, GetCommandClass (jo));
//		code = code.Replace (INITIALIZE_CODE, GetInitializeCode (jo));
//		code = code.Replace (CLASS_COMMENT, GetClassDescription (jo));

		string file = Path.Combine (targetPath, string.Format ("{0}Base.cs", elementName));
		File.WriteAllText (file, code);
		filesGenerated.Add (file);
	}

	#endregion
}