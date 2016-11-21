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

}
