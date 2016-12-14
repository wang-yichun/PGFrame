using UnityEngine;
using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PogoTools;

public class GameCoreController : GameCoreControllerBase<GameCoreController>
{
	public GameCoreController ()
	{
	}

	public override void AddSomeBullet (GameCoreViewModel viewModel)
	{
		base.AddSomeBullet (viewModel);

		BulletViewModel bulletVM = new BulletViewModel ();
		viewModel.CurrentBullets.Add (bulletVM);
	}
	
}
