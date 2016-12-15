using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniRx;
using PogoTools;

public class GameCoreView : GameCoreViewBase
{
	public override void Initialize (ViewModelBase viewModel)
	{
		base.Initialize (viewModel);
	}

	public override void Bind ()
	{
		base.Bind ();
		Debug.Log (string.Format ("GameCoreView in {0} Bind.", gameObject.name));
	}

	public override void AfterBind ()
	{
		base.AfterBind ();
		Debug.Log (string.Format ("GameCoreView in {0} AfterBind.", gameObject.name));
	}

	public GameObject BulletPrefab;
	public Transform BulletContainer;

	public override void OnAdd_CurrentBullets (CollectionAddEvent<BulletViewModel> e)
	{
		GameObject go = Instantiate<GameObject> (BulletPrefab);
		BulletView bulletView = go.GetComponent<BulletView> ();
		bulletView.Initialize (e.Value);

		e.Value.GetView<BulletView> ("Default").transform.SetParent (BulletContainer);
	}

	public override void OnRemove_CurrentBullets (CollectionRemoveEvent<BulletViewModel> e)
	{
		Destroy (e.Value.GetView<BulletView> ("Default").gameObject);
	}
}
