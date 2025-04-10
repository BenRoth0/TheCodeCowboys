using Microsoft.EntityFrameworkCore;
using PromptQuest.Models;

namespace PromptQuest.Services {
	public interface IDatabaseService {
		GameState GetGameState(string userGoogleId);
		void SaveGameState(GameState gameState);
		void DeleteGameState(string userGoogleId);
		void DeleteItem(int itemId);
		void DeletePlayer(int playerId);
		void DeleteEnemy(int enemyId);
		bool IsAuthenticatedUser();
		string GetUserGoogleId();
	}

	public class DatabaseService:IDatabaseService {
		private readonly GameStateDbContext _dbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public DatabaseService(GameStateDbContext dbContext,IHttpContextAccessor httpContextAccessor) {
			_dbContext = dbContext;
			_httpContextAccessor = httpContextAccessor;
		}

		public GameState GetGameState(string userGoogleId) {
			return _dbContext.GameStates
					.Include(gs => gs.Player)
					.ThenInclude(p => p.Item)
					.Include(gs => gs.Enemy)
					.FirstOrDefault(gs => gs.UserGoogleId == userGoogleId);
		}

		public void SaveGameState(GameState gameState) {
			//Check if game state already exists for this user
			var existingGameState = _dbContext.GameStates.FirstOrDefault(gs => gs.UserGoogleId == gameState.UserGoogleId);
			if(existingGameState == null) {
				//GameState hasn't been saved yet, add it to the db
				_dbContext.GameStates.Add(gameState);
			}
			//GameState already exists for this user, save the changes made to it.
			_dbContext.SaveChanges();
		}

		/// <summary> Deletes the GameState with the given GoogleUserId. If it isn't found, nothing happens. </summary>
		public void DeleteGameState(string userGoogleId) {
			var gameState = _dbContext.GameStates.Find(userGoogleId);
			if(gameState != null) {
				_dbContext.GameStates.Remove(gameState);
				_dbContext.SaveChanges();
			}
		}


		/// <summary> Deletes the Item with the given ItemId. If it isn't found, nothing happens. </summary>
		public void DeleteItem(int itemId) {
			var item = _dbContext.Items.Find(itemId);
			if(item != null) {
				_dbContext.Items.Remove(item);
				_dbContext.SaveChanges();
			}
		}

		/// <summary> Deletes the Player with the given PlayerId. If it isn't found, nothing happens. </summary>
		public void DeletePlayer(int playerId) {
			var player = _dbContext.Players.Find(playerId);
			if(player != null) {
				_dbContext.Players.Remove(player);
				_dbContext.SaveChanges();
			}
		}

		/// <summary> Deletes the Enemy with the given EnemyId. If it isn't found, nothing happens. </summary>
		public void DeleteEnemy(int enemyId) {
			var enemy = _dbContext.Enemies.Find(enemyId);
			if(enemy != null) {
				_dbContext.Enemies.Remove(enemy);
				_dbContext.SaveChanges();
			}
		}

		/// <summary> Returns true if the current user is authenticated. </summary>
		public bool IsAuthenticatedUser() {
			return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
		}

		/// <summary> Returns the current users GoogleAccountId if they are authenticated. If the user isn't authenticated, returns a blank string </summary>
		public string GetUserGoogleId() {
			return _httpContextAccessor.HttpContext?.User?.FindFirst("GoogleAccountId")?.Value ?? string.Empty;
		}
	}
}
