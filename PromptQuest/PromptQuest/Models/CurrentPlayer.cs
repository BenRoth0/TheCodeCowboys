namespace PromptQuest.Models
{
	public static class CurrentPlayer
	{
		public static PlayerModel _player;

		public static void SetPlayer(PlayerModel player) 
		{
			_player = player;
		}
		public static PlayerModel GetPlayer() 
		{
			return _player;
		}
	}
}
