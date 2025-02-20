namespace PromptQuest.Models {
	public class CombatResult {
		public string Message { get; set; } = "";
		public int PlayerHealth { get; set; } = 0;
		public int PlayerHealthPotions { get; set; } = 0;
		public int EnemyHealth { get; set; } = 0;
		public bool isPlayerDead { get; set; } = false;
		public bool isEnemyDead { get; set; } = false;
	}
}

