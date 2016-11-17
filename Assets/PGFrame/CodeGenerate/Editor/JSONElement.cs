using UnityEngine;
using System.Collections;
using System.IO;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class JSONElement
{
	private FileInfo fileInfo;

	public FileInfo FileInfo {
		get {
			return fileInfo;
		}
		set {
			fileInfo = value;
			Load ();
		}
	}

	public JObject jo;

	public string Workspace;
	public string DocType;
	public string Name;

	public void Load ()
	{
		string json = File.ReadAllText (fileInfo.FullName);
		jo = JObject.Parse (json);

		Workspace = jo ["Workspace"].Value<string> ();
		DocType = jo ["DocType"].Value<string> ();

		if (DocType == "Element") {
			Name = jo ["Common"] ["Name"].Value<string> ();
		}
//		Debug.Log (fileInfo.FullName + "\n" + jo ["Workspace"].Value<string> ());
	}

	public void Save ()
	{
		string json = JsonConvert.SerializeObject (jo, Formatting.Indented);
		File.WriteAllText (fileInfo.FullName, json);
	}
}
