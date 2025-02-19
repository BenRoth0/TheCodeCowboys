namespace PromptQuest.Models
{
	public static class CurrentPlayer
	{
		public static PlayerModel _player = new PlayerModel { Name = "Test Player", MaxHealth = 10, CurrentHealth = 10 };

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
