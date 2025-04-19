//----------- CACHED ELEMENTS ---------------------------------------------------------------------------------------
//Commonly hidden/shown displays
let playerDisplay;
let enemyDisplay;
let actionButtonDisplay;
let backgroundImage;
let campsiteButtonDisplay;
let eventButtonDisplay;
//Player action buttons
let attackBtn;
let healBtn;
let restBtn;
let skipRestBtn;
let acceptBtn;
let denyBtn;

//----------- ADD EVENT LISTENERS ---------------------------------------------------------------------------------------

document.addEventListener("DOMContentLoaded", () => {
	//Grab the commonly used UI elements on load.
	playerDisplay = document.getElementById("player-display");
	enemyDisplay = document.getElementById("enemy-display");
	actionButtonDisplay = document.getElementById("action-button-display");
	backgroundImage = document.getElementById("bg-image");
	campsiteButtonDisplay = document.getElementById("campsite-button-display");
	eventButtonDisplay = document.getElementById("event-button-display");
	//Grab all the buttons from the DOM on load.
	attackBtn = document.getElementById("attack-btn");
	healBtn = document.getElementById("health-potion-btn");
	restBtn = document.getElementById("rest-btn");
	skipRestBtn = document.getElementById("skip-rest-btn");
	acceptBtn = document.getElementById("accept-btn");
	denyBtn = document.getElementById("deny-btn");
	//Add all event listeners on load
	attackBtn.attachPlayerAction('attack');
	healBtn.attachPlayerAction('heal');
	restBtn.attachPlayerAction('rest');
	skipRestBtn.attachPlayerAction('skip-rest');
	acceptBtn.attachPlayerAction('accept');
	denyBtn.attachPlayerAction('deny');
});

//----------- REFRESH DISPLAY ---------------------------------------------------------------------------------------

function refreshDisplay() {
	// Refresh all dynamic displays
	refreshPlayerDisplay();
	refreshEnemyDisplay();
	refreshInventory();
	refreshMap();
	refreshDialogBox();
	//Sync button states (disabled/enabled).
	attackBtn.syncButtonState(gameState.inCombat && gameState.isPlayersTurn);
	healBtn.syncButtonState(gameState.inCombat && gameState.isPlayersTurn);
	restBtn.syncButtonState(gameState.inCampsite && !gameState.isLocationComplete);
	skipRestBtn.syncButtonState(gameState.inCampsite && !gameState.isLocationComplete);
	acceptBtn.syncButtonState(gameState.inEvent && !gameState.isLocationComplete);
	denyBtn.syncButtonState(gameState.inEvent && !gameState.isLocationComplete);
	//Sync UI visibility (visible/hidden).
	playerDisplay.syncVisibility(gameState.player.currentHealth > 0);
	enemyDisplay.syncVisibility(gameState.inCombat && gameState.enemy.currentHealth > 0);
	actionButtonDisplay.syncVisibility(gameState.inCombat);
	backgroundImage.syncVisibility(gameState.inCampsite);
	campsiteButtonDisplay.syncVisibility(gameState.inCampsite);
	eventButtonDisplay.syncVisibility(gameState.inEvent);
	//This will be merged into the sync pattern above at some point.
	hideRespawnModal();
	if (gameState.player.currentHealth <= 0) { 
		showRespawnModal();
	}
}

// This respawn modal code will be refactored soon to fit into the above patterns.

// Function to show the respawn modal
function showRespawnModal() {
	const respawnModal = new bootstrap.Modal(document.getElementById('respawnModal'));
	respawnModal.show();
}

// Function to hide the respawn modal
function hideRespawnModal() {
	const respawnModalElement = document.getElementById('respawnModal');
	const modalInstance = bootstrap.Modal.getInstance(respawnModalElement);
	if (modalInstance) {
		modalInstance.hide();
	}
}

// Function to respawn the player
async function respawnPlayer() {
	await sendPostRequest("/Game/Respawn");
}

// ------------------------ REFRESH DISPLAY HELPER METHODS ------------------------------------------------------------------------------------------------------

function refreshDialogBox() {
	gameState.listMessages.forEach((message) => {
		const dialogBox = document.querySelector(".dialog-box");
		dialogBox.textContent += message = '\n';
	});
}

function refreshPlayerDisplay() {
	document.querySelectorAll(".player-name").forEach(el => { el.textContent = gameState.player.name; });
	document.querySelectorAll(".player-image").forEach(el => { el.src = "/images/" + gameState.player.class + ".png"; }); // Placeholder image for now.
	document.querySelectorAll(".player-image").forEach(el => { el.alt = gameState.player.name; });
	const equippedItem = gameState.player.itemEquipped
	document.querySelectorAll(".player-attack").forEach(el => { el.textContent = gameState.player.attack + equippedItem?.attack ?? 0; });
	document.querySelectorAll(".player-defense").forEach(el => { el.textContent = gameState.player.defense + equippedItem?.defense ?? 0; });
	document.querySelectorAll(".player-hp").forEach(el => { el.textContent = gameState.player.currentHealth + "/" + gameState.player.maxHealth + " HP"; });
	document.getElementById("player-health-potions").textContent = gameState.player.healthPotions;
}

function refreshEnemyDisplay() {
	document.getElementById("enemy-name").textContent = gameState.enemy.name;
	document.getElementById("enemy-image").src = gameState.enemy.imageUrl;
	document.getElementById("enemy-image").alt = gameState.enemy.name;
	document.getElementById("enemy-attack").textContent = gameState.enemy.attack;
	document.getElementById("enemy-defense").textContent = gameState.enemy.defense;
	document.getElementById("enemy-hp").textContent = gameState.enemy.currentHealth + "/" + gameState.enemy.maxHealth + " HP";
}

