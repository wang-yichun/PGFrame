using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniRx;

public class GameCoreViewBase : ViewBase , IGameCoreView
{
	public GameCoreViewModel VM;

	public GameCoreViewModel GameCore {
		get {
			return VM;
		}
	}

	public override ViewModelBase GetViewModel ()
	{
		return VM;
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

	public void ViewModelPropertyRef ()
	{
		
		if (_MyInfoView.GetViewModel () == null) {
			_MyInfoView.CreateViewModel ();
		}
		VM.MyInfo = _MyInfoView.GetViewModel () as PlayerInfoViewModel;
	}

	public override void Bind ()
	{
		base.Bind ();
		
		VM.CurrentBullets.ObserveAdd ().Subscribe (OnAdd_CurrentBullets);
		VM.CurrentBullets.ObserveRemove ().Subscribe (OnRemove_CurrentBullets);
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
	public IPlayerInfoView MyInfoView {
		get {
			return (IPlayerInfoView)_MyInfoView;
		}
		set {
			_MyInfoView = (ViewBase)value;
		}
	}
}
