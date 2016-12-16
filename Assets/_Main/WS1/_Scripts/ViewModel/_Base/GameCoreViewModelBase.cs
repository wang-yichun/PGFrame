using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WS1
{

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
			CurrentBullets = new ReactiveCollection<BulletViewModel> ();
			RC_AddSomeBullet = new ReactiveCommand ();
			RC_RemoveSomeBullet = new ReactiveCommand ();
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

		/*  */
		[JsonProperty] public ReactiveCollection<BulletViewModel> CurrentBullets;

		/*  */
		public ReactiveCommand RC_AddSomeBullet;
		

		/*  */
		public ReactiveCommand RC_RemoveSomeBullet;
		
	}

	
	public class AddSomeBulletCommand : ViewModelCommandBase
	{
	
	}
	
	public class RemoveSomeBulletCommand : ViewModelCommandBase
	{
	
	}
	

}