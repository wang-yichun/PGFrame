using UnityEngine;
using UniRx;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SecondHudViewBase : ViewBase
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

	public bool AutoCreateViewModel = false;

	public string ViewModelInitValueJson;

	public override void Initialize (ViewModelBase viewModel)
	{
		if (viewModel != null) {
			VM = (SecondViewModel)viewModel;
		} else {
			if (AutoCreateViewModel) {
				if (VM == null) {
					CreateViewModel ();
				}
			}
		}

		base.Initialize (null);
	}

	public void CreateViewModel ()
	{
		if (string.IsNullOrEmpty (ViewModelInitValueJson) == false) {
			VM = JsonConvert.DeserializeObject<SecondViewModel> (ViewModelInitValueJson);
		} else {
			VM = new SecondViewModel ();
		}
	}

	public override void Bind ()
	{
		base.Bind ();
		
		VM.RP_IntValue.Subscribe (OnChanged_IntValue);
		VM.Numbers.ObserveAdd ().Subscribe (OnAdd_Numbers);
		VM.Numbers.ObserveRemove ().Subscribe (OnRemove_Numbers);
		VM.MyDictionary.ObserveAdd ().Subscribe (OnAdd_MyDictionary);
		VM.MyDictionary.ObserveRemove ().Subscribe (OnRemove_MyDictionary);
	}

	public override void AfterBind ()
	{
		base.AfterBind ();
	}

	

	public virtual void OnChanged_IntValue (int value)
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
