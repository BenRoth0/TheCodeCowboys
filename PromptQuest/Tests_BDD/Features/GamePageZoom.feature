Feature: Game page zoom
	As a user I want the game page elements to scale when I zoom in or out for better usability and visibility

Scenario: Zoom In
	Given I am on the game page
	When I zoom in my browser
	Then the player display will increase in size
	And the enemy display will increase in size
	And the dialog box will increase in size
	And the action button display will increase in size

Scenario: Zooming Out
	Given I am on the game page
	When I zoom out my browser
	Then the player display will decrease in size
	And the enemy display will decrease in size
	And the dialog box will decrease in size
	And the action button display will decrease in size

Scenario: Maximum Zoom
	Given I am on the game page
	When I zoom in my browser an excessive amount
	Then the player display will reach a maximum
	And the enemy display will reach a maximum
	And the dialog box will reach a maximum
	And the action button display will reach a maximum

Scenario: Minimum Zoom
	Given I am on the game page
	When I zoom out my browser an excessive amount
	Then the player display will reach a minimum
	And the enemy display will reach a minimum
	And the dialog box will reach a minimum
	And the action button display will reach a minimum