function refreshInventory() {
	const items = gameState.player.items;
	// Clear existing inventory slots
	for (let i = 1; i <= 20; i++) {
		const slot = document.getElementById("inventory-slot-" + i);
		while (slot.firstChild) {
			slot.removeChild(slot.firstChild);
		}
	}
	for (let i = 0; i < items.length; i += 1) {
		//Create img tag and insert it into the slot
		const image = document.createElement("img");
		image.src = items[i].imageSrc;
		image.alt = items[i].name;
		const slot = document.getElementById("inventory-slot-" + (i + 1));
		slot.appendChild(image);
		//Set up select behavior for the slot
		image.addEventListener("click", () => {
			selectItem(items[i], i);
		});
	}
	// Fill Equipped item slot
	const equippedItem = gameState.player.itemEquipped;
	//Clear out equipped item slot
	const equippedItemSlot = document.getElementById("equipped-item");
	while (equippedItemSlot.firstChild) {
		equippedItemSlot.removeChild(equippedItemSlot.firstChild);
	}
	if (equippedItem != null) {
		const image = document.createElement("img");
		image.src = equippedItem.imageSrc;
		image.alt = equippedItem.name;
		equippedItemSlot.appendChild(image);
		//Set up the select behavior
		image.addEventListener("click", () => {
			selectItem(equippedItem, -1);
		});
	}
	// Set up the equip button
	const equipButton = document.getElementById("equip-button");
	equipButton.removeEventListener("click", equipItem);
	equipButton.addEventListener("click", equipItem);
}

function refreshMap() {
	const mapContainer = document.getElementById("map-container");
	mapContainer.innerHTML = ""; // Clear previous map
	// Updates the floor counter
	const floorTracker = document.getElementById("floor-tracker");
	floorTracker.textContent = "Floor " + gameState.floor;
	// Adds a check for if the player has defeated the boss to show next floor button
	if (gameState.playerLocation == 10 && gameState.isLocationComplete) {
		nextFloorButton.style.visibility = "visible";
		nextFloorButton.attachPlayerAction('move');
	} else {
		nextFloorButton.style.visibility = "hidden";
	}
	// Draw the map nodes and edges
	for (let i = 0; i < map.listMapNodes.length; i++) {
		// node.removeEventListener("click", movePlayerToNode());
		const nodeElement = document.createElement("button");
		nodeElement.className = "map-node";
		nodeElement.setAttribute("data-node-id", map.listMapNodes[i].mapNodeId);
		mapContainer.appendChild(nodeElement);
		// Check for NodeType and add image if it is "Boss"
		if (map.listMapNodes[i].nodeType === "Boss") {
			const imgElement = document.createElement("img");
			imgElement.src = "/images/boss.png";
			imgElement.className = "map-image";
			nodeElement.appendChild(imgElement);
		}
		// Check for NodeType and add image if it is "Campsite"
		if (map.listMapNodes[i].nodeType === "Campsite") {
			const imgElement = document.createElement("img");
			imgElement.src = "/images/campsite.png";
			imgElement.className = "map-image";
			nodeElement.appendChild(imgElement);
		}
		// Check for NodeType and add image if it is "Event"
		if (map.listMapNodes[i].nodeType === "Event") {
			const imgElement = document.createElement("img");
			imgElement.src = "/images/event.png";
			imgElement.className = "map-image";
			nodeElement.appendChild(imgElement);
		}
		// Show player which node they are on
		if (map.listMapNodes[i].mapNodeId == gameState.playerLocation) {
			nodeElement.classList.add("map-node-current");
		}
		if (map.listMapNodes[i].mapNodeId < gameState.playerLocation) {
			nodeElement.classList.add("map-node-completed");
		}
		// Enable the next node after the current node
		if (map.listMapNodes[i].mapNodeId == gameState.playerLocation + 1 && gameState.isLocationComplete) {
			nodeElement.classList.add("map-node-enabled");
			// Add event listener for node click on enabled nodes
			nodeElement.attachPlayerAction('move');
		}
		else {
			nodeElement.classList.add("map-node-disabled");
		}
		// Don't create a map edge for the last node
		if (i == map.listMapEdges.length) {
			continue;
		}
		// Create a map edge between nodes
		const nodeEdgeElement = document.createElement("div");
		nodeEdgeElement.className = "map-edge";
		mapContainer.appendChild(nodeEdgeElement);
	}
}

//------------------------ OVERLOADS --------------------------------------------------------------------------------------------------------------

//Shows/Hides an html element according to the given condition. Only updates if necessary to avoid UI flicker.
HTMLElement.prototype.syncVisibility = function (condition) {
	if (condition && this.style.display === "none") {
		//Element should be visible but is currently hidden. Show it.
		this.style.display = "";
	}
	if (!condition && this.style.display !== "none") {
		//Element should be hidden but is currently visible. Hide it.
		this.style.display = "none";
	}
};

//Attaches a player action to a button as an eventlistener. Does not need to be removed because event will only fire if button is enabled.
HTMLButtonElement.prototype.attachPlayerAction = function (action) {
	this.addEventListener("click", async () => {
		if (this.disabled) {
			return; //Button is disabled, so let's bounce.
		}
		await executePlayerAction(action);
	});
};

//Enables/disables a button according to the given condition. Only updates if necessary to avoid UI flicker.
HTMLButtonElement.prototype.syncButtonState = function (condition) {
	if (condition && this.disabled) {
		//Button should be enabled but is currently disabled. Enable it.
		this.disabled = false;
	}
	if (!condition && this.disabled == false) {
		//Button should not be enabled but is currently enabled. Disable it.
		this.disabled = true;
	}
};

