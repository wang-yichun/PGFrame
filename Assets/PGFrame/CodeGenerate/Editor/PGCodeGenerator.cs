using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using PogoTools;

public class PGCodeGenerator
{
	public static readonly string lt = "PGFrame.PGCodeGenerator";
	public static readonly Color lc = new Color32 (0, 162, 255, 255);

	FileInfo xViewModelBase;
	FileInfo xViewModel;
	FileInfo xControllerBase;
	FileInfo xController;
	FileInfo xViewBase;
	FileInfo xView;

	public void Init ()
	{
		xViewModelBase = new FileInfo (Path.Combine (Application.dataPath, "PGFrame/CodeGenerate/Template/__XXX__ViewModelBase.txt"));
		xViewModel = new FileInfo (Path.Combine (Application.dataPath, "PGFrame/CodeGenerate/Template/__XXX__ViewModel.txt"));
		xControllerBase = new FileInfo (Path.Combine (Application.dataPath, "PGFrame/CodeGenerate/Template/__XXX__ControllerBase.txt"));
		xController = new FileInfo (Path.Combine (Application.dataPath, "PGFrame/CodeGenerate/Template/__XXX__Controller.txt"));
		xViewBase = new FileInfo (Path.Combine (Application.dataPath, "PGFrame/CodeGenerate/Template/__XXX__ViewBase.txt"));
		xView = new FileInfo (Path.Combine (Application.dataPath, "PGFrame/CodeGenerate/Template/__XXX__View.txt"));
	}

	public void GenerateCode (string workspaceName, string elementName)
	{
		IList<string> filesGenerated = new List<string> ();

		GenerateViewModelBase (workspaceName, elementName, filesGenerated);
		GenerateViewModel (workspaceName, elementName, filesGenerated);
		GenerateControllerBase (workspaceName, elementName, filesGenerated);
		GenerateController (workspaceName, elementName, filesGenerated);
		GenerateViewBase (workspaceName, elementName, filesGenerated);
		GenerateView (workspaceName, elementName, filesGenerated);

		PRDebug.TagLog (lt, lc, JsonConvert.SerializeObject (filesGenerated, Formatting.Indented));

	}

	void GenerateViewModel (string workspaceName, string elementName, IList<string> filesGenerated)
	{
		string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/ViewModel");

		string code = File.ReadAllText (xViewModel.FullName);
		code = code.Replace ("__XXX__", elementName);

		string file = Path.Combine (targetPath, string.Format ("{0}ViewModel.cs", elementName));
		File.WriteAllText (file, code);

		filesGenerated.Add (file);
	}

	void GenerateViewModelBase (string workspaceName, string elementName, IList<string> filesGenerated)
	{
		string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/ViewModel/_Base");

		string code = File.ReadAllText (xViewModelBase.FullName);
		code = code.Replace ("__XXX__", elementName);

		string file = Path.Combine (targetPath, string.Format ("{0}ViewModelBase.cs", elementName));
		File.WriteAllText (file, code);

		filesGenerated.Add (file);
	}

	void GenerateControllerBase (string workspaceName, string elementName, IList<string> filesGenerated)
	{
		string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/Controller/_Base");

		string code = File.ReadAllText (xControllerBase.FullName);
		code = code.Replace ("__XXX__", elementName);

		string file = Path.Combine (targetPath, string.Format ("{0}ControllerBase.cs", elementName));
		File.WriteAllText (file, code);

		filesGenerated.Add (file);
	}

	void GenerateController (string workspaceName, string elementName, IList<string> filesGenerated)
	{
		string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/Controller");

		string code = File.ReadAllText (xController.FullName);
		code = code.Replace ("__XXX__", elementName);

		string file = Path.Combine (targetPath, string.Format ("{0}Controller.cs", elementName));
		File.WriteAllText (file, code);

		filesGenerated.Add (file);
	}

	void GenerateViewBase (string workspaceName, string elementName, IList<string> filesGenerated)
	{
		string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/View/_Base");

		string code = File.ReadAllText (xViewBase.FullName);
		code = code.Replace ("__XXX__", elementName);

		string file = Path.Combine (targetPath, string.Format ("{0}ViewBase.cs", elementName));
		File.WriteAllText (file, code);

		filesGenerated.Add (file);
	}

	void GenerateView (string workspaceName, string elementName, IList<string> filesGenerated)
	{
		string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/View");

		string code = File.ReadAllText (xView.FullName);
		code = code.Replace ("__XXX__", elementName);

		string file = Path.Combine (targetPath, string.Format ("{0}View.cs", elementName));
		File.WriteAllText (file, code);

		filesGenerated.Add (file);
	}



	public void DeleteCode (string workspaceName, string elementName)
	{
		string[] targetPaths = {
			Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/Controller/" + string.Format ("{0}Controller.cs", elementName)),
			Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/Controller/_Base/" + string.Format ("{0}ControllerBase.cs", elementName)),
			Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/ViewModel/" + string.Format ("{0}ViewModel.cs", elementName)),
			Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/ViewModel/_Base/" + string.Format ("{0}ViewModelBase.cs", elementName)),
			Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/View/" + string.Format ("{0}View.cs", elementName)),
			Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/View/_Base/" + string.Format ("{0}ViewBase.cs", elementName)),
		};

		foreach (string s in targetPaths) {
			File.Delete (s);
		}
	}
}
