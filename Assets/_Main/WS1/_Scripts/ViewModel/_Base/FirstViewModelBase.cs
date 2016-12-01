using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Newtonsoft.Json;

/*//////////////////////////////////////////////////////////////////////////////
 一个 element 的定义
一个 element 的定义的第二行 
//////////////////////////////////////////////////////////////////////////////*/
[JsonObjectAttribute (MemberSerialization.OptIn)]
public class FirstViewModelBase : FBViewModel
{
	public FirstViewModelBase ()
	{
	}

	public override void Initialize ()
	{
		base.Initialize ();
		
		RP_LabelTextNum = new ReactiveProperty<int> ();
		Numbers = new ReactiveCollection<int> ();
		MyDictionary = new ReactiveDictionary<string, string> ();
		RC_DefaultCommand = new ReactiveCommand ();
		RP_MyNewProperty = new ReactiveProperty<object> ();
		DefaultCollection = new ReactiveCollection<object> ();
		RP_DefaultProperty = new ReactiveProperty<object> ();
		RC_AddNum = new ReactiveCommand<AddNumCommand> ();
		RP_CurrentVector2 = new ReactiveProperty<UnityEngine.Vector2> ();
		RP_CurrentVector3 = new ReactiveProperty<UnityEngine.Vector3> ();
		RP_CurrentVector4 = new ReactiveProperty<UnityEngine.Vector4> ();
		RP_CurrentQuaternion = new ReactiveProperty<UnityEngine.Quaternion> ();
		RP_CurrentRect = new ReactiveProperty<UnityEngine.Rect> ();
		RP_CurrentBounds = new ReactiveProperty<UnityEngine.Bounds> ();
		RP_CurrentColor = new ReactiveProperty<UnityEngine.Color> ();
		RP_CurrentDateTime = new ReactiveProperty<DateTime> ();
	}

	public override void Attach ()
	{
		base.Attach ();
		FirstController.Instance.Attach (this);
	}

	

	/* LabelText# LabelTextNum's Comment */
	public ReactiveProperty<int> RP_LabelTextNum;

	[JsonProperty]
	public int LabelTextNum {
		get {
			return RP_LabelTextNum.Value;
		}
		set {
			RP_LabelTextNum.Value = value;
		}
	}

	/* This is a numbers list */
	[JsonProperty] public ReactiveCollection<int> Numbers;

	/* 我的字典# */
	[JsonProperty] public ReactiveDictionary<string, string> MyDictionary;

	/*  */
	public ReactiveCommand RC_DefaultCommand;
	

	/*  */
	public ReactiveProperty<object> RP_MyNewProperty;

	[JsonProperty]
	public object MyNewProperty {
		get {
			return RP_MyNewProperty.Value;
		}
		set {
			RP_MyNewProperty.Value = value;
		}
	}

	/*  */
	[JsonProperty] public ReactiveCollection<object> DefaultCollection;

	/*  */
	public ReactiveProperty<object> RP_DefaultProperty;

	[JsonProperty]
	public object DefaultProperty {
		get {
			return RP_DefaultProperty.Value;
		}
		set {
			RP_DefaultProperty.Value = value;
		}
	}

	/*  */
	public ReactiveCommand<AddNumCommand> RC_AddNum;
	

	/*  */
	public ReactiveProperty<UnityEngine.Vector2> RP_CurrentVector2;

	[JsonProperty]
	public UnityEngine.Vector2 CurrentVector2 {
		get {
			return RP_CurrentVector2.Value;
		}
		set {
			RP_CurrentVector2.Value = value;
		}
	}

	/*  */
	public ReactiveProperty<UnityEngine.Vector3> RP_CurrentVector3;

	[JsonProperty]
	public UnityEngine.Vector3 CurrentVector3 {
		get {
			return RP_CurrentVector3.Value;
		}
		set {
			RP_CurrentVector3.Value = value;
		}
	}

	/*  */
	public ReactiveProperty<UnityEngine.Vector4> RP_CurrentVector4;

	[JsonProperty]
	public UnityEngine.Vector4 CurrentVector4 {
		get {
			return RP_CurrentVector4.Value;
		}
		set {
			RP_CurrentVector4.Value = value;
		}
	}

	/*  */
	public ReactiveProperty<UnityEngine.Quaternion> RP_CurrentQuaternion;

	[JsonProperty]
	[JsonConverter (typeof(QuaternionJsonConverter))]
	public UnityEngine.Quaternion CurrentQuaternion {
		get {
			return RP_CurrentQuaternion.Value;
		}
		set {
			RP_CurrentQuaternion.Value = value;
		}
	}

	/*  */
	public ReactiveProperty<UnityEngine.Rect> RP_CurrentRect;

	[JsonProperty]
	[JsonConverter (typeof(RectJsonConverter))]
	public UnityEngine.Rect CurrentRect {
		get {
			return RP_CurrentRect.Value;
		}
		set {
			RP_CurrentRect.Value = value;
		}
	}

	/*  */
	public ReactiveProperty<UnityEngine.Bounds> RP_CurrentBounds;

	[JsonProperty]
	[JsonConverter (typeof(BoundsJsonConverter))]
	public UnityEngine.Bounds CurrentBounds {
		get {
			return RP_CurrentBounds.Value;
		}
		set {
			RP_CurrentBounds.Value = value;
		}
	}

	/*  */
	public ReactiveProperty<UnityEngine.Color> RP_CurrentColor;

	[JsonProperty]
	[JsonConverter (typeof(ColorJsonConverter))]
	public UnityEngine.Color CurrentColor {
		get {
			return RP_CurrentColor.Value;
		}
		set {
			RP_CurrentColor.Value = value;
		}
	}

	/*  */
	public ReactiveProperty<DateTime> RP_CurrentDateTime;

	[JsonProperty]
	public DateTime CurrentDateTime {
		get {
			return RP_CurrentDateTime.Value;
		}
		set {
			RP_CurrentDateTime.Value = value;
		}
	}
}


public class DefaultCommandCommand : ViewModelCommandBase
{

}

public class AddNumCommand : ViewModelCommandBase
{

	/*  */
	public int Param0;
	
	/*  */
	public int Param1;
	
}
