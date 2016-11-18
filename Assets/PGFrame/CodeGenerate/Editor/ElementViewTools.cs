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
			JObject jo_view_members = jo_view ["Members"] as JObject;
			JObject jo_member = new JObject ();
			jo_member.Add ("Bind", CreateDefaultBindJObject (rt));
			jo_view_members.Add (member_name, jo_member);
		}
	}

	public void DeleteMember (string member_name)
	{
		JArray ja_views = ElementJson ["Views"] as JArray;
		for (int i = 0; i < ja_views.Count; i++) {
			JObject jo_view = ja_views [i] as JObject;
			JObject jo_view_members = jo_view ["Members"] as JObject;
			jo_view_members.Remove (member_name);
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
