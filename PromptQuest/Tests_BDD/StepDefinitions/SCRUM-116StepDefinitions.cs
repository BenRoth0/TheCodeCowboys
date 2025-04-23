using System;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Reqnroll;
using System.Runtime.InteropServices;
using NUnit.Framework;
namespace Tests_BDD.StepDefinitions {
	[Binding]
	public class SCRUM116StepDefinitions {
		private IWebDriver webDriver;

		[BeforeScenario]
		public void Setup() {
			// Initialize WebDriver before each scenario
			webDriver = new ChromeDriver();
			webDriver.Manage().Window.Maximize();
			// Start a new game
			PromptQuestTestMethods.StartNewGame(webDriver, skipTutorial: true);
		}
		[Then("An elite should be spawned")]
		public void ThenAnEliteShouldBeSpawned() {
			// Close the menu
			IWebElement closeButton = webDriver.FindElement(By.Id("pq-modal-close"));
			closeButton.Click();
			// Wait for the boss to spawn
			PromptQuestTestMethods.WaitForElementToLoad(webDriver, "attack-btn");
			// Search for the boss' name Dark Orc Warlock
			IWebElement bossName = webDriver.FindElement(By.Id("enemy-name"));
			// Assert that the boss name displayed is Dark Orc Warlock
			Assert.IsTrue(bossName.Text == "Spectral Orc Berserker", "The elite name is incorrect.");
		}

		[Given("I am in the {int}th room")]
		public void GivenIAmInTheThRoom(int p0) {
			PromptQuestTestMethods.MoveToRoom(webDriver, p0);
		}

		[When("I defeat the elite")]
		public void WhenIDefeatTheElite() {
			PromptQuestTestMethods.ClearRoom(webDriver);
		}

		[Then("I should be given an elite item")]
		public void ThenIShouldBeGivenAnEliteItem() {
			throw new PendingStepException();
		}
		[AfterScenario]
		public void TearDown() {
			if (webDriver != null) {
				webDriver.Quit(); // Ensure the browser is closed
				webDriver?.Dispose(); // Clean up unmanaged resources
			}
		}
	}
}