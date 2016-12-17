using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WS1
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	/*//////////////////////////////////////////////////////////////////////////////
	  
	//////////////////////////////////////////////////////////////////////////////*/
	[JsonObjectAttribute (MemberSerialization.OptIn)]
	public class BulletViewModelBase : ViewModelBase
	{
		public BulletViewModelBase ()
		{
		}

		public override void Initialize ()
		{
			
			RP_BType = new ReactiveProperty<BulletType> ();
			
			base.Initialize ();
		}

		public override void Attach ()
		{
			base.Attach ();
			BulletController.Instance.Attach (this);
		}

		

		/*  */
		public ReactiveProperty<BulletType> RP_BType;

		[JsonProperty]
		public BulletType BType {
			get {
				return RP_BType.Value;
			}
			set {
				RP_BType.Value = value;
			}
		}
	}

	

}