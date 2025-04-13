Feature: Inventory

@scrum-98
Scenario: Display item details when an item is clicked
	Given I am on the inventory tab
	When I click on an item in the inventory
	Then I should see a window with that item's title, image, and stats

@scrum-99
Scenario: Equiping items
	Given I am on the inventory tab
	When I click on an item
	And I click the equip button
	Then that item will move to the equipped item slot

@scrum-99
Scenario: Trying to equip no item
	Given I am on the inventory tab
	When I click the equip button
	And I don't have an item selected
	Then nothing should happen

@scrum-99
Scenario: Equipped item is saved for logged in users
	Given I am an authenticated user
	And I am on the inventory tab
	And I have an item equipped
	When I close the game
	And continue the game
	Then the item is still equipped

@scrum-99
Scenario: inventory is saved for logged in users
	Given I am an authenticated user
	And I am on the inventory tab
	And I have items in my inventory
	When I close the game
	And continue the game
	Then I will have the same items in my inventory
