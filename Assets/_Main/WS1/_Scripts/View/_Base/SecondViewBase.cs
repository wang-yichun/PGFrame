using UnityEngine;
using UniRx;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SecondViewBase : ViewBase
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
		
		VM.RP_LabelTextNum.Subscribe (OnChanged_LabelTextNum);
		VM.Numbers.ObserveAdd ().Subscribe (OnAdd_Numbers);
		VM.Numbers.ObserveCountChanged ().Subscribe (OnCountChanged_Numbers);
		VM.Numbers.ObserveMove ().Subscribe (OnMove_Numbers);
		VM.Numbers.ObserveRemove ().Subscribe (OnRemove_Numbers);
		VM.Numbers.ObserveReplace ().Subscribe (OnReplace_Numbers);
		VM.Numbers.ObserveReset ().Subscribe (OnReset_Numbers);
		VM.MyDictionary.ObserveAdd ().Subscribe (OnAdd_MyDictionary);
		VM.MyDictionary.ObserveCountChanged ().Subscribe (OnCountChanged_MyDictionary);
		VM.MyDictionary.ObserveRemove ().Subscribe (OnRemove_MyDictionary);
		VM.MyDictionary.ObserveReplace ().Subscribe (OnReplace_MyDictionary);
		VM.MyDictionary.ObserveReset ().Subscribe (OnReset_MyDictionary);
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

	public virtual void OnCountChanged_Numbers (int count)
	{
	}

	public virtual void OnMove_Numbers (CollectionMoveEvent<int> e)
	{
	}

	public virtual void OnRemove_Numbers (CollectionRemoveEvent<int> e)
	{
	}

	public virtual void OnReplace_Numbers (CollectionReplaceEvent<int> e)
	{
	}

	public virtual void OnReset_Numbers (Unit u)
	{
	}

	public virtual void OnAdd_MyDictionary (DictionaryAddEvent<string, string> e)
	{
	}

	public virtual void OnCountChanged_MyDictionary (int count)
	{
	}

	public virtual void OnRemove_MyDictionary (DictionaryRemoveEvent<string, string> e)
	{
	}

	public virtual void OnReplace_MyDictionary (DictionaryReplaceEvent<string, string> e)
	{
	}

	public virtual void OnReset_MyDictionary (Unit u)
	{
	}

}
