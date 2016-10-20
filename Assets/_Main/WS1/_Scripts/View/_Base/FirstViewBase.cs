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

			JObject jo = JObject.Parse (ViewModelInitValueJson);
			VM.Numbers = jo ["Numbers"].Values<int> ().ToList ();

		} else {
			VM = new FirstViewModel ();
		}
	}

	public override void Bind ()
	{
		base.Bind ();

		VM.RP_LabelTextNum.Subscribe (OnChanged_LabelTextNum);
		VM.RC_Numbers.ObserveAdd ().Subscribe (OnAdd_Numbers);
		VM.RC_Numbers.ObserveRemove ().Subscribe (OnRemove_Numbers);
		VM.RCMD_AddNum.Subscribe<AddNumCommand> (OnExecuted_AddNum);
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

	public virtual void OnExecuted_AddNum (AddNumCommand command)
	{
	}
}
