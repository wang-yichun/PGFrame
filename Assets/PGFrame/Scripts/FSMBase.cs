using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PGFrame
{

	using UniRx;

	public class FSMBase<T>
	{
		public FSMBase ()
		{
			Initialize ();
		}

		public ReactiveProperty<T> CurrentState { get; set; }

		public virtual void Initialize ()
		{
			baseAttachDisposables = new CompositeDisposable ();
		}

		public virtual void Attach ()
		{
		}

		public virtual void Detach ()
		{
			if (baseAttachDisposables != null) {
				baseAttachDisposables.Dispose ();
				baseAttachDisposables = null;
			}
		}

		public CompositeDisposable baseAttachDisposables;
	}
}