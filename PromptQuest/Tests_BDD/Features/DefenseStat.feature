Feature: Defense Stat

The stat that reduces the player's damage taken
	@scrum-75
	Scenario: Player Attacked
		Given the user is on the game page
		When the user is attacked by an enemy
		Then the user should receive damage equal to the enemy's attack minus player defense