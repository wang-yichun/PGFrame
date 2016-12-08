/*//////////////////////////////////////////////////////////////////////////////
 This is my first SimpleClass SCA. 
//////////////////////////////////////////////////////////////////////////////*/
public class SCABase : SimpleClassBase
{
	

	/* First # member */
	[UnityEngine.SerializeField, UnityEngine.Tooltip("First # member")]
	private string _DefaultSimpleClass;

	public string DefaultSimpleClass { get { return _DefaultSimpleClass; } set { _DefaultSimpleClass = value; } }

	/* 2nd # member */
	[UnityEngine.SerializeField, UnityEngine.Tooltip("2nd # member")]
	private int _DefaultSimpleClass1;

	public int DefaultSimpleClass1 { get { return _DefaultSimpleClass1; } set { _DefaultSimpleClass1 = value; } }

	/*  */
	[UnityEngine.SerializeField, UnityEngine.Tooltip("")]
	private UnityEngine.Vector3 _DefaultSimpleClass2;

	public UnityEngine.Vector3 DefaultSimpleClass2 { get { return _DefaultSimpleClass2; } set { _DefaultSimpleClass2 = value; } }
}
