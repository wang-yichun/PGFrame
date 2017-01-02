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

	public class NewFSM : FSMBase<NewFSM.State>
	{
		public enum State
		{
			DefaultState
		}

		public override void Initialize ()
		{
			CurrentState = new ReactiveProperty<State> (State.DefaultState);
			

			base.Initialize ();
		}

		public override void Attach ()
		{
			base.Attach ();
			
		}

		public override void Detach ()
		{
			base.Detach ();
		}
		
	}

}