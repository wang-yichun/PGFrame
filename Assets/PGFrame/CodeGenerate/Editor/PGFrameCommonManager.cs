using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;

namespace PGFrame
{
	using PogoTools;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class PGFrameCommonManager
	{
		public class CommonModel
		{
			public class ElementFilesModel
			{
				public DocType DocType;
				public string Name;
				public string File;
			}

			public string Workspace;
			public string DocType;
			public List<ElementFilesModel> ElementFiles;
		}

		public PGFrameWindow FrameWindow;

		public PGFrameCommonManager (PGFrameWindow frameWindow)
		{
			this.FrameWindow = frameWindow;
		}

		public DirectoryInfo[] WorkspaceDirectoryInfos;
		public Dictionary<string, CommonModel> CommonObjectDic;

		public void Load ()
		{
			string JsonRootFull = Path.Combine (Application.dataPath, PGFrameWindow.JsonRoot);
			DirectoryInfo di = new DirectoryInfo (JsonRootFull);
			WorkspaceDirectoryInfos = di.GetDirectories ();

			CommonObjectDic = new Dictionary<string, CommonModel> ();

			for (int i = 0; i < WorkspaceDirectoryInfos.Length; i++) {
				DirectoryInfo wdi = WorkspaceDirectoryInfos [i];
				string ws_name = wdi.Name;
				string JsonWSFull = Path.Combine (JsonRootFull, ws_name);
				string JsonCommonFull = Path.Combine (JsonWSFull, "_Common.json");
				FileInfo common_fi = new FileInfo (JsonCommonFull);

				JSONElement e = new JSONElement ();
				e.FileInfo = common_fi;

				CommonModel cm = JsonConvert.DeserializeObject<CommonModel> (e.jo.ToString (Formatting.None));

				CommonObjectDic.Add (ws_name, cm);
			}
		}

		public DocType? GetTheDocTypeByName (string workspace, string name)
		{
			DocType? dt = null;
			if (CommonObjectDic != null) {
				if (CommonObjectDic.ContainsKey (workspace)) {
					if (name.EndsWith ("ViewModel")) {
						CommonModel.ElementFilesModel m = CommonObjectDic [workspace].ElementFiles.FirstOrDefault (_efm => _efm.Name + "ViewModel" == name);
						if (m != null) {
							dt = m.DocType;
						}
					} else {
						CommonModel.ElementFilesModel m = CommonObjectDic [workspace].ElementFiles.FirstOrDefault (_efm => _efm.Name == name);
						if (m != null && m.DocType != DocType.Element) {
							dt = m.DocType;
						}
					}
				}
			}
			return dt;
		}
	}
}
