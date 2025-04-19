// "Global" Variables for the selected and equipped items
let selectedItemIndex = -1;

async function equipItem() {
	// If there is no selected item then do nothing.
	if (selectedItemIndex == -1) {
		return;
	}
	await sendPostRequest(`/Game/EquipItem?itemIndex=${selectedItemIndex}`);
}

// Function to select an item and display its stats
function selectItem(item, index) {
	//Save the items Id so we can use it later
	selectedItemIndex = index;
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
	const selectedSlot = document.getElementById("inventory-slot-" + (selectedItemIndex+1));
	if (selectedSlot) selectedSlot.style.borderColor = "#ffdc4a";
}