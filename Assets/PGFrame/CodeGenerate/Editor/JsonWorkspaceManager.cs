using UnityEngine;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PogoTools;

public class JsonWorkspaceManager
{
	public DirectoryInfo JsonWorkspaceRoot;

	public JsonWorkspaceManager (string workspaceRootPath)
	{
		this.JsonWorkspaceRoot = new DirectoryInfo (workspaceRootPath);
	}

	public string CreateWorkspace (string workspace)
	{
		string targetWorkspaceRootName = Path.Combine (JsonWorkspaceRoot.FullName, workspace);
		if (Directory.Exists (targetWorkspaceRootName)) {
			return "工作空间已存在";
		}
			
		Directory.CreateDirectory (targetWorkspaceRootName);


		string targetFileName = Path.Combine (targetWorkspaceRootName, "_Common.json");

		JObject jo = new JObject ();
		jo.Add ("Workspace", workspace);
		jo.Add ("DocType", "Workspace");
		jo.Add ("ElementFiles", new JArray ());

		string content = JsonConvert.SerializeObject (jo, Formatting.Indented);

		File.WriteAllText (targetFileName, content);

		return null;
	}
}
