using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WSAsyncTest
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class TesterViewBase : ViewBase , ITesterView
	{
		public  TesterViewModel VM;

		public TesterViewModel Tester {
			get {
				return VM;
			}
		}

		public override void CreateViewModel ()
		{
			if (UseEmptyViewModel || string.IsNullOrEmpty (ViewModelInitValueJson)) {
				VM = new TesterViewModel ();
			} else {
				VM = JsonConvert.DeserializeObject<TesterViewModel> (ViewModelInitValueJson);
				ViewModelPropertyRef ();
			}
			
			VM.AddHostView (ViewModelBase.DefaultViewBaseKey, this);
		}

		public override ViewModelBase GetViewModel ()
		{
			return VM;
		}
		
		public override void SetViewModel (ViewModelBase viewModel)
		{
			VM = (TesterViewModel)viewModel;
		}

		public override void Initialize (ViewModelBase viewModel)
		{
			if (viewModel != null) {
				VM = (TesterViewModel)viewModel;
				VM.AddHostView (ViewModelBase.DefaultViewBaseKey, this);
			} else {
				if (AutoCreateViewModel && VM == null) {
					CreateViewModel ();
				}
			}

			base.Initialize (VM);
		}

		public override void ViewModelPropertyRef ()
		{
			
		}

		public override void BeforeBind ()
		{
			base.BeforeBind ();
		}
		
		public override void Bind ()
		{
			base.Bind ();
			
			VM.RP_CountValue.Subscribe (OnChanged_CountValue).AddTo (baseBindDisposables);
		}

		public override void AfterBind ()
		{
			base.AfterBind ();
		}

		

		public virtual void OnChanged_CountValue (int value)
		{
		}

		
	}

}