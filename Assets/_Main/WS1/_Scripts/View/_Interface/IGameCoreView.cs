namespace WS1
{

	public interface IGameCoreView
	{
		
		IPlayerInfoView MyInfoView { get; set; }
		WS2.IBallView MyWS2BallView { get; set; }
	}

}