using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WS1
{

	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class FirstHudViewBase : ViewBase , IFirstView
	{
		public FirstViewModel VM;

		public FirstViewModel First {
			get {
				return VM;
			}
		}

		public override ViewModelBase GetViewModel ()
		{
			return VM;
		}

		public override void Initialize (ViewModelBase viewModel)
		{
			if (viewModel != null) {
				VM = (FirstViewModel)viewModel;
				VM.AddHostView (ViewModelBase.DefaultViewBaseKey, this);
			} else {
				if (AutoCreateViewModel && VM == null) {
					CreateViewModel ();
				}
			}

			base.Initialize (VM);
		}

		public override void CreateViewModel ()
		{
			if (UseEmptyViewModel || string.IsNullOrEmpty (ViewModelInitValueJson)) {
				VM = new FirstViewModel ();
			} else {
				VM = JsonConvert.DeserializeObject<FirstViewModel> (ViewModelInitValueJson);
				ViewModelPropertyRef ();
			}
			
			VM.AddHostView (ViewModelBase.DefaultViewBaseKey, this);
		}

		public void ViewModelPropertyRef ()
		{
			
		}

		public override void Bind ()
		{
			base.Bind ();
			
			VM.RP_LabelTextNum.Subscribe (OnChanged_LabelTextNum);
			VM.Numbers.ObserveAdd ().Subscribe (OnAdd_Numbers);
			VM.Numbers.ObserveRemove ().Subscribe (OnRemove_Numbers);
			VM.MyDictionary.ObserveAdd ().Subscribe (OnAdd_MyDictionary);
			VM.MyDictionary.ObserveRemove ().Subscribe (OnRemove_MyDictionary);
		}

		public override void AfterBind ()
		{
			base.AfterBind ();
		}

		

		public virtual void OnChanged_LabelTextNum (int value)
		{
		}

		public virtual void OnAdd_Numbers (CollectionAddEvent<int> e)
		{
		}

		public virtual void OnRemove_Numbers (CollectionRemoveEvent<int> e)
		{
		}

		public virtual void OnAdd_MyDictionary (DictionaryAddEvent<string, string> e)
		{
		}

		public virtual void OnRemove_MyDictionary (DictionaryRemoveEvent<string, string> e)
		{
		}

		
	}

}