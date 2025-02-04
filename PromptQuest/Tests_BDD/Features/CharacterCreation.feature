Feature: Character Creation

The ability for the player to create a character

@SomeTag
Scenario: Form Submission
	Given that I am a user
	And I am on the character creation page
	When I click the New Adventure button
	Then the form will be submitted and reset
