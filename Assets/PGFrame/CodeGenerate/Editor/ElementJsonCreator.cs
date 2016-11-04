using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

public class ElementJsonCreator
{
	public JObject jo;

	public ElementJsonCreator ()
	{
		jo = new JObject ();
		jo.Add ("Workspace", null);
		jo.Add ("DocType", "Element");
		jo.Add ("Common", JToken.FromObject (new {}));
		jo.Add ("Member", new JArray ());

		creating_command = false;
		member_command_jo = null;
	}

	bool creating_command;
	JObject member_command_jo;

	public void RowCommand (DataRow row, XLSXJsonConverter.cidx cidx)
	{
		JArray ja;
		string rowTag = row [cidx.ClassType].ToString ();

		switch (rowTag) {
		case "#WS":
			jo ["Workspace"] = row [cidx.Name].ToString ();
			break;
		case "#Element":
			jo ["Common"] ["Name"] = row [cidx.Name].ToString ();
			jo ["Common"] ["Desc"] = row [cidx.Description].ToString ();
			break;
		case "#Property":
			ja = (JArray)jo ["Member"];
			ja.Add (JObject.FromObject (new {
				RxType = "Property",
				Name = row [cidx.Name].ToString (),
				Type = row [cidx.Type].ToString (),
				Desc = row [cidx.Description].ToString ()
			}));
			break;
		case "#Collection":
			ja = (JArray)jo ["Member"];
			ja.Add (JObject.FromObject (new {
				RxType = "Collection",
				Name = row [cidx.Name].ToString (),
				Type = row [cidx.Type].ToString (),
				Desc = row [cidx.Description].ToString ()
			}));
			break;
		case "#Dictionary":
			ja = (JArray)jo ["Member"];
			ja.Add (JObject.FromObject (new {
				RxType = "Dictionary",
				Name = row [cidx.Name].ToString (),
				Type = row [cidx.Type].ToString (),
				Desc = row [cidx.Description].ToString ()
			}));
			break;
		case "#Command":
			if (creating_command) {
				// 处理当前 command 的结束
				ja = (JArray)jo ["Member"];
				ja.Add (member_command_jo);
			}
			creating_command = true;
			// 第一个参数

			JArray command_params_ja = new JArray ();
			if (string.IsNullOrEmpty (row [cidx.ParamName].ToString ()) == false) {
				command_params_ja.Add (GetCommandParamJObject (row, cidx));
			}

			member_command_jo = JObject.FromObject (new {
				RxType = "Command",
				Name = row [cidx.Name].ToString (),
				Params = command_params_ja,
				Desc = row [cidx.Description].ToString ()
			});

			break;
		case "#End":
			break;
		default:
			if (creating_command) {
				if (string.IsNullOrEmpty (row [cidx.ParamName].ToString ()) == false) {
					JArray ps = (JArray)member_command_jo ["Params"];
					ps.Add (GetCommandParamJObject (row, cidx));
				}
			}
			break;
		}

		if (creating_command) {
			if (!string.IsNullOrEmpty (rowTag) && rowTag != "#Command") {
				creating_command = false;
				// 处理当前 command 的结束

				ja = (JArray)jo ["Member"];
				ja.Add (member_command_jo);
			}
		}
	}

	JObject GetCommandParamJObject (DataRow row, XLSXJsonConverter.cidx cidx)
	{
		return JObject.FromObject (new {
			Name = row [cidx.ParamName].ToString (),
			Type = row [cidx.ParamType].ToString (),
			Desc = row [cidx.ParamDescription].ToString ()
		});
	}
}
