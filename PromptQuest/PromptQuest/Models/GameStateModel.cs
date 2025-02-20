namespace PromptQuest.Models {
	public class GameStateModel {
		public PlayerModel PlayerModel { get; set; }
		public EnemyModel EnemyModel { get; set; }
		public bool inCombat { get; set; } = false;
	}
}
