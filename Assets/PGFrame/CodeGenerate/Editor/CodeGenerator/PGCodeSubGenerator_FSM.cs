using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PGFrame
{
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class PGCodeSubGenerator_FSM: IPGCodeSubGenerator
	{
		public PGCodeSubGenerator_FSM ()
		{
		}

		public PGCodeSubGenerator_FSM (string templateFileName)
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
			string entryStateName = jo ["Common"] ["EntryState"].Value<string> ();
			string targetPath = Path.Combine (Application.dataPath, "_Main/" + workspaceName + "/_Scripts/FSM");
			string code = File.ReadAllText (templateFileInfo.FullName);
			code = code.Replace ("__XXX__", elementName);
			code = code.Replace ("__WWW__", workspaceName);
			code = code.Replace ("__ZZZ__", "State." + entryStateName);

			code = code.Replace (FSM_STATES, GetStatesCode (jo));
			code = code.Replace (FSM_INITIALIZE, GetInitializeCode (jo));
			code = code.Replace (FSM_ATTACH, GetAttachCode (jo));
			code = code.Replace (FSM_DECLARE, GetDeclareCode (jo));

			if (!Directory.Exists (targetPath))
				Directory.CreateDirectory (targetPath);

			string file = Path.Combine (targetPath, string.Format ("{0}.cs", elementName));
			File.WriteAllText (file, code);
			filesGenerated.Add (file);
		}

		#endregion

		public static readonly string FSM_STATES = @"/****STATES****/";
		public static readonly string FSM_INITIALIZE = @"/****INITIALIZE_REACTIVECOMMAND****/";
		public static readonly string FSM_ATTACH = @"/****ATTACH_REACTIVECOMMAND****/";
		public static readonly string FSM_DECLARE = @"/****DECLARE_REACTIVECOMMAND****/";

		public string GetStatesCode (JObject jo)
		{
			StringBuilder sb = new StringBuilder ();
			JArray ja_states = jo ["State"] as JArray;
			for (int i = 0; i < ja_states.Count; i++) {
				JObject jo_state = ja_states [i] as JObject;
				string state_name = jo_state ["Name"].Value<string> ();
				if (i != ja_states.Count - 1) {
					sb.AppendFormat ("\t\t\t{0},\n", state_name);
				} else {
					sb.AppendFormat ("\t\t\t{0}", state_name);
				}
			}
			return sb.ToString ();
		}

		public string GetInitializeCode (JObject jo)
		{
			StringBuilder sb = new StringBuilder ();
			JArray ja_transitions = jo ["Transition"] as JArray;
			for (int i = 0; i < ja_transitions.Count; i++) {
				string transition_name = ja_transitions [i] ["Name"].Value<string> ();

				List<string> state_names = new List<string> ();

				JArray ja_states = jo ["State"] as JArray;
				for (int j = 0; j < ja_states.Count; j++) {
					JObject jo_state = ja_states [j] as JObject;
					string state_name = jo_state ["Name"].Value<string> ();
					JArray ja_state_transitions = jo_state ["Transitions"] as JArray;

					for (int k = 0; k < ja_state_transitions.Count; k++) {
						JObject jo_state_transition = ja_state_transitions [k] as JObject;
						if (jo_state_transition ["Name"].Value<string> () == transition_name) {
							state_names.Add (string.Format ("_ == State.{0}", state_name));
						}
					}
				}

				string state_names_joined = string.Join (" || ", state_names.ToArray ());
				string template = string.Format (@"
			{0}Transition = CurrentState.Select (_ => {1}).ToReactiveCommand ();", transition_name, state_names_joined);

				sb.Append (template);
			}
			return sb.ToString ();
		}

		public string GetAttachCode (JObject jo)
		{
			StringBuilder sb = new StringBuilder ();
			JArray ja_transitions = jo ["Transition"] as JArray;
			for (int i = 0; i < ja_transitions.Count; i++) {
				string transition_name = ja_transitions [i] ["Name"].Value<string> ();

				StringBuilder sb_inner = new StringBuilder ();

				JArray ja_states = jo ["State"] as JArray;
				for (int j = 0; j < ja_states.Count; j++) {
					JObject jo_state = ja_states [j] as JObject;
					string state_name = jo_state ["Name"].Value<string> ();
					JArray ja_state_transitions = jo_state ["Transitions"] as JArray;
					for (int k = 0; k < ja_state_transitions.Count; k++) {
						JObject jo_state_transition = ja_state_transitions [k] as JObject;
						string target_state_name = jo_state_transition ["TargetState"].Value<string> ();

						if (jo_state_transition ["Name"].Value<string> () == transition_name) {
							sb_inner.AppendFormat (@"
				{2}if (CurrentState.Value == State.{0})
					CurrentState.Value = State.{1};", state_name, target_state_name, sb_inner.Length == 0 ? string.Empty : "else ");
						}
					}
				}

				sb.AppendFormat (@"
			
			{0}Transition.Subscribe (_ => {{{1}
			}}).AddTo (this.baseAttachDisposables);", transition_name, sb_inner.ToString ());

			}
			return sb.ToString ();
		}

		public string GetDeclareCode (JObject jo)
		{
			StringBuilder sb = new StringBuilder ();
			JArray ja_transitions = jo ["Transition"] as JArray;
			for (int i = 0; i < ja_transitions.Count; i++) {
				string transition_name = ja_transitions [i] ["Name"].Value<string> ();
				string template = string.Format (@"
		public ReactiveCommand {0}Transition;", transition_name);

				sb.Append (template);
			}
			return sb.ToString ();
		}

	}
}