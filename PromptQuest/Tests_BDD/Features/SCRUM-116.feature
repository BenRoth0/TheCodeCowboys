Feature: Elite Enemy Encounters

	@scrum-116
	Scenario: Elite Enemy Encountered
		Given the user is on the game page
		When I move to the 7th room 
		Then An elite should be spawned

	Scenario: Defeating the elite
		Given I am in the 7th room
		When I defeat the elite
		Then I should be given an elite item