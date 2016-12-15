using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WS1 {

	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UniRx;

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
		RP_CurrentAC = new ReactiveProperty<UnityEngine.AnimationCurve> ();
		RP_CurrentDateTime = new ReactiveProperty<DateTime> ();
		RP_CurrentTimeSpan = new ReactiveProperty<TimeSpan> ();
		RP_CurrentJObject = new ReactiveProperty<JObject> ();
		RP_CurrentJArray = new ReactiveProperty<JArray> ();
		V3List = new ReactiveCollection<UnityEngine.Vector3> ();
		RectList = new ReactiveCollection<UnityEngine.Rect> ();
		RP_MyEA = new ReactiveProperty<EA> ();
		RP_SCA_a = new ReactiveProperty<SCA> ();
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
	public ReactiveProperty<UnityEngine.AnimationCurve> RP_CurrentAC;

	[JsonProperty]
	public UnityEngine.AnimationCurve CurrentAC {
		get {
			return RP_CurrentAC.Value;
		}
		set {
			RP_CurrentAC.Value = value;
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

	/*  */
	public ReactiveProperty<TimeSpan> RP_CurrentTimeSpan;

	[JsonProperty]
	public TimeSpan CurrentTimeSpan {
		get {
			return RP_CurrentTimeSpan.Value;
		}
		set {
			RP_CurrentTimeSpan.Value = value;
		}
	}

	/*  */
	public ReactiveProperty<JObject> RP_CurrentJObject;

	[JsonProperty]
	public JObject CurrentJObject {
		get {
			return RP_CurrentJObject.Value;
		}
		set {
			RP_CurrentJObject.Value = value;
		}
	}

	/*  */
	public ReactiveProperty<JArray> RP_CurrentJArray;

	[JsonProperty]
	public JArray CurrentJArray {
		get {
			return RP_CurrentJArray.Value;
		}
		set {
			RP_CurrentJArray.Value = value;
		}
	}

	/*  */
	[JsonProperty] public ReactiveCollection<UnityEngine.Vector3> V3List;

	/*  */
	[JsonProperty] public ReactiveCollection<UnityEngine.Rect> RectList;

	/*  */
	public ReactiveProperty<EA> RP_MyEA;

	[JsonProperty]
	public EA MyEA {
		get {
			return RP_MyEA.Value;
		}
		set {
			RP_MyEA.Value = value;
		}
	}

	/*  */
	public ReactiveProperty<SCA> RP_SCA_a;

	[JsonProperty]
	public SCA SCA_a {
		get {
			return RP_SCA_a.Value;
		}
		set {
			RP_SCA_a.Value = value;
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


}