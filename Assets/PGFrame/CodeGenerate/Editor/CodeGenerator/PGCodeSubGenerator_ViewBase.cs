using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PGFrame
{
	using Newtonsoft.Json.Linq;

	public class PGCodeSubGenerator_ViewBase: IPGCodeSubGenerator
	{
		public PGCodeSubGenerator_ViewBase ()
		{
		}

		public PGCodeSubGenerator_ViewBase (string templateFileName)
		{
			templateFileInfo = new FileInfo (templateFileName);
		}

		FileInfo templateFileInfo;

		#region IPGCodeSubGenerator implementation

		public bool CanGenerate (JObject jo)
		{
			return true;
		}

		public void GenerateCode (JObject jo, IList<string> filesGenerated)
		{
			string workspaceName = jo ["Workspace"].Value<string> ();
			string elementName = jo ["Common"] ["Name"].Value<string> ();
			JArray ja_views = jo ["Views"] as JArray;
			for (int i = 0; i < ja_views.Count; i++) {
				JObject jo_view = ja_views [i] as JObject;
				string viewName = jo_view ["Name"].Value<string> ();
				string baseViewName = jo_view ["Type"].Value<string> ();
				GetBindCodeAndFunc (jo, i);
				string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/View/_Base");
				string code = File.ReadAllText (templateFileInfo.FullName);
				code = code.Replace ("__YYY__", viewName);
				code = code.Replace ("__XXX__", elementName);
				code = code.Replace ("__ZZZ__", string.IsNullOrEmpty (baseViewName) ? "ViewBase" : baseViewName);
				code = code.Replace (BIND_CODE, bind_code);
				code = code.Replace (BIND_FUNC, bind_func);
				code = code.Replace (VM_PROPERTY_VIEW, GetVMPropertyViewCode (jo));
				code = code.Replace (VM_PROPERTY_REF, GetVMPropertyRefCode (jo));
				string file = Path.Combine (targetPath, string.Format ("{0}Base.cs", viewName));
				File.WriteAllText (file, code);
				filesGenerated.Add (file);
			}
		}

		#endregion

		public static readonly string BIND_CODE = @"/****bind_code****/";
		public static readonly string BIND_FUNC = @"/****bind_func****/";
		public static readonly string VM_PROPERTY_VIEW = @"/****vm_property_view****/";
		public static readonly string VM_PROPERTY_REF = @"/****vm_property_ref****/";

		public string GetVMPropertyRefCode (JObject jo)
		{
			StringBuilder sb = new StringBuilder ();

			string ws_name = jo ["Workspace"].Value<string> ();
			JArray ja_members = jo ["Member"] as JArray;
			for (int i = 0; i < ja_members.Count; i++) {
				JObject jo_member = ja_members [i] as JObject;
				RxType rt = (RxType)Enum.Parse (typeof(RxType), jo_member ["RxType"].Value<string> ());
				if (rt == RxType.Property) {
					string member_name = jo_member ["Name"].Value<string> ();
					string member_type = jo_member ["Type"].Value<string> ();
					DocType? dt = PGFrameTools.GetDocTypeByWorkspaceAndType (ws_name, member_type);
					if (dt.HasValue && dt.Value == DocType.Element) {

						string element_name = member_type.ConvertToElementName ();
						sb.AppendFormat (@"
		if (_{0}View.GetViewModel () == null) {{
			_{0}View.CreateViewModel ();
		}}
		VM.{0} = _{0}View.GetViewModel () as {1}ViewModel;", member_name, element_name);
					}
				}
			}
			return sb.ToString ();
		}

		public string GetVMPropertyViewCode (JObject jo)
		{
			StringBuilder sb = new StringBuilder ();

			string ws_name = jo ["Workspace"].Value<string> ();
			JArray ja_members = jo ["Member"] as JArray;
			for (int i = 0; i < ja_members.Count; i++) {
				JObject jo_member = ja_members [i] as JObject;
				RxType rt = (RxType)Enum.Parse (typeof(RxType), jo_member ["RxType"].Value<string> ());
				if (rt == RxType.Property) {
					string member_name = jo_member ["Name"].Value<string> ();
					string member_type = jo_member ["Type"].Value<string> ();
					DocType? dt = PGFrameTools.GetDocTypeByWorkspaceAndType (ws_name, member_type);
					if (dt.HasValue && dt.Value == DocType.Element) {

						string element_name = member_type.ConvertToElementName ();
						sb.AppendFormat (@"
	
	[SerializeField, HideInInspector]
	public ViewBase _{0}View;
	public I{1}View {0}View {{
		get {{
			return (I{1}View)_{0}View;
		}}
		set {{
			_{0}View = (ViewBase)value;
		}}
	}}", member_name, element_name);
					}
				}
			}

			return sb.ToString ();
		}

		string bind_code;
		string bind_func;

		public void GetBindCodeAndFunc (JObject jo, int view_idx)
		{
			StringBuilder sb_bind_code = new StringBuilder ();
			StringBuilder sb_bind_func = new StringBuilder ();

			string elementName = jo ["Common"] ["Name"].Value<string> ();
			JArray ja_members = jo ["Member"] as JArray;
			JObject jo_view = jo ["Views"] [view_idx] as JObject;

			for (int i = 0; i < ja_members.Count; i++) {
				JObject jo_member = ja_members [i] as JObject;
				string[] codes = GenCode_ViewBase.GenerateCode (jo_member, jo_view);
				sb_bind_code.Append (codes [0]);
				sb_bind_func.Append (codes [1]);
			}
			bind_code = sb_bind_code.ToString ();
			bind_func = sb_bind_func.ToString ();
		}
	}

	public static class GenCode_ViewBase
	{
		public static string[] GenerateCode (JObject jo_member, JObject jo_view)
		{
			string[] result = new string[2];
			RxType rx_type = (RxType)Enum.Parse (typeof(RxType), jo_member ["RxType"].Value<string> ());
			switch (rx_type) {
			case RxType.Property:
				result [0] = GenBindCodeReactiveMemberProperty (jo_member, jo_view);
				result [1] = GenFuncCodeReactiveMemberProperty (jo_member, jo_view);
				break;
			case RxType.Collection:
				result [0] = GenBindCodeReactiveMemberCollection (jo_member, jo_view);
				result [1] = GenFuncCodeReactiveMemberCollection (jo_member, jo_view);
				break;
			case RxType.Dictionary:
				result [0] = GenBindCodeReactiveMemberDictionary (jo_member, jo_view);
				result [1] = GenFuncCodeReactiveMemberDictionary (jo_member, jo_view);
				break;
			case RxType.Command:
				result [0] = GenBindCodeReactiveMemberCommand (jo_member, jo_view);
				result [1] = GenFuncCodeReactiveMemberCommand (jo_member, jo_view);
				break;
			default:
				break;
			}

			return result;
		}

		public static string GenBindCodeReactiveMemberProperty (JObject jo_member, JObject jo_view)
		{
			string member_name = jo_member ["Name"].Value<string> ();
			string result = "";
			if (jo_view ["Members"] [member_name] ["Bind"] ["Changed"].Value<bool> ()) {
				result = string.Format (@"
		VM.RP_{0}.Subscribe (OnChanged_{0});", member_name);
			}
			return result;
		}

		public static string GenFuncCodeReactiveMemberProperty (JObject jo_member, JObject jo_view)
		{
			string member_name = jo_member ["Name"].Value<string> ();
			string member_type = jo_member ["Type"].Value<string> ();
			string result = "";
			if (jo_view ["Members"] [member_name] ["Bind"] ["Changed"].Value<bool> ()) {
				result = string.Format (@"

	public virtual void OnChanged_{0} ({1} value)
	{{
	}}", member_name, member_type);
			}
			return result;
		}

		public static string GenBindCodeReactiveMemberCollection (JObject jo_member, JObject jo_view)
		{
			string member_name = jo_member ["Name"].Value<string> ();
			JObject jo_bind = jo_view ["Members"] [member_name] ["Bind"] as JObject;
			StringBuilder sb = new StringBuilder ();

			if (jo_bind ["Add"].Value<bool> ()) {
				sb.AppendFormat (@"
		VM.{0}.ObserveAdd ().Subscribe (OnAdd_{0});", member_name);
			}
			if (jo_bind ["CountChanged"].Value<bool> ()) {
				sb.AppendFormat (@"
		VM.{0}.ObserveCountChanged ().Subscribe (OnCountChanged_{0});", member_name);
			}
			if (jo_bind ["Move"].Value<bool> ()) {
				sb.AppendFormat (@"
		VM.{0}.ObserveMove ().Subscribe (OnMove_{0});", member_name);
			}
			if (jo_bind ["Remove"].Value<bool> ()) {
				sb.AppendFormat (@"
		VM.{0}.ObserveRemove ().Subscribe (OnRemove_{0});", member_name);
			}
			if (jo_bind ["Replace"].Value<bool> ()) {
				sb.AppendFormat (@"
		VM.{0}.ObserveReplace ().Subscribe (OnReplace_{0});", member_name);
			}
			if (jo_bind ["Reset"].Value<bool> ()) {
				sb.AppendFormat (@"
		VM.{0}.ObserveReset ().Subscribe (OnReset_{0});", member_name);
			}

			return sb.ToString ();
		}

		public static string GenFuncCodeReactiveMemberCollection (JObject jo_member, JObject jo_view)
		{
			string member_name = jo_member ["Name"].Value<string> ();
			string member_type = jo_member ["Type"].Value<string> ();
			JObject jo_bind = jo_view ["Members"] [member_name] ["Bind"] as JObject;
			StringBuilder sb = new StringBuilder ();

			if (jo_bind ["Add"].Value<bool> ()) {
				sb.AppendFormat (@"

	public virtual void OnAdd_{0} (CollectionAddEvent<{1}> e)
	{{
	}}", member_name, member_type);
			}
			if (jo_bind ["CountChanged"].Value<bool> ()) {
				sb.AppendFormat (@"

	public virtual void OnCountChanged_{0} (int count)
	{{
	}}", member_name);
			}

			if (jo_bind ["Move"].Value<bool> ()) {
				sb.AppendFormat (@"

	public virtual void OnMove_{0} (CollectionMoveEvent<{1}> e)
	{{
	}}", member_name, member_type);
			}
			if (jo_bind ["Remove"].Value<bool> ()) {
				sb.AppendFormat (@"

	public virtual void OnRemove_{0} (CollectionRemoveEvent<{1}> e)
	{{
	}}", member_name, member_type);
			}
			if (jo_bind ["Replace"].Value<bool> ()) {
				sb.AppendFormat (@"

	public virtual void OnReplace_{0} (CollectionReplaceEvent<{1}> e)
	{{
	}}", member_name, member_type);
			}
			if (jo_bind ["Reset"].Value<bool> ()) {
				sb.AppendFormat (@"

	public virtual void OnReset_{0} (Unit u)
	{{
	}}", member_name);
			}

			return sb.ToString ();
		}

		public static string GenBindCodeReactiveMemberDictionary (JObject jo_member, JObject jo_view)
		{
			string member_name = jo_member ["Name"].Value<string> ();
			JObject jo_bind = jo_view ["Members"] [member_name] ["Bind"] as JObject;
			StringBuilder sb = new StringBuilder ();

			if (jo_bind ["Add"].Value<bool> ()) {
				sb.AppendFormat (@"
		VM.{0}.ObserveAdd ().Subscribe (OnAdd_{0});", member_name);
			}
			if (jo_bind ["CountChanged"].Value<bool> ()) {
				sb.AppendFormat (@"
		VM.{0}.ObserveCountChanged ().Subscribe (OnCountChanged_{0});", member_name);
			}
			if (jo_bind ["Remove"].Value<bool> ()) {
				sb.AppendFormat (@"
		VM.{0}.ObserveRemove ().Subscribe (OnRemove_{0});", member_name);
			}
			if (jo_bind ["Replace"].Value<bool> ()) {
				sb.AppendFormat (@"
		VM.{0}.ObserveReplace ().Subscribe (OnReplace_{0});", member_name);
			}
			if (jo_bind ["Reset"].Value<bool> ()) {
				sb.AppendFormat (@"
		VM.{0}.ObserveReset ().Subscribe (OnReset_{0});", member_name);
			}

			return sb.ToString ();
		}

		public static string GenFuncCodeReactiveMemberDictionary (JObject jo_member, JObject jo_view)
		{
			string member_name = jo_member ["Name"].Value<string> ();
			string member_type = jo_member ["Type"].Value<string> ();
			string[] member_types = member_type.Split (new char[]{ ',' });
			JObject jo_bind = jo_view ["Members"] [member_name] ["Bind"] as JObject;
			StringBuilder sb = new StringBuilder ();

			if (jo_bind ["Add"].Value<bool> ()) {
				sb.AppendFormat (@"

	public virtual void OnAdd_{0} (DictionaryAddEvent<{1}, {2}> e)
	{{
	}}", member_name, member_types [0], member_types [1]);
			}
			if (jo_bind ["CountChanged"].Value<bool> ()) {
				sb.AppendFormat (@"

	public virtual void OnCountChanged_{0} (int count)
	{{
	}}", member_name);
			}
			
			if (jo_bind ["Remove"].Value<bool> ()) {
				sb.AppendFormat (@"

	public virtual void OnRemove_{0} (DictionaryRemoveEvent<{1}, {2}> e)
	{{
	}}", member_name, member_types [0], member_types [1]);
			}
			if (jo_bind ["Replace"].Value<bool> ()) {
				sb.AppendFormat (@"

	public virtual void OnReplace_{0} (DictionaryReplaceEvent<{1}, {2}> e)
	{{
	}}", member_name, member_types [0], member_types [1]);
			}
			if (jo_bind ["Reset"].Value<bool> ()) {
				sb.AppendFormat (@"

	public virtual void OnReset_{0} (Unit u)
	{{
	}}", member_name);
			}

			return sb.ToString ();
		}

		public static string GenBindCodeReactiveMemberCommand (JObject jo_member, JObject jo_view)
		{
			string member_name = jo_member ["Name"].Value<string> ();
			JObject jo_bind = jo_view ["Members"] [member_name] ["Bind"] as JObject;
			StringBuilder sb = new StringBuilder ();

			if (jo_member ["Params"] == null || (jo_member ["Params"] as JArray).Count == 0) {
				if (jo_bind ["Executed"].Value<bool> ()) {
					sb.AppendFormat (@"
		VM.RC_{0}.Subscribe (OnExecuted_{0});", member_name);
				}
			} else {
				if (jo_bind ["Executed"].Value<bool> ()) {
					sb.AppendFormat (@"
		VM.RC_{0}.Subscribe<{0}Command> (OnExecuted_{0});", member_name);
				}
			}

			return sb.ToString ();
		}

		public static string GenFuncCodeReactiveMemberCommand (JObject jo_member, JObject jo_view)
		{
			string member_name = jo_member ["Name"].Value<string> ();
			JObject jo_bind = jo_view ["Members"] [member_name] ["Bind"] as JObject;
			StringBuilder sb = new StringBuilder ();

			if (jo_member ["Params"] == null || (jo_member ["Params"] as JArray).Count == 0) {
				if (jo_bind ["Executed"].Value<bool> ()) {
					sb.AppendFormat (@"

	public virtual void OnExecuted_{0} (Unit unit)
	{{
	}}", member_name);
				}
			} else {
				if (jo_bind ["Executed"].Value<bool> ()) {
					sb.AppendFormat (@"

	public virtual void OnExecuted_{0} ({0}Command command)
	{{
	}}", member_name);
				}
			}

			return sb.ToString ();
		}
	}
}