
public class Singleton<T>
		where T: Singleton<T>, new()
{
	protected static T instance;

	public static T Instance {
		get {
			if (instance == null) {
				CreateNewOne ();
			}
			return instance;
		}
	}

	public static T CreateNewOne ()
	{
		instance = new T ();
		instance.InitializeSingleton ();
		return instance;
	}

	protected virtual void InitializeSingleton ()
	{
	}

}
