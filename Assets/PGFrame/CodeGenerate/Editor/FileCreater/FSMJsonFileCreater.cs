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

	public class FSMJsonFileCreater : JsonFileCreater
	{
		public FSMJsonFileCreater (PGFrameWindow frameWindow, string name)
			: base (frameWindow, name)
		{
		}

		public override void Create ()
		{
			JArray ja_elements = FrameWindow.SelectedWorkspaceCommon.jo ["ElementFiles"] as JArray;

			if (FrameWindow.jElements.FirstOrDefault (_ => _.Name == Name) == null) {

				string jsonName = MakeJsonName ();

				JObject jo = new JObject ();
				jo.Add ("DocType", "FSM");
				jo.Add ("Name", Name);
				jo.Add ("File", jsonName);
				ja_elements.Add (jo);

				FrameWindow.SaveCommonFile ();

				CreateJson (MakeJsonName (), FrameWindow.SelectedWorkspaceCommon.Workspace, Name);

				FrameWindow.NeedRefresh = true;
			} else {
				PRDebug.TagLog (PGFrameWindow.lt, PGFrameWindow.lc, "该工作空间中已经含有名字: " + Name);
			}
		}

		public override string MakeJsonName ()
		{
			return string.Format (
				"{0}.{1}.{2}.json", 
				FrameWindow.SelectedWorkspaceCommon.Workspace, 
				"FSM",
				Name
			);
		}

		void CreateJson (string jsonFullName, string workspace, string elementName)
		{
			string targetFileFullPath = Path.Combine (Application.dataPath, PGFrameWindow.JsonRoot);
			targetFileFullPath = Path.Combine (targetFileFullPath, string.Format ("{0}/{1}", workspace, jsonFullName));

			JObject jo = new JObject ();
			jo.Add ("Workspace", workspace);
			jo.Add ("DocType", "FSM");

			JObject jo_common = new JObject ();
			jo_common.Add ("Name", elementName);
			jo_common.Add ("Desc", string.Empty);
			jo_common.Add ("EntryState", "DefaultState");
			jo.Add ("Common", jo_common);
			jo.Add ("Transition", new JArray ());

			JArray ja_states = new JArray ();
			ja_states.Add (new JObject () {
				{ "Name", "DefaultState" },
				{ "Transitions", new JArray () },
				{ "Rect", new JObject (){ { "x", 150 }, { "y" , 100 }, { "w" , 100 }, { "h" , 50 } } }
			});
			jo.Add ("State", ja_states);

			string json = JsonConvert.SerializeObject (jo, Formatting.Indented);
			File.WriteAllText (targetFileFullPath, json);

			AssetDatabase.Refresh ();

			PRDebug.TagLog (PGFrameWindow.lt, PGFrameWindow.lc, targetFileFullPath + " (Created)");
		}
	}
}