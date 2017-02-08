using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace WS1
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class GameCoreElementEditor : ElementEditorBase<GameCoreViewModel>
	{
		public override GameCoreViewModel VM { get; set; }

		public override void OnInspectorGUI ()
		{
			EditorGUILayout.BeginVertical ();
			
			base.OnInspectorGUI ();

			if (ToggleView = EditorGUILayout.Foldout (ToggleView, "View")) {
				base.DrawDefaultInspector ();
				EditorGUILayout.Space ();
			}

			if (ToggleViewModel = EditorGUILayout.Foldout (ToggleViewModel, "ViewModel - GameCore")) {

				if (VM != null) {
					InspectorGUI_ViewModel ();
				} else {
					EditorGUILayout.HelpBox ("没有绑定 ViewModel", MessageType.Warning);
				}

				EditorGUILayout.Space ();
			}

			EditorGUILayout.EndVertical ();
		}

		public void InspectorGUI_ViewModel ()
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginVertical ();

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Name & ID");
			EditorGUILayout.TextField (string.Format ("{0} ({1})", "GameCoreViewModel", VM.VMID.ToString ().Substring (0, 8)));
			EditorGUILayout.EndHorizontal ();

			

		string vmk;

		vmk = "GameID";
		string tempGameID = EditorGUILayout.DelayedTextField (vmk, VM.GameID);
		if (tempGameID != VM.GameID) {
			VM.GameID = tempGameID;
		}

			vmk = "MyInfo";
			ViewBase MyInfoView = (target as IGameCoreView).MyInfoView as ViewBase;
			if (EditorApplication.isPlaying && VM.MyInfo == null)
				MyInfoView = null;
			ViewBase tempMyInfoView = (ViewBase)EditorGUILayout.ObjectField (vmk, MyInfoView, typeof(ViewBase), true);
			if (tempMyInfoView == null) {
				(target as IGameCoreView).MyInfoView = null;
				VM.MyInfo = null;
			} else if (MyInfoView != tempMyInfoView) {
				var view = tempMyInfoView as WS1.IPlayerInfoView;
				if (view != null) {
					(target as IGameCoreView).MyInfoView = tempMyInfoView as WS1.IPlayerInfoView;
					VM.MyInfo = (WS1.PlayerInfoViewModel)tempMyInfoView.GetViewModel ();
				} else {
					Debug.Log ("类型不匹配, 需要一个: PlayerInfo");
				}
			}

		vmk = "CurrentBullets";
		EditorGUILayout.BeginHorizontal ();
		string CurrentBulletsJson = JsonConvert.SerializeObject (VM.CurrentBullets);
		string tempCurrentBulletsJson = EditorGUILayout.DelayedTextField (vmk, CurrentBulletsJson);
		if (tempCurrentBulletsJson != CurrentBulletsJson) {
			if (string.IsNullOrEmpty (tempCurrentBulletsJson)) {
				VM.CurrentBullets = null;
			} else {
				VM.CurrentBullets = JsonConvert.DeserializeObject<ReactiveCollection<BulletViewModel>> (tempCurrentBulletsJson);
			}
		}
		if (GUILayout.Button ("...", GUILayout.MaxWidth (20))) {
			PopupWindow.Show (
				new Rect (Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f), 
				new ReactiveCollectionEditorPopupWindow<BulletViewModel> (this, VM.CurrentBullets)
			);
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "AddSomeBullet";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_AddSomeBullet.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

		vmk = "RemoveSomeBullet";
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (vmk);
		if (GUILayout.Button ("Invoke")) {
			VM.RC_RemoveSomeBullet.Execute ();
		}
		EditorGUILayout.EndHorizontal ();

			vmk = "MyWS2Ball";
			ViewBase MyWS2BallView = (target as IGameCoreView).MyWS2BallView as ViewBase;
			if (EditorApplication.isPlaying && VM.MyWS2Ball == null)
				MyWS2BallView = null;
			ViewBase tempMyWS2BallView = (ViewBase)EditorGUILayout.ObjectField (vmk, MyWS2BallView, typeof(ViewBase), true);
			if (tempMyWS2BallView == null) {
				(target as IGameCoreView).MyWS2BallView = null;
				VM.MyWS2Ball = null;
			} else if (MyWS2BallView != tempMyWS2BallView) {
				var view = tempMyWS2BallView as WS2.IBallView;
				if (view != null) {
					(target as IGameCoreView).MyWS2BallView = tempMyWS2BallView as WS2.IBallView;
					VM.MyWS2Ball = (WS2.BallViewModel)tempMyWS2BallView.GetViewModel ();
				} else {
					Debug.Log ("类型不匹配, 需要一个: Ball");
				}
			}

			vmk = "GameState";
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel (vmk);
			VM.GameState = (GameCoreFSM.State)EditorGUILayout.EnumPopup (VM.GameState);
			if (GUILayout.Button ("Transition...")) {
				GenericMenu menu = new GenericMenu ();

				Type FSMType = typeof(WS1.GameCoreFSM);
				FieldInfo[] fis = FSMType.GetFields ();
				fis.ToObservable ().Where (_ => {
					return _.FieldType == typeof(UniRx.ReactiveCommand) &&
					_.Name.ToString ().EndsWith ("Transition");
				}).Select ((_, idx) => {
					int lio = _.Name.ToString ().LastIndexOf ("Transition");
					return new {_ = _, idx = idx, name = _.Name.ToString ().Substring (0, lio)};
				}).Subscribe (_ => {
					menu.AddItem (new GUIContent (_.idx + ". " + _.name), false, () => {
						Type t = VM.FSM_GameState.GetType ();
						FieldInfo fi = t.GetField (_._.Name);
						ReactiveCommand rc = fi.GetValue (VM.FSM_GameState) as ReactiveCommand;
						rc.Execute ();
					});
				});
				menu.ShowAsContext ();
			}
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.EndVertical ();
			EditorGUI.indentLevel--;

			if (EditorApplication.isPlaying == false) {
				if (GUI.changed) {
					VMCopyToJson ();
				}
			}
		}

		#region IElementEditor implementation

		public override void VMCopyToJson ()
		{
		}

		#endregion
	}

}