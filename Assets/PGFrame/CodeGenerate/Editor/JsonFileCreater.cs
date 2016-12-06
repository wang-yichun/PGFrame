public abstract class JsonFileCreater
{
	protected PGFrameWindow FrameWindow;

	public JsonFileCreater (PGFrameWindow frameWindow, string name)
	{
		this.FrameWindow = frameWindow;
		this.Name = name;
	}

	protected string Name;

	public abstract void Create ();

	public abstract string MakeJsonName ();
}
