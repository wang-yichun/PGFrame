using System;
using System.Collections;
using System.Collections.Generic;

namespace WSAsyncTest
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class TesterViewModel : TesterViewModelBase
	{
		public override void Initialize ()
		{
			base.Initialize ();
			SubscribeCounterEnable ();
		}

		public ICancelable CounterCanceler;

		public void SubscribeCounterEnable ()
		{
			RP_CounterEnable.Subscribe (_ => {
				if (_) {
					CounterCanceler = (ICancelable)Observable.EveryUpdate ().Subscribe (l => CountValue = (int)l);
				} else {
					if (CounterCanceler != null) {
						CounterCanceler.Dispose ();
					}
				}
			}).AddTo (baseAttachDisposables);
		}
	}

}