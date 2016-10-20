using UnityEngine;
using UniRx;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class FirstViewBase : ViewBase
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

	public bool AutoCreateViewModel = false;

	public string ViewModelInitValueJson;

	public override void Initialize (ViewModelBase viewModel)
	{
		if (viewModel != null) {
			VM = (FirstViewModel)viewModel;
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
			VM = JsonConvert.DeserializeObject<FirstViewModel> (ViewModelInitValueJson);
		} else {
			VM = new FirstViewModel ();
		}
	}

	public override void Bind ()
	{
		base.Bind ();
		VM.RP_LabelTextNum.Subscribe (OnChanged_LabelTextNum);
		VM.Numbers.ObserveAdd ().Subscribe (OnAdd_Numbers);
		VM.Numbers.ObserveRemove ().Subscribe (OnRemove_Numbers);
		VM.Numbers.ObserveReplace ().Subscribe (OnReplace_Numbers);
		VM.MyDictionary.ObserveAdd ().Subscribe (OnAdd_MyDictionary);
		VM.MyDictionary.ObserveRemove ().Subscribe (OnRemove_MyDictionary);
		VM.MyDictionary.ObserveReplace ().Subscribe (OnReplace_MyDictionary);
		VM.RC_AddNum.Subscribe<AddNumCommand> (OnExecuted_AddNum);
		VM.RC_ButtonClick.Subscribe (OnExecuted_ButonClick);
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

	public virtual void OnReplace_Numbers (CollectionReplaceEvent<int> e)
	{
	}

	public virtual void OnAdd_MyDictionary (DictionaryAddEvent<string, string> e)
	{
	}

	public virtual void OnRemove_MyDictionary (DictionaryRemoveEvent<string, string> e)
	{
	}

	public virtual void OnReplace_MyDictionary (DictionaryReplaceEvent<string, string> e)
	{
	}

	public virtual void OnExecuted_AddNum (AddNumCommand command)
	{
	}

	public virtual void OnExecuted_ButonClick (Unit unit)
	{
	}
}
