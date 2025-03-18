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
	defaultItem1 = new Item("Helmet", 0, 2, "/images/PlaceholderItem1.png");
	defaultItem2 = new Item("Fiery Sword", 4, 0, "/images/PlaceholderItem2.png");
	defaultItem3 = new Item("Frozen Shield", 1, 3, "/images/PlaceholderItem3.png");
	defaultItem4 = new Item("Warded Sword", 3, 2, "/images/PlaceholderItem4.png");
	defaultItems = [defaultItem1, defaultItem2, defaultItem3, defaultItem4];
	for (i = 0; i < defaultItems.length; i++) {
		item = defaultItems[i];
		image = document.createElement("img");
		// image.className = "item";
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
}