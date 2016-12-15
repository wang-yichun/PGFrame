namespace WS1 {

	/*//////////////////////////////////////////////////////////////////////////////
	 test derive 
	//////////////////////////////////////////////////////////////////////////////*/
	public class SCBBase : SCA
	{
		

	/*  */
	[UnityEngine.SerializeField, UnityEngine.Tooltip("")]
	private int _SCB_Member;

	public int SCB_Member { get { return _SCB_Member; } set { _SCB_Member = value; } }
	}

}