using System.Text.Json;
using System.Threading.Tasks;
using PromptQuest.Models;

namespace PromptQuest.Services {
	public class GameService {
		private readonly IHttpContextAccessor _httpContextAccessor;
		private const string GameStateSessionKey = "GameState";

		public GameService(IHttpContextAccessor httpContextAccessor) {
			_httpContextAccessor = httpContextAccessor;
		}

		private GameStateModel GetGameState() {
			var session = _httpContextAccessor.HttpContext.Session;
			var gameStateJson = session.GetString(GameStateSessionKey);
			return gameStateJson != null ? JsonSerializer.Deserialize<GameStateModel>(gameStateJson) : new GameStateModel();
		}

		private void SetGameState(GameStateModel gameStateModel) {
			var session = _httpContextAccessor.HttpContext.Session;
			var gameStateJson = JsonSerializer.Serialize(gameStateModel);
			session.SetString(GameStateSessionKey,gameStateJson);
		}

		#region Get Methods
		/// <summary>Returns the entire game state. Used for loading the game view.</summary>
		public GameStateModel GetGameStateModel() {
			return GetGameState();
		}
		#endregion Get Methods - End

		#region Update Methods
		/// <summary>Updates the player. Also functions as a create player method if one does not exist yet.</summary>
		public void UpdatePlayer(PlayerModel playerModel) {
			GameStateModel gameStateModel = GetGameState();
			// Overide current playerModel with the new one.
			gameStateModel.PlayerModel = playerModel;
			SetGameState(gameStateModel);
		}
		#endregion Update Methods

		#region Combat

		/// <summary>Initiates combat between the player and a default enemy.</summary>
		public void StartCombat() {
			// Retrieve current game state from session.
			GameStateModel gameStateModel = GetGameState();
			// Let the view know that combat has started.
			gameStateModel.inCombat = true;
			// Generate a default enemy.
			gameStateModel.EnemyModel=GetDefaultEnemy();
			// Update session.
			SetGameState(gameStateModel);
		}

		#region Player Actions
		/// <summary>Calculates the damage that the player does to the enemy, updates the game state, then returns a CombatResult.</summary>
		public CombatResult PlayerAttack() {
			// Retrieve current game state from session.
			GameStateModel gameStateModel = GetGameState();
			// Calculate damage as attack - defense.
			int damage = gameStateModel.PlayerModel.Attack - gameStateModel.EnemyModel.Defense;
			// If attack is less than one make it one.
			if(damage < 1) {
				damage = 1;
			}
			// Update enemy health.
			gameStateModel.EnemyModel.CurrentHealth -= damage;
			// Return the result to the user.
			// Check if enemy died.
			if(gameStateModel.EnemyModel.CurrentHealth < 1) {
				gameStateModel.inCombat = false;// Enemy is dead, combat has ended.
			}
			// Update session.
			SetGameState(gameStateModel);
			CombatResult combatResult = new CombatResult();
			combatResult.EnemyHealth = gameStateModel.EnemyModel.CurrentHealth;
			combatResult.Message = $"You attacked the {gameStateModel.EnemyModel.Name} for {damage} damage";
			return combatResult;
		}

		public CombatResult PlayerUseHealthPotion() {
			GameStateModel gameStateModel = GetGameState();
			// If player has no potions, don't let them heal.
			if(gameStateModel.PlayerModel.HealthPotions <= 0) {
				return new CombatResult {
					Message = "You have no Health Potions!",
					PlayerHealth = gameStateModel.PlayerModel.CurrentHealth,
					PlayerHealthPotions = gameStateModel.PlayerModel.HealthPotions
				};
			}
			// If player is already at max health, don't let them heal.
			if(gameStateModel.PlayerModel.CurrentHealth == gameStateModel.PlayerModel.MaxHealth) {
				return new CombatResult {
					Message = "You are already at max health!",
					PlayerHealth = gameStateModel.PlayerModel.CurrentHealth,
					PlayerHealthPotions = gameStateModel.PlayerModel.HealthPotions
				};
			}
			// Update player health and number of potions.
			gameStateModel.PlayerModel.HealthPotions -= 1;
			gameStateModel.PlayerModel.CurrentHealth += 5;
			// If the potion put the player's health above maximum, set it to maximum.
			if(gameStateModel.PlayerModel.CurrentHealth > gameStateModel.PlayerModel.MaxHealth) {
				gameStateModel.PlayerModel.CurrentHealth = gameStateModel.PlayerModel.MaxHealth;
			}
			SetGameState(gameStateModel);
			return new CombatResult {
				Message = $"You healed to {gameStateModel.PlayerModel.CurrentHealth} HP!",
				PlayerHealth = gameStateModel.PlayerModel.CurrentHealth,
				PlayerHealthPotions = gameStateModel.PlayerModel.HealthPotions
			};
		}
		#endregion Player Actions - End

		#region Enemy Actions
		/// <summary>Calculates the damage that the enemy does to the player, updates the game state, then returns a CombatResult.</summary>
		public CombatResult EnemyAttack() {
			GameStateModel gameStateModel = GetGameState();
			// Calculate damage as attack - defense.
			int damage = gameStateModel.EnemyModel.Attack - gameStateModel.PlayerModel.Defense;
			// If attack is less than one make it one.
			if(damage < 1) {
				damage = 1;
			}
			// Update player health.
			gameStateModel.PlayerModel.CurrentHealth -= damage;
			// Check if player died.
			if(gameStateModel.PlayerModel.CurrentHealth < 1) {
				gameStateModel.inCombat = false;// Player is dead, combat has ended.
			}
			// Update session.
			SetGameState(gameStateModel);
			// Return the result to the user.
			CombatResult combatResult = new CombatResult();
			combatResult.PlayerHealth = gameStateModel.PlayerModel.CurrentHealth;
			combatResult.Message = $"The {gameStateModel.EnemyModel.Name} attacked you for {damage} damage";
			return combatResult;
		}
		#endregion Enemy Actions - End

		#region Helper Methods
		/// <summary>Returns a default enemy as a EnemyModel and updates the game state.</summary>
		private EnemyModel GetDefaultEnemy() {
			// Default enemy: Ancient Orc
			var enemyModelDefault = new EnemyModel { Name = "Ancient Orc",ImageUrl = "/images/PlaceholderAncientOrc.png",MaxHealth = 10,CurrentHealth = 10,Attack = 3 };
			return enemyModelDefault;
		}
		#endregion Helper Methods - End

		#endregion Combat - End	
	}
}

