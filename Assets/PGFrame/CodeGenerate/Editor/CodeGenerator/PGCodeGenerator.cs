using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PogoTools;
using System;

/// <summary>
/// 维护 ViewModel/Controller/View/ElementEditor 的生成器,并启动生成器的生成
/// </summary>
public class PGCodeGenerator
{
	public static readonly string lt = "PGFrame.PGCodeGenerator";
	public static readonly Color lc = new Color32 (0, 162, 255, 255);
	public static readonly Color lc_r = new Color32 (255, 162, 162, 255);

	IPGCodeSubGenerator sg_ViewModelBase;
	IPGCodeSubGenerator sg_ViewModel;
	IPGCodeSubGenerator sg_ControllerBase;
	IPGCodeSubGenerator sg_Controller;
	IPGCodeSubGenerator sg_ViewInterface;
	IPGCodeSubGenerator sg_ViewBase;
	IPGCodeSubGenerator sg_View;
	IPGCodeSubGenerator sg_ElementEditor;
	IPGCodeSubGenerator sg_ElementViewEditor;
	IPGCodeSubGenerator sg_SimpleClassBase;
	IPGCodeSubGenerator sg_SimpleClass;
	IPGCodeSubGenerator sg_Enum;

	public void Init ()
	{
		sg_ViewModelBase = new PGCodeSubGenerator_ViewModelBase (Path.Combine (Application.dataPath, "PGFrame/CodeGenerate/Template/__XXX__ViewModelBase.txt"));
		sg_ViewModel = new PGCodeSubGenerator_ViewModel (Path.Combine (Application.dataPath, "PGFrame/CodeGenerate/Template/__XXX__ViewModel.txt"));
		sg_ControllerBase = new PGCodeSubGenerator_ControllerBase (Path.Combine (Application.dataPath, "PGFrame/CodeGenerate/Template/__XXX__ControllerBase.txt"));
		sg_Controller = new PGCodeSubGenerator_Controller (Path.Combine (Application.dataPath, "PGFrame/CodeGenerate/Template/__XXX__Controller.txt"));
		sg_ViewInterface = new PGCodeSubGenerator_ViewInterface (Path.Combine (Application.dataPath, "PGFrame/CodeGenerate/Template/__XXX__ViewInterface.txt"));
		sg_ViewBase = new PGCodeSubGenerator_ViewBase (Path.Combine (Application.dataPath, "PGFrame/CodeGenerate/Template/__XXX__ViewBase.txt"));
		sg_View = new PGCodeSubGenerator_View (Path.Combine (Application.dataPath, "PGFrame/CodeGenerate/Template/__XXX__View.txt"));
		sg_ElementEditor = new PGCodeSubGenerator_ElementEditor (Path.Combine (Application.dataPath, "PGFrame/CodeGenerate/Template/__XXX__ElementEditor.txt"));
		sg_ElementViewEditor = new PGCodeSubGenerator_ElementViewEditor (Path.Combine (Application.dataPath, "PGFrame/CodeGenerate/Template/__XXX__ElementViewEditor.txt"));
		sg_SimpleClassBase = new PGCodeSubGenerator_SimpleClassBase (Path.Combine (Application.dataPath, "PGFrame/CodeGenerate/Template/__XXX__SimpleClassBase.txt"));
		sg_SimpleClass = new PGCodeSubGenerator_SimpleClass (Path.Combine (Application.dataPath, "PGFrame/CodeGenerate/Template/__XXX__SimpleClass.txt"));
		sg_Enum = new PGCodeSubGenerator_Enum (Path.Combine (Application.dataPath, "PGFrame/CodeGenerate/Template/__XXX__Enum.txt"));
	}

	public void GenerateCode (JObject jo)
	{
		IList<string> filesGenerated = new List<string> ();

		DocType dt = (DocType)Enum.Parse (typeof(DocType), jo ["DocType"].Value<string> ());

		switch (dt) {
		case DocType.Element:
			if (sg_ViewModelBase.CanGenerate (jo))
				sg_ViewModelBase.GenerateCode (jo, filesGenerated);
			if (sg_ViewModel.CanGenerate (jo))
				sg_ViewModel.GenerateCode (jo, filesGenerated);
			if (sg_ControllerBase.CanGenerate (jo))
				sg_ControllerBase.GenerateCode (jo, filesGenerated);
			if (sg_Controller.CanGenerate (jo))
				sg_Controller.GenerateCode (jo, filesGenerated);
			if (sg_ViewInterface.CanGenerate (jo))
				sg_ViewInterface.GenerateCode (jo, filesGenerated);
			if (sg_ViewBase.CanGenerate (jo))
				sg_ViewBase.GenerateCode (jo, filesGenerated);
			if (sg_View.CanGenerate (jo))
				sg_View.GenerateCode (jo, filesGenerated);
			if (sg_ElementEditor.CanGenerate (jo))
				sg_ElementEditor.GenerateCode (jo, filesGenerated);
			if (sg_ElementViewEditor.CanGenerate (jo))
				sg_ElementViewEditor.GenerateCode (jo, filesGenerated);
			break;
		case DocType.SimpleClass:
			if (sg_SimpleClassBase.CanGenerate (jo))
				sg_SimpleClassBase.GenerateCode (jo, filesGenerated);
			if (sg_SimpleClass.CanGenerate (jo))
				sg_SimpleClass.GenerateCode (jo, filesGenerated);
			break;
		case DocType.Enum:
			if (sg_Enum.CanGenerate (jo))
				sg_Enum.GenerateCode (jo, filesGenerated);
			break;
		default:
			throw new ArgumentOutOfRangeException ();
		}


		PRDebug.TagLog (lt + ".GenerateCode", lc, JsonConvert.SerializeObject (filesGenerated, Formatting.Indented));
	}

	public void DeleteCode (JObject jo)
	{
		string workspaceName = jo ["Workspace"].Value<string> ();
		string elementName = jo ["Common"] ["Name"].Value<string> ();

		string[] targetPaths = {
			Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/Controller/" + string.Format ("{0}Controller.cs", elementName)),
			Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/Controller/_Base/" + string.Format ("{0}ControllerBase.cs", elementName)),
			Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/ViewModel/" + string.Format ("{0}ViewModel.cs", elementName)),
			Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/ViewModel/_Base/" + string.Format ("{0}ViewModelBase.cs", elementName)),
			Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/View/" + string.Format ("{0}View.cs", elementName)),
			Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/View/_Base/" + string.Format ("{0}ViewBase.cs", elementName)),
			Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/Editor/" + string.Format ("{0}ElementEditor.cs", elementName)),
			Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/SimpleClass/" + string.Format ("{0}.cs", elementName)),
			Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/SimpleClass/_Base/" + string.Format ("{0}Base.cs", elementName))
		};
		List<string> fileDeleted = new List<string> ();

		foreach (string s in targetPaths) {
			if (File.Exists (s)) {
				File.Delete (s);
				fileDeleted.Add (s);
			}
		}

		PRDebug.TagLog (lt + ".DeleteCode", lc, JsonConvert.SerializeObject (fileDeleted, Formatting.Indented));
	}
}
