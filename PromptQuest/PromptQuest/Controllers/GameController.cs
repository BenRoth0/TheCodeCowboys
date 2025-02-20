using Microsoft.AspNetCore.Mvc;
using PromptQuest.Models;
using PromptQuest.Services;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace PromptQuest.Controllers {

	public class GameController:Controller {
		private readonly ILogger<GameController> _logger;
		private readonly GameService _gameService;
		public GameController(ILogger<GameController> logger,GameService gameService) {
			_logger = logger;
			_gameService = gameService;
		}

		[HttpGet]
		public IActionResult CreateCharacter() {
			return View();
		}

		[HttpPost]
		public IActionResult CreateCharacter(PlayerModel player) {
			// Default stats for now.
			player.MaxHealth = 10;
			player.CurrentHealth = 10;
			player.HealthPotions = 2;
			player.Attack = 1;
			if(ModelState.IsValid) {// Character created succesfully
				_gameService.UpdatePlayer(player);// Add player to the game state.
				// Start combat right away, for now.
				_gameService.StartCombat();
				return RedirectToAction("Game");
			}
			else {
				return View();
			}
		}

		[HttpGet]
		public IActionResult Game() {
			return View();
		}

		[HttpGet]
		public JsonResult GetGameState() {
			GameStateModel gameState = _gameService.GetGameStateModel();
			// Return the entire game state.
			return Json(gameState);
		}

		[HttpPost]
		public JsonResult PlayerAttack() {
			CombatResult combatResult = _gameService.PlayerAttack();
			// Return only what could change as a result of this action.
			return Json(combatResult);
		}

		[HttpPost]
		public JsonResult PlayerUseHealthPotion() {
			CombatResult combatResult = _gameService.PlayerUseHealthPotion();
			// Return only what could change as a result of this action.
			return Json(combatResult);
		}

		[HttpPost]
		public JsonResult EnemyAttack() {
			CombatResult combatResult = _gameService.EnemyAttack();
			// Return only what could change as a result of this action.
			return Json(combatResult);
		}
	}
}