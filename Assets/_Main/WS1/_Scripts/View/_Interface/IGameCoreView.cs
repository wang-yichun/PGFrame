namespace WS1
{

	using PGFrame;
	
	public interface IGameCoreView
	{
		
		IPlayerInfoView MyInfoView { get; set; }
		WS2.IBallView MyWS2BallView { get; set; }
	}

}