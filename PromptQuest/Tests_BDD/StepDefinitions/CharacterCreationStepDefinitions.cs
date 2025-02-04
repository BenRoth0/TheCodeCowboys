namespace Tests_BDD.StepDefinitions {
	[Binding]
	public sealed class CalculatorStepDefinitions {
		// For additional details on Reqnroll step definitions see https://go.reqnroll.net/doc-stepdef

		[Given("Given that I am a user")]
		public void GivenUser(int number) {
			//TODO: implement arrange (precondition) logic

			throw new PendingStepException();
		}
		
		[Given("I am on the character creation page")]
		public void GivenCharacterCreationPage(int number) {
			//TODO: implement arrange (precondition) logic
			// For storing and retrieving scenario-specific data see https://go.reqnroll.net/doc-sharingdata
			// To use the multiline text or the table argument of the scenario,
			// additional string/Table parameters can be defined on the step definition
			// method. 

			throw new PendingStepException();
		}

		[When("I click the New Adventure button")]
		public void WhenSubmitClicked() {
			//TODO: implement act (action) logic

			throw new PendingStepException();
		}

		[Then("the form will be submitted")]
		public void ThenFormSubmitted(int result) {
			//TODO: implement assert (verification) logic

			throw new PendingStepException();
		}

		[Then("the form will be reset")]
		public void ThenFormReset(int result) {
			//TODO: implement assert (verification) logic

			throw new PendingStepException();
		}
	}
}
