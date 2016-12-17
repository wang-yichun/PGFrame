using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WS1
{

	using PGFrame;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

	public class SecondHudViewBase : ViewBase , ISecondView
	{
		public SecondViewModel VM;

		public SecondViewModel Second {
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
				VM = (SecondViewModel)viewModel;
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
				VM = new SecondViewModel ();
			} else {
				VM = JsonConvert.DeserializeObject<SecondViewModel> (ViewModelInitValueJson);
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
			
			VM.RP_IntValue.Subscribe (OnChanged_IntValue);
			VM.IntList.ObserveAdd ().Subscribe (OnAdd_IntList);
			VM.IntList.ObserveRemove ().Subscribe (OnRemove_IntList);
			VM.IntDictionary.ObserveAdd ().Subscribe (OnAdd_IntDictionary);
			VM.IntDictionary.ObserveRemove ().Subscribe (OnRemove_IntDictionary);
			VM.RC_IntCommand.Subscribe<IntCommandCommand> (OnExecuted_IntCommand);
			VM.RC_SimpleCommand.Subscribe (OnExecuted_SimpleCommand);
		}

		public override void AfterBind ()
		{
			base.AfterBind ();
		}

		

		public virtual void OnChanged_IntValue (int value)
		{
		}

		public virtual void OnAdd_IntList (CollectionAddEvent<int> e)
		{
		}

		public virtual void OnRemove_IntList (CollectionRemoveEvent<int> e)
		{
		}

		public virtual void OnAdd_IntDictionary (DictionaryAddEvent<string, int> e)
		{
		}

		public virtual void OnRemove_IntDictionary (DictionaryRemoveEvent<string, int> e)
		{
		}

			public virtual void OnExecuted_IntCommand (IntCommandCommand command)
			{
			}

			public virtual void OnExecuted_SimpleCommand (Unit unit)
			{
			}

		
	}

}