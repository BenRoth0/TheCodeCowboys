//----------------------------------------------------------------------- Item Creation -----------------------------------------------------------------------
// This function will produce an item object when called
function Item(name, attack, defense, image) {
	this.name = name;
	this.attack = attack;
	this.defense = defense;
	this.image = image;
}
// This function will call the Item function to create new items and add them to the defaultItems array for testing
function LoadItems() {
	defaultItem1 = new Item("Jeweled Helmet", 0, 2, "/images/PlaceholderItem1.png");
	defaultItem2 = new Item("Fiery Sword", 4, 0, "/images/PlaceholderItem2.png");
	defaultItem3 = new Item("Frozen Shield", 1, 3, "/images/PlaceholderItem3.png");
	defaultItem4 = new Item("Warded Sword", 3, 2, "/images/PlaceholderItem4.png");
	defaultItems = [defaultItem1, defaultItem2, defaultItem3, defaultItem4];
	// Clear existing items in inventory slots
	for (let i = 1; i <= 20; i++) {
		let slot = document.getElementById("inventory-slot-" + i);
		while (slot.firstChild) {
			slot.removeChild(slot.firstChild);
		}
	}
	// Add items to inventory slots
	for (i = 0; i < defaultItems.length; i++) {
		item = defaultItems[i];
		image = document.createElement("img");
		image.src = item.image;
		image.alt = item.name;
		document.getElementById("inventory-slot-" + (i + 1)).appendChild(image);
		image.addEventListener("click", (function (item) {			// This function will create a closure to store the item (IIFE)
			return function () {
				// Display item stats
				selectItem(item);
			};
		})(item));
	}
}
function selectItem(item) {
	// Display item stats
	document.getElementById("item-name").innerHTML = item.name;
	document.getElementById("item-attack").innerHTML = item.attack;
	document.getElementById("item-defense").innerHTML = item.defense;
	document.getElementById("item-image").src = item.image;
	// Show the elements
	document.getElementById("item-name").style.display = "block";
	document.getElementById("item-attack").style.display = "block";
	document.getElementById("item-defense").style.display = "block";
	document.getElementById("item-image").style.display = "block";
	document.getElementById("shield-icon").style.display = "block";
	document.getElementById("sword-icon").style.display = "block";
}