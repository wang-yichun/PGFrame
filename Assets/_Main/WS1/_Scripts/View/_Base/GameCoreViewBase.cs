using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WS1
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class GameCoreViewBase : ViewBase , IGameCoreView
	{
		public  GameCoreViewModel VM;

		public GameCoreViewModel GameCore {
			get {
				return VM;
			}
		}

		public override void CreateViewModel ()
		{
			if (UseEmptyViewModel || string.IsNullOrEmpty (ViewModelInitValueJson)) {
				VM = new GameCoreViewModel ();
			} else {
				VM = JsonConvert.DeserializeObject<GameCoreViewModel> (ViewModelInitValueJson);
				ViewModelPropertyRef ();
			}
			
			VM.AddHostView (ViewModelBase.DefaultViewBaseKey, this);
		}

		public override ViewModelBase GetViewModel ()
		{
			return VM;
		}
		
		public override void SetViewModel (ViewModelBase viewModel)
		{
			VM = (GameCoreViewModel)viewModel;
		}

		public override void Initialize (ViewModelBase viewModel)
		{
			if (viewModel != null) {
				VM = (GameCoreViewModel)viewModel;
				VM.AddHostView (ViewModelBase.DefaultViewBaseKey, this);
			} else {
				if (AutoCreateViewModel && VM == null) {
					CreateViewModel ();
				}
			}

			base.Initialize (VM);
		}

		public override void ViewModelPropertyRef ()
		{
			

			if (_MyInfoView != null) {
				if (_MyInfoView.GetViewModel () == null) {
					_MyInfoView.CreateViewModel ();
				}
				ViewModelBase _MyInfoViewModel = _MyInfoView.GetViewModel ();
				if (_MyInfoViewModel != null) {
					VM.MyInfo = _MyInfoView.GetViewModel () as PlayerInfoViewModel;
				}
			}

			if (_MyWS2BallView != null) {
				if (_MyWS2BallView.GetViewModel () == null) {
					_MyWS2BallView.CreateViewModel ();
				}
				ViewModelBase _MyWS2BallViewModel = _MyWS2BallView.GetViewModel ();
				if (_MyWS2BallViewModel != null) {
					VM.MyWS2Ball = _MyWS2BallView.GetViewModel () as WS2.BallViewModel;
				}
			}
		}

		public override void BeforeBind ()
		{
			base.BeforeBind ();
		}
		
		public override void Bind ()
		{
			base.Bind ();
			
			VM.CurrentBullets.ObserveAdd ().Subscribe (OnAdd_CurrentBullets).AddTo(baseBindDisposables);
			VM.CurrentBullets.ObserveRemove ().Subscribe (OnRemove_CurrentBullets).AddTo(baseBindDisposables);
		}

		public override void AfterBind ()
		{
			base.AfterBind ();
		}

		

		public virtual void OnAdd_CurrentBullets (CollectionAddEvent<BulletViewModel> e)
		{
		}

		public virtual void OnRemove_CurrentBullets (CollectionRemoveEvent<BulletViewModel> e)
		{
		}

		
	
		[SerializeField, HideInInspector]
		public ViewBase _MyInfoView;

		public WS1.IPlayerInfoView MyInfoView {
			get {
				return (WS1.IPlayerInfoView)_MyInfoView;
			}
			set {
				_MyInfoView = (ViewBase)value;
			}
		}
	
		[SerializeField, HideInInspector]
		public ViewBase _MyWS2BallView;

		public WS2.IBallView MyWS2BallView {
			get {
				return (WS2.IBallView)_MyWS2BallView;
			}
			set {
				_MyWS2BallView = (ViewBase)value;
			}
		}
	}

}