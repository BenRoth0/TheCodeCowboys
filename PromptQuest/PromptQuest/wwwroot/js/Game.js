// Cached GameState. Each server request returns a diff that is merged into this. Completely refreshed on page relod.
let gameState;
let tutorialflag;

document.addEventListener("DOMContentLoaded", function () {
	// Page loaded, get current game state and store it locally.  
	loadGame();
});

async function loadGame() {
	let response = await fetch("/Game/GetGameState");
	gameState = await response.json();
	// Check if the user doesn't have a character
	if (gameState.player == null) {// new player or session expired
		window.location.href = "/"; // boot them to the Main Menu.
	}
	let flagresponse = await fetch("/Game/IsTutorial"); // get the tutorial flag
	tutorialflag = await flagresponse.json()//if in the tutorial: start the tutorial
	if (tutorialflag) {
		startTutorial()
	}
	// Update display with loaded data.  
	updateDisplay();
}

// Function that makes an ajax call telling the game engine to process the player's action, then updates the local gameState variable with the response.  
async function executePlayerAction(action) {
	const response = await fetch(`/Game/PlayerAction?action1=${encodeURIComponent(action)}`, { method: 'Post', });
	await processResponse(response);
}

// Function to update the player's display  
function updateDisplay() {
	gameState.listMessages.forEach((message) => {
		addLogEntry(message);
	});
	// Update Player display.
	document.querySelectorAll(".player-name").forEach(el => { el.textContent = gameState.player.name; });
	document.querySelectorAll(".player-image").forEach(el => { el.src = "/images/" + gameState.player.class + ".png"; }); // Placeholder image for now.
	document.querySelectorAll(".player-image").forEach(el => { el.alt = gameState.player.name; });
	const equippedItem = gameState.player.itemEquipped
	document.querySelectorAll(".player-attack").forEach(el => { el.textContent = gameState.player.attack + equippedItem?.attack??0; });
	document.querySelectorAll(".player-defense").forEach(el => { el.textContent = gameState.player.defense + equippedItem?.defense??0; });
	document.querySelectorAll(".player-hp").forEach(el => { el.textContent = gameState.player.currentHealth + "/" + gameState.player.maxHealth + " HP"; });
	document.getElementById("player-health-potions").textContent = gameState.player.healthPotions;
	if (gameState.inCombat) {
		//Enable/disable combat buttons
		if (gameState.isPlayersTurn) {
			enableCombatButtons();
		}
		else {
			disableCombatButtons();
		}
		showCombatUI();
		hideCampsiteUI();
		hideEventUI();
		// Update Enemy display.
		document.getElementById("enemy-name").textContent = gameState.enemy.name;
		document.getElementById("enemy-image").src = gameState.enemy.imageUrl;
		document.getElementById("enemy-image").alt = gameState.enemy.name;
		document.getElementById("enemy-attack").textContent = gameState.enemy.attack;
		document.getElementById("enemy-defense").textContent = gameState.enemy.defense;
		document.getElementById("enemy-hp").textContent = gameState.enemy.currentHealth + "/" + gameState.enemy.maxHealth + " HP";
	}
	else if (gameState.inCampsite) {
		hideCombatUI();
		hideEventUI();
		showCampsiteUI();
		if (gameState.isLocationComplete) {
			disableCampsiteButtons();
		}
		// Gets rid of last combat's messages;
		clearDialogBox();
		// Update Map
		updateMap();
		// Inform the player about the campsite
		addLogEntry("Rest at the campsite to heal 30% of your maximum HP and refill Health Potions");
	}
	else if (gameState.inEvent)
	{
		hideCombatUI();
		hideCampsiteUI();
		showEventUI();
		if (gameState.isLocationComplete) {
			disableEventButtons();
		}
		// Gets rid of last combat's messages;
		clearDialogBox();
		// Update Map
		updateMap();
		// Inform the player about the event
		addLogEntry("A prickly bush lies in your path. A few red objects shimmer from fairly deep inside. Reach in and grab them?");
	}
	else {
		hideCombatUI();
		hideCampsiteUI();
		hideEventUI();
	}
}

// Function to add log entries to the dialog box.  
function addLogEntry(message) {
	const dialogBox = document.querySelector(".dialog-box");
	const logLimit = 5;
	if (dialogBox.childElementCount >= logLimit) {
		dialogBox.innerHTML = "";
	}
	const logDiv = document.createElement("div");
	logDiv.textContent = message;
	dialogBox.appendChild(logDiv);
}

function clearDialogBox() {
	const dialogBox = document.querySelector(".dialog-box");
	dialogBox.innerHTML = "";
}

async function processResponse(response) {
	if (response.text == null) {
		return;
	}
	if (!response.ok) {
		throw new Error(`Server error: ${response.status} ${response.statusText}`);
	}
	const json = await response.json();
	console.log(json);
	mergeDiffIntoCache(gameState, json);
	updateDisplay();
}

//This method essentially does the opposite of what GameController.GenerateDiff does then merges it with the cached gameState
function mergeDiffIntoCache(target, json) {
	Object.keys(json).forEach(key => {
		if (key === "PlayerLocation") {
			let breakpoint = "here";
		}
		let newValue = json[key];
		let normalizedKey = key.charAt(0).toLowerCase() + key.slice(1);

		// Check if target has this key
		if (target.hasOwnProperty(normalizedKey)) {
			let existingValue = target[normalizedKey];

			// Handle collections: Replace entire list
			if (Array.isArray(newValue)) {
				target[normalizedKey] = newValue;
			}
			// Handle nested objects recursively
			else if (typeof newValue === "object" && newValue !== null) {
				if (typeof existingValue === "object" && existingValue !== null) {
					mergeDiffIntoCache(existingValue, newValue);
				} else {
					target[normalizedKey] = newValue;
				}
			}
			// Handle primitive values
			else {
				target[normalizedKey] = newValue;
			}
		} else {
			target[normalizedKey] = newValue;
		}
	});
}