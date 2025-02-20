document.addEventListener("DOMContentLoaded", async function () {
	// "Global" Variable too keep track of the game state so that functions don't have to be passed parameters
	let gameState;

	// Page loaded, get current game state
	let response = await fetch("/Game/GetGameState");
	gameState = await response.json();
	// Update player display with loaded data.
	updatePlayerDisplay();
	if (gameState.inCombat == true) {// Player is in combat
		updateEnemyDisplay(); // Update enemy displays with loaded data.
		//addLogEntry("You were attacked by an " + gameState.enemyModel.name + "!");
		// Player always starts first, this works for now but probably won't work long term.
		playerTurn();  
	}
	else {// Player is not in combat, hide combat UI
		// Disable combat buttons just in case.
		disableAttackButton();
		disableHealthPotionButton();
		// Hide combat UI.
		hideCombatUI();
	}

	// Function to handle player's turn.
	function playerTurn() {
		// Let the player make an action now because it is their turn.
		enableAttackButton();
		enableHealthPotionButton();
	}

	// Function to handle player's attack.
	function playerAttack() {
		fetch("/Game/PlayerAttack", { method: "POST" })
			.then(response => response.json())
			.then(combatResult => {
				gameState.enemyModel.currentHealth = combatResult.enemyHealth;
				updateHealthDisplay();
				addLogEntry(combatResult.message);
				// Check if combat is over
				if (gameState.enemyModel.currentHealth < 1) { // No longer in combat
					addLogEntry("The enemy has been defeated!");
					// Disable combat buttons just in case.
					disableAttackButton();
					disableHealthPotionButton();
					hideCombatUI();
				} else { // Still in combat
					enemyTurn(); // Now it is the enemy's turn.
				}
			});
	}

	// Function to let the player use a health potion.
	function playerUseHealthPotion() {
		fetch("/Game/PlayerUseHealthPotion", { method: "POST" })
			.then(response => response.json())
			.then(combatResult => {
				gameState.playerModel.currentHealth = combatResult.playerHealth;
				gameState.playerModel.healthPotions = combatResult.playerHealthPotions;
				updateHealthDisplay();
				updateHealthPotionDisplay();
				addLogEntry(combatResult.message);
			});
	}

	// Function to handle the enemy's turn
	function enemyTurn() {
		// Disable combat buttons because it isn't the player's turn.
		disableAttackButton();
		disableHealthPotionButton();
		// Add a small delay so that the enemy's turn takes time.
		setTimeout(() => {
			enemyAttack(); // Enemy attacks blindly for now.
		}, 1000);
	}

	// Function to handle the enemy's attack
	function enemyAttack() {
		fetch("/Game/EnemyAttack", { method: "POST" })
			.then(response => response.json())
			.then(combatResult => {
				gameState.playerModel.currentHealth = combatResult.playerHealth;
				updateHealthDisplay();
				addLogEntry(combatResult.message);
				// Check if combat is over
				if (gameState.playerModel.currentHealth < 1) {// No longer in combat
					addLogEntry("You have been defeated!");
					hideCombatUI();
				} else {// Still in combat
					playerTurn(); // Now it is the player's turn.
				}
			});
	}

	//----------- Helper Functions -----------------------------------------------------------------------------------------------------------

	// Function to update the player's display
	function updatePlayerDisplay() {
		document.getElementById("player-name").textContent = gameState.playerModel.name;
		document.getElementById("player-image").src = "/images/PlaceholderPlayerPortrait.png";
		document.getElementById("player-image").alt = gameState.playerModel.name;
		document.getElementById("player-attack").textContent = gameState.playerModel.attack;
		document.getElementById("player-defense").textContent = gameState.playerModel.defense;
		document.getElementById("player-hp").textContent = gameState.playerModel.currentHealth + "/" + gameState.playerModel.maxHealth + " HP";
		document.getElementById("player-health-potions").textContent = gameState.playerModel.healthPotions;
	}

	// Function to update the enemy's display
	function updateEnemyDisplay() {
		document.getElementById("enemy-name").textContent = gameState.enemyModel.name;
		document.getElementById("enemy-image").src = gameState.enemyModel.imageUrl;
		document.getElementById("enemy-image").alt = gameState.enemyModel.name;
		document.getElementById("enemy-attack").textContent = gameState.enemyModel.attack;
		document.getElementById("enemy-defense").textContent = gameState.enemyModel.defense;
		document.getElementById("enemy-hp").textContent = gameState.enemyModel.currentHealth + "/" + gameState.enemyModel.maxHealth + " HP";
	}

	// Function to update the health displays when health changes
	function updateHealthDisplay() {
		document.getElementById("player-hp").textContent = gameState.playerModel.currentHealth + "/" + gameState.playerModel.maxHealth + " HP";
		document.getElementById("enemy-hp").textContent = gameState.enemyModel.currentHealth + "/" + gameState.enemyModel.maxHealth + " HP";
	}

	// Function to update the health potions display
	function updateHealthPotionDisplay() {
		document.getElementById("player-health-potions").textContent = gameState.playerModel.healthPotions;
	}

	// Function to add log entries to the dialog box
	function addLogEntry(message) {
		const dialogBox = document.querySelector(".DialogBox");
		const logLimit = 5;
		if (dialogBox.childElementCount >= logLimit) {
			dialogBox.innerHTML = "";
		}
		const logDiv = document.createElement("div");
		logDiv.textContent = message;
		dialogBox.appendChild(logDiv);
	}

	// Function to disable the attack button
	function disableAttackButton() {
		const attackButton = document.getElementById("attack-btn");
		attackButton.removeEventListener("click", playerAttack);
		attackButton.disabled = true;
		attackButton.classList.add("PQButtonDisabled");
	}

	// Function to enable the attack button
	function enableAttackButton() {
		const attackButton = document.getElementById("attack-btn");
		attackButton.addEventListener("click", playerAttack);
		attackButton.disabled = false;
		attackButton.classList.remove("PQButtonDisabled");
	}

	// Function to disable the health potion button
	function disableHealthPotionButton() {
		const healButton = document.getElementById("health-potion-btn");
		healButton.removeEventListener("click", playerUseHealthPotion);
		healButton.disabled = true;
		healButton.classList.add("PQButtonDisabled");
	}

	// Function to enable the health potion button.
	function enableHealthPotionButton() {
		const healButton = document.getElementById("health-potion-btn");
		healButton.addEventListener("click", playerUseHealthPotion);
		healButton.disabled = false;
		healButton.classList.remove("PQButtonDisabled");
	}

	// Function to hide the enemy display.
	function hideCombatUI() {
		// Hide the enemy display.
		const enemyDisplay = document.getElementById("enemy-display");
		enemyDisplay.style.display = "none";
		// Hide the combat buttons display.
		const combatButtonsDisplay = document.getElementById("combat-buttons-display");
		combatButtonsDisplay.style.display = "none";
	}

	//----------- Helper Functions - End -----------------------------------------------------------------------------------------------------------
});
