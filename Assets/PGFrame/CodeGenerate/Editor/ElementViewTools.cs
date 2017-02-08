using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Data;

namespace PGFrame
{
	using Newtonsoft.Json.Linq;
	using Newtonsoft.Json;

	public class ElementViewTools
	{
		public JObject ElementJson;

		public ElementViewTools (JObject elementJson)
		{
			this.ElementJson = elementJson;
		}

		public void CreateDefaultView (string view_name)
		{
			JArray ja_views = ElementJson ["Views"] as JArray;
			JObject jo_view = new JObject ();
			jo_view.Add ("Name", view_name);
			jo_view.Add ("Type", "");
			jo_view.Add ("Desc", "");

			JObject jo_view_members = new JObject ();
			JArray ja_members = ElementJson ["Member"] as JArray;
			for (int i = 0; i < ja_members.Count; i++) {
				JObject jo_member = ja_members [i] as JObject;
				string member_name = jo_member ["Name"].Value<string> ();
				string member_rxtype = jo_member ["RxType"].Value<string> ();
				RxType member_RxType = (RxType)Enum.Parse (typeof(RxType), member_rxtype);
				
				JObject jo_view_member_value = new JObject ();
				jo_view_member_value.Add ("Bind", CreateDefaultBindJObject (member_RxType));
				jo_view_members.Add (member_name, jo_view_member_value);
			}

			jo_view.Add ("Members", jo_view_members);
			ja_views.Add (jo_view);
		}

		public void DeleteView (int view_idx)
		{
			JArray ja_views = ElementJson ["Views"] as JArray;
			ja_views.RemoveAt (view_idx);
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

		public void ChangeName (string oldName, string newName)
		{
			JArray ja_views = ElementJson ["Views"] as JArray;
			for (int i = 0; i < ja_views.Count; i++) {
				JObject jo_view = ja_views [i] as JObject;
				JObject jo_view_members = jo_view ["Members"] as JObject;
				JProperty jp = jo_view_members.Property (oldName);
				jp.AddAfterSelf (new JProperty (newName, jp.Value));
				jp.Remove ();
			}
		}

		public JObject CreateDefaultBindJObject (RxType rt)
		{
			JObject jo = new JObject ();
			switch (rt) {
			case RxType.Property:
				jo.Add ("Changed", false);
				jo.Add ("PairChanged", false);
				break;
			case RxType.Collection:
				jo.Add ("Add", false);
				jo.Add ("CountChanged", false);
				jo.Add ("Move", false);
				jo.Add ("Remove", false);
				jo.Add ("Replace", false);
				jo.Add ("Reset", false);
				break;
			case RxType.Dictionary:
				jo.Add ("Add", false);
				jo.Add ("CountChanged", false);
				jo.Add ("Remove", false);
				jo.Add ("Replace", false);
				jo.Add ("Reset", false);
				break;
			case RxType.Command:
				jo.Add ("Executed", false);
				break;
			case RxType.FSM:
				jo.Add ("Changed", false);
				jo.Add ("PairChanged", false);
				break;
			default:
				break;
			}
			return jo;
		}

		/// <summary>
		/// 在Generator ViewBase生成 代码时,一个新开发的绑定在旧的文档中没有描述,则自动加入false值并返回
		/// </summary>
		/// <returns><c>true</c>选定该绑定方式<c>false</c> otherwise.</returns>
		/// <param name="jo">view/members/member_name/bind 级别的 JObject</param>
		/// <param name="propertyName">绑定名称</param>
		public static bool GetBindPropertyValue (JObject jo, string propertyName)
		{
			bool result;
			JToken jt;
			if (jo.TryGetValue (propertyName, out jt)) {
				result = jt.Value<bool> ();
			} else {
				jo.Add (propertyName, false);
				result = false;
			}
			return result;
		}
	}
}