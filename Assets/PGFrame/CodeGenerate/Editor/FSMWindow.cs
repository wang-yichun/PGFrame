using UnityEngine;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEditorInternal;
using System.Collections;
using System.Linq;
using PogoTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PGFrame
{

	public class FSMWindow : EditorWindow
	{
		public static readonly string lt = "PGFrame-FSM";
		public static readonly Color lc = new Color32 (0, 162, 255, 255);
		public static readonly Color lcr = new Color32 (255, 162, 162, 255);

		public static FSMWindow Current;

		public Texture2D pgf_window_title_icon;
		public Texture2D pgf_transation_point;

		public void SetIcons ()
		{
			pgf_window_title_icon = Resources.Load<Texture2D> ("pgf_fsm_window_title_icon");
			pgf_transation_point = Resources.Load<Texture2D> ("pgf_fsm_transation_point");
		}

		[MenuItem ("PogoRock/PGFrame/Finite State Machine... %_F2")]
		public static void Init ()
		{
			FSMWindow window = (FSMWindow)EditorWindow.GetWindow (typeof(FSMWindow));
			window.SetIcons ();
			window.titleContent = new GUIContent (window.pgf_window_title_icon);
			window.Show ();
			Current = window;
		}

		public JSONElement jElement;

		public ReorderableList TransitionsList;

		void ResetReorderableList ()
		{
			if (jElement != null) {
				JArray ja_elements = jElement.jo ["Transition"] as JArray;
				TransitionsList = new ReorderableList (ja_elements, typeof(JToken));
				TransitionsList.drawHeaderCallback += (Rect rect) => {
					GUI.Label (rect, "Transitions");
				};
				float[] split = new float[]{ 0f, 1f };
				TransitionsList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) => {

					Rect r = new Rect (rect);
					r.y -= 1;
					r.height -= 2;
					int split_idx = 0;
					r.x = (rect.width - 25f) * split [split_idx] + 25f;
					r.width = (rect.width - 25f) * (split [split_idx + 1] - split [split_idx]);
					JObject jo_element = ja_elements [index] as JObject;
					string ori_element_name = jo_element ["Name"].Value<string> ();
					string new_element_name = EditorGUI.DelayedTextField (r, ori_element_name);
					if (new_element_name != ori_element_name) {
						RenameTransition (ori_element_name, new_element_name);
					}
				};

				TransitionsList.onAddCallback += (ReorderableList list) => {
					JObject transition_jo = new JObject ();
					transition_jo.Add ("Name", "");
					ja_elements.Add (transition_jo);
				};

				TransitionsList.onRemoveCallback += (ReorderableList list) => {
					JObject jo = ja_elements [list.index] as JObject;
					DeleteTransition (jo ["Name"].Value<string> ());
				};
			}
		}

		Vector2 scrollPosition;

		static readonly float tab_width = 200f;

		private void OnGUI ()
		{
			if (TransitionsList == null) {
				if (jElement == null) {
					this.Close ();
					return;
				}
				ResetReorderableList ();
			}

			JObject jo_common = jElement.jo ["Common"] as JObject;


			GUILayout.BeginHorizontal ();

			// tab
			GUILayout.BeginVertical (GUILayout.MaxWidth (tab_width));

			TransitionsList.DoLayoutList ();

			GUILayout.FlexibleSpace ();

			if (in_state_rename_jo_state != null) {
				GUILayout.BeginVertical ("box");
				GUILayout.Label (string.Format ("Rename State({0}) To:", in_state_rename_jo_state ["Name"].Value<string> ()));
				in_state_rename_target_name = GUILayout.TextField (in_state_rename_target_name);
				if (GUILayout.Button ("Confirm Rename")) {
					RenameState ();
					in_state_rename_jo_state = null;
					in_state_rename_target_name = "";
				}
				if (GUILayout.Button ("Cancel Rename State")) {
					in_state_rename_jo_state = null;
					in_state_rename_target_name = "";
				}
				GUILayout.EndVertical ();
			}

			if (in_state_transition_target_selecting_jo_state_transition != null) {
				if (GUILayout.Button ("Cancel Transition To")) {
					in_state_transition_target_selecting_jo_state_transition = null;
					in_state_transition_target_selecting_window_id = -1;
				}
			}
			if (GUILayout.Button ("Create New State")) {
				CreateNewState ();
			}
			if (GUILayout.Button ("JElement")) {
				PRDebug.TagLog (lt, lc, JsonConvert.SerializeObject (jElement, Formatting.Indented));
			}
			if (GUILayout.Button ("Reset")) {
				TransitionsList = null;
				jElement.Load ();
			}
			if (GUILayout.Button ("Save")) {
				SaveJsonFile ();
			}
			if (GUILayout.Button ("Save & Close")) {
				SaveJsonFile ();
				this.Close ();
				return;
			}
			EditorGUILayout.Space ();
			GUILayout.EndVertical ();

			// title
			GUILayout.BeginVertical ();
			GUILayout.Label (jo_common ["Name"].Value<string> (), GUIStyleTemplate.FSMTitleStyle ());
			jo_common ["Desc"] = EditorGUILayout.TextArea (jo_common ["Desc"].Value<string> (), GUIStyleTemplate.GreenDescStyle ());
			GUILayout.EndVertical ();

			GUILayout.EndHorizontal ();

			DrawTransitionLines ();

			BeginWindows ();
			if (jElement != null) {
				JArray ja_states = jElement.jo ["State"] as JArray;

				for (int i = 0; i < ja_states.Count; i++) {

					JObject jo_state = ja_states [i] as JObject;
					string state_name = jo_state ["Name"].Value<string> ();
					float x = jo_state ["Rect"] ["x"].Value<float> ();
					float y = jo_state ["Rect"] ["y"].Value<float> ();
					float w = jo_state ["Rect"] ["w"].Value<float> ();
					float h = jo_state ["Rect"] ["h"].Value<float> ();

					Rect rect = GUI.Window (i, new Rect (x, y, w, h), WindowFunction, state_name);

					rect = SnapRect (rect);
					jo_state ["Rect"] ["x"] = (int)rect.x;
					jo_state ["Rect"] ["y"] = (int)rect.y;
					jo_state ["Rect"] ["w"] = (int)rect.width;
					jo_state ["Rect"] ["h"] = (int)rect.height;

				}
			}
			EndWindows ();
		}

		void WindowFunction (int windowID)
		{
			string entry_state = jElement.jo ["Common"] ["EntryState"].Value<string> ();
			JArray ja_states = jElement.jo ["State"] as JArray;
			JObject jo_state = ja_states [windowID] as JObject;
			string state_name = jo_state ["Name"].Value<string> ();

			GUILayout.BeginVertical ();

			JArray ja_transitions = jo_state ["Transitions"] as JArray;

			float maxWidth = 50f;
			float maxHeight = 30f;

			JObject jo_rect = jo_state ["Rect"] as JObject;

			bool right_click_transition_box = false;

			// title size
			GUIStyle style_cal = GUI.skin.label;
			GUIContent title_content = new GUIContent (state_name);
			Vector2 title_size = style_cal.CalcSize (title_content);
			maxWidth = Mathf.Max (maxWidth, title_size.x + 22f);

			if (ja_transitions.Count > 0) {

				for (int i = 0; i < ja_transitions.Count; i++) {
					JObject jo_transition = ja_transitions [i] as JObject;
					GUILayout.BeginHorizontal ();
					string transition_name = jo_transition ["Name"].Value<string> ();

					GUIStyle style = new GUIStyle (GUI.skin.label);
					style.alignment = TextAnchor.MiddleCenter;

					if (in_state_transition_target_selecting_window_id == windowID
					    && in_state_transition_target_selecting_jo_state_transition ["Name"].Value<string> () == transition_name) {
						style.normal.textColor = Color.green;
						style.fontStyle = FontStyle.Bold;
					}

					GUIContent content = new GUIContent (transition_name);
					Vector2 size = style.CalcSize (content);
					maxWidth = Mathf.Max (maxWidth, size.x + 22f);
					maxHeight += size.y + 10f;

					GUILayout.BeginHorizontal ("box");
					GUILayout.Label (content, style, GUILayout.Width (jo_rect ["w"].Value<float> () - 19f));

					if (Event.current.type == EventType.mouseDown && GUILayoutUtility.GetLastRect ().Contains (Event.current.mousePosition)) {
						if (Event.current.button == 1) {
							right_click_transition_box = true;
							ShowStateTransitionContextMenu (windowID, i);
						}
					}
					GUILayout.EndHorizontal ();

					GUILayout.EndHorizontal ();
				}
			} else {

				GUIStyle style = new GUIStyle (GUI.skin.box);
				style.alignment = TextAnchor.MiddleCenter;
				GUIContent content = new GUIContent ("<Empty>");
				Vector2 size = style.CalcSize (content);
				maxWidth = Mathf.Max (maxWidth, size.x + 11f);
				maxHeight += size.y + 4f;

				GUILayout.Label (content, style, GUILayout.Width (jo_rect ["w"].Value<float> () - 11f));
			}


			if (state_name == entry_state) {
				GUIStyle style = new GUIStyle (GUI.skin.box);
				style.alignment = TextAnchor.MiddleCenter;
				GUIContent content = new GUIContent ("<Entry>");
				Vector2 size = style.CalcSize (content);
				maxWidth = Mathf.Max (maxWidth, size.x + 11f);
				maxHeight += size.y + 4f;

				GUILayout.Label (content, style, GUILayout.Width (jo_rect ["w"].Value<float> () - 11f));
			}

			jo_rect ["w"] = maxWidth;
			jo_rect ["h"] = maxHeight;

			GUILayout.EndVertical ();

			if (Event.current.type == EventType.mouseDown && right_click_transition_box == false) {
				focused_state_name = state_name;

				if (in_state_transition_target_selecting_jo_state_transition != null) {
					in_state_transition_target_selecting_jo_state_transition ["TargetState"] = state_name;
					in_state_transition_target_selecting_jo_state_transition = null;
					in_state_transition_target_selecting_window_id = -1;
				}

				if (Event.current.button == 1) {
					ShowStateContextMenu_Add (windowID);
				}
			}

			if (Event.current.button == 0)
				GUI.DragWindow ();
		}

		//		bool in_state_transition_target_selecting_mode = false;
		JObject in_state_transition_target_selecting_jo_state_transition = null;
		int in_state_transition_target_selecting_window_id = -1;

		// 显示状态节点右键菜单
		public void ShowStateTransitionContextMenu (int windowID, int transition_idx)
		{
			JArray ja_states = jElement.jo ["State"] as JArray;
			JObject jo_state = ja_states [windowID] as JObject;
			JArray jo_state_transitions = jo_state ["Transitions"] as JArray;

			GenericMenu menu = new GenericMenu ();
			menu.AddItem (new GUIContent ("Transition To..."), false, () => {
				JObject jo_state_transition = jo_state_transitions [transition_idx] as JObject;
				in_state_transition_target_selecting_jo_state_transition = jo_state_transition;
				in_state_transition_target_selecting_window_id = windowID;
			});
			menu.AddItem (new GUIContent ("Transition To Null"), false, () => {
				JObject jo_state_transition = jo_state_transitions [transition_idx] as JObject;
				jo_state_transition ["TargetState"] = null;
			});
			menu.AddItem (new GUIContent ("Up"), false, () => {
				if (transition_idx > 0) {
					JObject jo_temp = jo_state_transitions [transition_idx - 1] as JObject;
					jo_state_transitions [transition_idx - 1] = jo_state_transitions [transition_idx];
					jo_state_transitions [transition_idx] = jo_temp;
				}
			});
			menu.AddItem (new GUIContent ("Down"), false, () => {
				if (transition_idx < jo_state_transitions.Count - 1) {
					JObject jo_temp = jo_state_transitions [transition_idx + 1] as JObject;
					jo_state_transitions [transition_idx + 1] = jo_state_transitions [transition_idx];
					jo_state_transitions [transition_idx] = jo_temp;
				}
			});
			menu.AddItem (new GUIContent ("Delete"), false, () => {
				jo_state_transitions.RemoveAt (transition_idx);
			});

			menu.ShowAsContext ();
		}

		JObject in_state_rename_jo_state = null;
		string in_state_rename_target_name = "";

		public void ShowStateContextMenu_Add (int windowID)
		{
			JArray ja_states = jElement.jo ["State"] as JArray;
			JObject jo_state = ja_states [windowID] as JObject;
			string state_name = jo_state ["Name"].Value<string> ();

			GenericMenu menu = new GenericMenu ();

			JArray ja_transitions = jElement.jo ["Transition"] as JArray;
			for (int i = 0; i < ja_transitions.Count; i++) {
				JObject jo_transition = ja_transitions [i] as JObject;
				string transition_name = jo_transition ["Name"].Value<string> ();
					
				menu.AddItem (new GUIContent ("Add: " + transition_name), false, () => {
					JArray ja_state_transitions = jo_state ["Transitions"] as JArray;
					if (ja_state_transitions.FirstOrDefault (_ => _ ["Name"].Value<string> () == transition_name) == null) {
						JObject jo_new_transitions = new JObject ();
						jo_new_transitions.Add ("Name", transition_name);
						jo_new_transitions.Add ("TargetState", null);
						ja_state_transitions.Add (jo_new_transitions);
					}
				});
			}

			menu.AddSeparator ("");
			menu.AddItem (new GUIContent ("Rename State"), false, () => {
				in_state_rename_jo_state = jo_state;
			});

			menu.AddSeparator ("");
			if (jElement.jo ["Common"] ["EntryState"].Value<string> () != state_name) {
				menu.AddItem (new GUIContent ("Set Entry"), false, () => {
					jElement.jo ["Common"] ["EntryState"] = state_name;
				});
			}

			menu.AddSeparator ("");
			menu.AddItem (new GUIContent ("Delete State"), false, () => {
				ja_states.RemoveAt (windowID);
			});

			menu.ShowAsContext ();
		}

		public void SaveJsonFile ()
		{
			jElement.Save ();
		}

		public void OnDestroy ()
		{
			if (jElement != null) {
				this.jElement = null;
			}
			this.TransitionsList = null;
		}

		public Rect SnapRect (Rect rect)
		{
			rect.x = Mathf.Floor (rect.x / 10f) * 10f;
			rect.y = Mathf.Floor (rect.y / 10f) * 10f;
			return rect;
//			return new Rect (Mathf.Floor (rect.x / 10f) * 10f, Mathf.Floor (rect.y / 10f) * 10f, rect.width, rect.height);
		}

		public string focused_state_name;

		public void DrawTransitionLines ()
		{
			JArray ja_states = jElement.jo ["State"] as JArray;
			for (int i = 0; i < ja_states.Count; i++) {
				JObject jo_state = ja_states [i] as JObject;
				string state_name = jo_state ["Name"].Value<string> ();
				bool focused = false;
				if (state_name == focused_state_name) {
					focused = true;
				}
				JObject jo_rect = jo_state ["Rect"] as JObject;

				Rect rect = new Rect (jo_rect ["x"].Value<float> (), jo_rect ["y"].Value<float> (), jo_rect ["w"].Value<float> (), jo_rect ["h"].Value<float> ());

				JArray ja_transitions = jo_state ["Transitions"] as JArray;
				for (int j = 0; j < ja_transitions.Count; j++) {
					JObject jo_transition = ja_transitions [j] as JObject;
//					string transition_name = jo_transition ["Name"].Value<string> ();
					string target_stage_name = jo_transition ["TargetState"].Value<string> ();

					Vector2 startPosition;
					Vector2 startTangent;
					Vector2 endPostion;
					Vector2 endTangent;
					Vector2 targetCenter;

					bool hasTargetState = GetStatePT (target_stage_name, out endPostion, out endTangent, out targetCenter, rect.center);

					if (rect.center.x < targetCenter.x) {
						startPosition = new Vector2 (rect.x + rect.width, rect.y + j * 26f + 1.9f * EditorGUIUtility.singleLineHeight);
						startTangent = startPosition + Vector2.right * 50f;
					} else {
						startPosition = new Vector2 (rect.x, rect.y + j * 26f + 1.9f * EditorGUIUtility.singleLineHeight);
						startTangent = startPosition + Vector2.left * 50f;
					}

					if (hasTargetState) {
						Handles.BeginGUI ();

						Color color = Color.gray;
						if (target_stage_name == focused_state_name) {
							color = Color.yellow;
						} else if (focused) {
							color = Color.green;
						}
//						Color color = focused ? Color.green : Color.gray;

						Handles.DrawBezier (startPosition, endPostion, startTangent, endTangent, color, null, 4f);
						Vector2 arr0 = (endTangent - endPostion).normalized * 10f + endPostion + new Vector2 (0f, 5f);
						Vector2 arr1 = (endTangent - endPostion).normalized * 10f + endPostion + new Vector2 (0f, -5f);
						Handles.DrawBezier (endPostion, arr0, endPostion, arr0, color, null, 4f);
						Handles.DrawBezier (endPostion, arr1, endPostion, arr1, color, null, 4f);
						Handles.EndGUI ();
					}
				}
			}
		}

		public bool GetStatePT (string stateName, out Vector2 endPosition, out Vector2 endTangent, out Vector2 targetCenter, Vector2 oriPosition)
		{
			endPosition = default(Vector2);
			endTangent = default(Vector2);
			targetCenter = default(Vector2);

			JArray ja_states = jElement.jo ["State"] as JArray;
			for (int i = 0; i < ja_states.Count; i++) {
				JObject jo_state = ja_states [i] as JObject;
				string state_name = jo_state ["Name"].Value<string> ();
				if (state_name == stateName) {
					JObject jo_rect = jo_state ["Rect"] as JObject;
					Rect rect = new Rect (jo_rect ["x"].Value<float> (), jo_rect ["y"].Value<float> (), jo_rect ["w"].Value<float> (), jo_rect ["h"].Value<float> ());

					if (rect.center.x > oriPosition.x) {
						endPosition = new Vector2 (rect.x, rect.y + .5f * EditorGUIUtility.singleLineHeight);
						endTangent = Vector2.left * 50f + endPosition;
					} else {
						endPosition = new Vector2 (rect.x + rect.width, rect.y + .5f * EditorGUIUtility.singleLineHeight);
						endTangent = Vector2.right * 50f + endPosition;
					}

					targetCenter = rect.center;
					return true;
				}
			}
			return false;
		}

		public void RenameState ()
		{
			JArray ja_states = jElement.jo ["State"] as JArray;
			if (ja_states.FirstOrDefault (_ => _ ["Name"].Value<string> () == in_state_rename_target_name) == null) {

				string ori_state_name = in_state_rename_jo_state ["Name"].Value<string> ();
				in_state_rename_jo_state ["Name"] = in_state_rename_target_name;

				for (int i = 0; i < ja_states.Count; i++) {
					JObject jo_state = ja_states [i] as JObject;
					JArray ja_state_transitions = jo_state ["Transitions"] as JArray;
					for (int j = 0; j < ja_state_transitions.Count; j++) {
						JObject jo_state_transition = ja_state_transitions [j] as JObject;
						if (jo_state_transition ["TargetState"].Value<string> () == ori_state_name) {
							jo_state_transition ["TargetState"] = in_state_rename_target_name;
						}
					}
				}
			} else {
				PRDebug.TagLog (lt, lcr, "输入的状态名字已经被占用");
			}
		}

		public void RenameTransition (string ori_name, string new_name)
		{
			JArray ja_transitions = jElement.jo ["Transition"] as JArray;
			JArray ja_states = jElement.jo ["State"] as JArray;

			if (ja_transitions.FirstOrDefault (_ => _ ["Name"].Value<string> () == new_name) == null) {

				for (int i = 0; i < ja_transitions.Count; i++) {
					JObject jo_transition = ja_transitions [i] as JObject;
					if (jo_transition ["Name"].Value<string> () == ori_name) {
						jo_transition ["Name"] = new_name;
					}
				}

				for (int i = 0; i < ja_states.Count; i++) {
					JObject jo_state = ja_states [i] as JObject;
					JArray ja_state_transitions = jo_state ["Transitions"] as JArray;
					for (int j = 0; j < ja_state_transitions.Count; j++) {
						JObject jo_state_transition = ja_state_transitions [j] as JObject;
						if (jo_state_transition ["Name"].Value<string> () == ori_name) {
							jo_state_transition ["Name"] = new_name;
						}
					}
				}
			}
		}

		public void CreateNewState ()
		{
			JArray ja_states = jElement.jo ["State"] as JArray;
			string target_name = "NewState";
			int digital_suffix = 0;
			while (ja_states.FirstOrDefault (_ => _ ["Name"].Value<string> () == target_name) != null) {
				digital_suffix++;
				target_name = string.Format ("{0}{1}", target_name, digital_suffix);
			}

			JObject jo_state = new JObject ();
			jo_state.Add ("Name", target_name);
			jo_state.Add ("Transitions", new JArray ());
			jo_state.Add ("Rect", new JObject (){ { "x", 150 }, { "y" , 100 }, { "w" , 100 }, { "h" , 50 } });

			ja_states.Add (jo_state);
		}

		public void DeleteTransition (string name)
		{
			JArray ja_transitions = jElement.jo ["Transition"] as JArray;
			JArray ja_states = jElement.jo ["State"] as JArray;

			for (int i = 0; i < ja_transitions.Count; i++) {
				JObject jo_transition = ja_transitions [i] as JObject;
				if (jo_transition ["Name"].Value<string> () == name) {
					ja_transitions.RemoveAt (i);
					break;
				}
			}

			for (int i = 0; i < ja_states.Count; i++) {
				JObject jo_state = ja_states [i] as JObject;
				JArray ja_state_transitions = jo_state ["Transitions"] as JArray;
				for (int j = 0; j < ja_state_transitions.Count; j++) {
					JObject jo_state_transition = ja_state_transitions [j] as JObject;
					if (jo_state_transition ["Name"].Value<string> () == name) {
						ja_state_transitions.RemoveAt (j);
						break;
					}
				}
			}
		}
	}
}