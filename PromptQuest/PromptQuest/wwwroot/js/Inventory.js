// "Global" Variables for the selected and equipped items
let selectedItemIndex = 0;
let equippedItemId = 0;
function UpdateInventoryDisplay() {
		// Fill Equipped item slot
		const equippedItem = gameState.player.items.find(i => i.itemId === gameState.player.itemEquippedIndex); // Grab the equipped item
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
				selectItem(equippedItem);
			});
		}
		// Set up the equip button
		const equipButton = document.getElementById("equip-button");
		equipButton.removeEventListener("click", equipItem);
		equipButton.addEventListener("click", equipItem);
		// Fill unequipped inventory slots
		// Clear existing inventory slots
		for (let i = 1; i <= 20; i++) {
			const slot = document.getElementById("inventory-slot-" + i);
			while (slot.firstChild) {
				slot.removeChild(slot.firstChild);
			}
		}
		const items = gameState.player.items; // Grab the players items
		let slotCount = 1;
		// Populate inventory slots with items from the server
		items.forEach((item, index) => {
			//If item is equipped don't put it into the inventory
			if (item.itemId == equippedItem?.itemId??0) {
				return; //bail
			}
			//Create img tag and insert it into the slot
			const image = document.createElement("img");
			image.src = item.imageSrc;
			image.alt = item.name;
			const slot = document.getElementById("inventory-slot-" + (index + 1));
			slot.appendChild(image);
			//Store the items Id in the slot
			slot.setAttribute("data-item-id", item.itemId);
			//Set up select behavior for the slot
			image.addEventListener("click", () => {
				selectItem(item);
			});
		});
	}

async function equipItem() {
	const equippedItem = gameState.player.items.find(i => i.itemId === gameState.player.itemIdEquipped); // Grab the equipped item
	// If the equipped item is the selected item, or there is no selected item then do nothing.
	if (selectedItemIndex === equippedItem.itemId || selectedItemIndex == 0) {
		return;
	}
	await $.ajax({
		url: '/Game/EquipItem',
		type: 'POST',
		data: { itemIndex: selectedItemIndex },
		success: function (response) {
			loadGame(); //Refresh the local gamestate and all displays
		},
		error: function (xhr, status, error) {
			console.error('Error equipping item: ', error);
		}
	});
}

// Function to select an item and display its stats
function selectItem(item) {
	//Save the items Id so we can use it later
	selectedItemIndex = item.itemId;
	// Display item stats
	document.getElementById("item-name").textContent = item.name;
	document.getElementById("item-attack").textContent = item.attack;
	document.getElementById("item-defense").textContent = item.defense;
	document.getElementById("item-image").src = item.imageSrc;
	// Show the elements
	document.getElementById("item-name").style.display = "block";
	document.getElementById("item-attack").style.display = "block";
	document.getElementById("item-defense").style.display = "block";
	document.getElementById("item-image").style.display = "block";
	document.getElementById("shield-icon").style.display = "block";
	document.getElementById("sword-icon").style.display = "block";
	//Remove the highlight on any other item slots
	for (let i = 1; i <= 20; i++) {
		document.getElementById("inventory-slot-" + i).style.borderColor = "";
	}
	document.getElementById("equipped-item").style.borderColor = "";
	// Highlight the selected inventory slot
	const selectedSlot = document.querySelector(`[data-item-id="${selectedItemIndex}"]`);
	if (selectedSlot) selectedSlot.style.borderColor = "#ffdc4a";
}