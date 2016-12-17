using UnityEngine;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEditorInternal;
using System.Collections;
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

		public void SetIcons ()
		{
			pgf_window_title_icon = Resources.Load<Texture2D> ("pgf_fsm_window_title_icon");
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
					jo_element ["Name"] = GUI.TextField (r, jo_element ["Name"].Value<string> ());
				};

				TransitionsList.onAddCallback += (ReorderableList list) => {
					JObject transition_jo = new JObject ();
					transition_jo.Add ("Name", "");
					ja_elements.Add (transition_jo);
				};

				TransitionsList.onRemoveCallback += (ReorderableList list) => {
					JObject jo = ja_elements [list.index] as JObject;
					// PR_TODO: 删除所有使用了该 Transition 的地方
					ja_elements.RemoveAt (list.index);
				};
			}
		}

		Rect windowRect = new Rect (400 + 100, 100, 100, 100);
		Rect windowRect2 = new Rect (400, 100, 100, 100);

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
			}
			EditorGUILayout.Space ();
			GUILayout.EndVertical ();

			// title
			GUILayout.BeginVertical ();
			GUILayout.Label (jo_common ["Name"].Value<string> (), GUIStyleTemplate.FSMTitleStyle ());
			jo_common ["Desc"] = EditorGUILayout.TextArea (jo_common ["Desc"].Value<string> (), GUIStyleTemplate.GreenDescStyle ());
			GUILayout.EndVertical ();

			GUILayout.EndHorizontal ();

			Handles.BeginGUI ();
			Handles.DrawBezier (windowRect.center, windowRect2.center, new Vector2 (windowRect.xMax + 50f, windowRect.center.y), new Vector2 (windowRect2.xMin - 50f, windowRect2.center.y), Color.red, null, 5f);
			Handles.EndGUI ();

			BeginWindows ();
			windowRect = GUI.Window (0, windowRect, WindowFunction, "Box1");
			windowRect2 = GUI.Window (1, windowRect2, WindowFunction, "Box2");

			EndWindows ();

		}

		void WindowFunction (int windowID)
		{
			GUILayout.BeginVertical ();
			GUILayout.Label ("Hello World");
			GUILayout.EndVertical ();

			GUI.DragWindow ();

			windowRect = new Rect (Mathf.Floor (windowRect.x / 10f) * 10f, Mathf.Floor (windowRect.y / 10f) * 10f, windowRect.width, windowRect.height);
			windowRect2 = new Rect (Mathf.Floor (windowRect2.x / 10f) * 10f, Mathf.Floor (windowRect2.y / 10f) * 10f, windowRect2.width, windowRect2.height);
		}

		public void SaveJsonFile ()
		{
			jElement.Save ();
		}

		public void OnDestroy ()
		{
			this.jElement.Load ();
			this.jElement = null;
			this.TransitionsList = null;
		}
	}
}