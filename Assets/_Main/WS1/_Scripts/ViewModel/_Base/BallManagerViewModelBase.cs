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
	public class BallManagerViewModelBase : ViewModelBase
	{
		public BallManagerViewModelBase ()
		{
		}

		public override void Initialize ()
		{
			base.Initialize ();
			
			RP_MyBallType = new ReactiveProperty<WS2.BallType> ();
			MyBalls = new ReactiveCollection<WS2.BallViewModel> ();
			RP_CurrentBall = new ReactiveProperty<WS2.BallViewModel> ();
		}

		public override void Attach ()
		{
			base.Attach ();
			BallManagerController.Instance.Attach (this);
		}

		

		/*  */
		public ReactiveProperty<WS2.BallType> RP_MyBallType;

		[JsonProperty]
		public WS2.BallType MyBallType {
			get {
				return RP_MyBallType.Value;
			}
			set {
				RP_MyBallType.Value = value;
			}
		}

		/*  */
		[JsonProperty] public ReactiveCollection<WS2.BallViewModel> MyBalls;

		/*  */
		public ReactiveProperty<WS2.BallViewModel> RP_CurrentBall;

		
		public WS2.BallViewModel CurrentBall {
			get {
				return RP_CurrentBall.Value;
			}
			set {
				RP_CurrentBall.Value = value;
			}
		}
	}

	

}