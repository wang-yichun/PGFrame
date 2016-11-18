using UnityEngine;
using System.Collections;
using System.IO;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class ElementViewTools
{
	public JObject ElementJson;

	public ElementViewTools (JObject elementJson)
	{
		this.ElementJson = elementJson;
	}

	public void CreateDefaultMember (RxType rt, string member_name)
	{
		JArray ja_views = ElementJson ["Views"] as JArray;

		for (int i = 0; i < ja_views.Count; i++) {
			JObject jo_view = ja_views [i] as JObject;
			JArray ja_view_members = jo_view ["Members"] as JArray;
			JObject jo_view_member = new JObject ();
			jo_view_member.Add ("Name", member_name);
			jo_view_member.Add ("Bind", CreateDefaultBindJObject (rt));
			ja_view_members.Add (jo_view_member);
		}
	}

	public JObject CreateDefaultBindJObject (RxType rt)
	{
		JObject jo = new JObject ();
		switch (rt) {
		case RxType.Property:
			jo.Add ("Changed", false);
			break;
		case RxType.Collection:
			jo.Add ("Add", false);
			jo.Add ("CountChanged", false);
			jo.Add ("EveryValueChanged", false);
			jo.Add ("Move", false);
			jo.Add ("Remove", false);
			jo.Add ("Replace", false);
			jo.Add ("Reset", false);
			break;
		case RxType.Dictionary:
			jo.Add ("Add", false);
			jo.Add ("CountChanged", false);
			jo.Add ("EveryValueChanged", false);
			jo.Add ("Remove", false);
			jo.Add ("Replace", false);
			jo.Add ("Reset", false);
			break;
		case RxType.Command:
			jo.Add ("Executed", false);
			break;
		default:
			break;
		}
		return jo;
	}
}
