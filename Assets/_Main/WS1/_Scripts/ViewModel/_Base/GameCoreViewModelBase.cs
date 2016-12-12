using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniRx;

/*//////////////////////////////////////////////////////////////////////////////
  
//////////////////////////////////////////////////////////////////////////////*/
[JsonObjectAttribute (MemberSerialization.OptIn)]
public class GameCoreViewModelBase : ViewModelBase
{
	public GameCoreViewModelBase ()
	{
	}

	public override void Initialize ()
	{
		base.Initialize ();
		
		RP_GameID = new ReactiveProperty<string> ();
		RP_MyInfo = new ReactiveProperty<PlayerInfoViewModel> ();
	}

	public override void Attach ()
	{
		base.Attach ();
		GameCoreController.Instance.Attach (this);
	}

	

	/*  */
	public ReactiveProperty<string> RP_GameID;

	[JsonProperty]
	public string GameID {
		get {
			return RP_GameID.Value;
		}
		set {
			RP_GameID.Value = value;
		}
	}

	/*  */
	public ReactiveProperty<PlayerInfoViewModel> RP_MyInfo;

	
	public PlayerInfoViewModel MyInfo {
		get {
			return RP_MyInfo.Value;
		}
		set {
			RP_MyInfo.Value = value;
		}
	}
}

