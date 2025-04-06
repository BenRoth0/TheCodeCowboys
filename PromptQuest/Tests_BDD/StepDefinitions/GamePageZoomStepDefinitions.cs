using System;
using Reqnroll;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Drawing;
using NUnit.Framework;

namespace Tests_BDD.StepDefinitions {
	[Binding]
	public class GamePageZoomStepDefinitions {
		private IWebDriver webDriver;
		private IWebElement _playerDisplay, _enemyDisplay, _dialogBox, _actionButton;
		private Size _playerSizeBefore, _enemySizeBefore, _dialogBoxSizeBefore, _actionButtonSizeBefore;

		[BeforeScenario]
		public void Setup() {
			// Initialize WebDriver before each scenario
			webDriver = new ChromeDriver();
			webDriver.Manage().Window.Maximize(); //maximize the window, otherwise small zoom amounts would run into max dimensions immediately
		}

		#region Zoom In
		[Given("I am on the game page")]
		public void GivenIAmOnTheGamePage() {
			// Start a new game
			PromptQuestTestMethods.StartNewGame(webDriver,skipTutorial: true);
			// Make sure we're on the game page
			if(!webDriver.Url.Contains("/Game")) {
				throw new Exception("Not on the game page.");
			}

			// Store initial sizes of elements
			_playerDisplay = webDriver.FindElement(By.Id("player-display"));
			_enemyDisplay = webDriver.FindElement(By.Id("enemy-display"));
			_dialogBox = webDriver.FindElement(By.ClassName("dialog-box"));  // No ID for this element, so using className
			_actionButton = webDriver.FindElement(By.Id("action-button-display"));

			_playerSizeBefore = _playerDisplay.Size;
			_enemySizeBefore = _enemyDisplay.Size;
			_dialogBoxSizeBefore = _dialogBox.Size;
			_actionButtonSizeBefore = _actionButton.Size;
		}

		[When("I zoom in my browser")]
		public void WhenIZoomInMyBrowser() {
			//This feature incorporates rem units so increasing the root font size to simulate zoom is good enough.
			((IJavaScriptExecutor)webDriver).ExecuteScript("document.documentElement.style.fontSize = '120%';");
		}

		[Then("the player display will increase in size")]
		public void ThenThePlayerDisplayShouldIncreaseInSize() {
			var sizeAfter = _playerDisplay.Size;
			if(sizeAfter.Width <= _playerSizeBefore.Width || sizeAfter.Height <= _playerSizeBefore.Height) {
				throw new Exception("Player display did not increase in size.");
			}
		}

		[Then("the enemy display will increase in size")]
		public void ThenTheEnemyDisplayShouldIncreaseInSize() {
			var sizeAfter = _enemyDisplay.Size;
			if(sizeAfter.Width <= _enemySizeBefore.Width || sizeAfter.Height <= _enemySizeBefore.Height) {
				throw new Exception("Enemy display did not increase in size.");
			}
		}

		[Then("the dialog box will increase in size")]
		public void ThenTheDialogBoxWillIncreaseInSize() {
			var sizeAfter = _dialogBox.Size;
			if(sizeAfter.Width <= _dialogBoxSizeBefore.Width || sizeAfter.Height <= _dialogBoxSizeBefore.Height) {
				throw new Exception("Dialog box did not increase in size.");
			}
		}

		[Then("the action button display will increase in size")]
		public void ThenTheActionButtonDisplayWillIncreaseInSize() {
			var sizeAfter = _actionButton.Size;
			if(sizeAfter.Width <= _actionButtonSizeBefore.Width || sizeAfter.Height <= _actionButtonSizeBefore.Height) {
				throw new Exception("Action button display did not increase in size.");
			}
		}
		#endregion Zoom In - End

		#region Zoom Out
		[When("I zoom out my browser")]
		public void WhenIZoomOutMyBrowser() {
			//This feature incorporates rem units so increasing the root font size to simulate zoom is good enough.
			((IJavaScriptExecutor)webDriver).ExecuteScript("document.documentElement.style.fontSize = '80%';");
		}

		[Then("the player display will decrease in size")]
		public void ThenThePlayerDisplayShouldDecreaseInSize() {
			var sizeAfter = _playerDisplay.Size;
			if(sizeAfter.Width >= _playerSizeBefore.Width || sizeAfter.Height >= _playerSizeBefore.Height) {
				throw new Exception("Player display did not decrease in size.");
			}
		}

		[Then("the enemy display will decrease in size")]
		public void ThenTheEnemyDisplayShouldDecreaseInSize() {
			var sizeAfter = _enemyDisplay.Size;
			if(sizeAfter.Width >= _enemySizeBefore.Width || sizeAfter.Height >= _enemySizeBefore.Height) {
				throw new Exception("Enemy display did not decrease in size.");
			}
		}

		[Then("the dialog box will decrease in size")]
		public void ThenTheDialogBoxWillDecreaseInSize() {
			var sizeAfter = _dialogBox.Size;
			if(sizeAfter.Width >= _dialogBoxSizeBefore.Width || sizeAfter.Height >= _dialogBoxSizeBefore.Height) {
				throw new Exception("Dialog box did not decrease in size.");
			}
		}

		[Then("the action button display will decrease in size")]
		public void ThenTheActionButtonDisplayWillDecreaseInSize() {
			var sizeAfter = _actionButton.Size;
			if(sizeAfter.Width >= _actionButtonSizeBefore.Width || sizeAfter.Height >= _actionButtonSizeBefore.Height) {
				throw new Exception("Action button display did not decrease in size.");
			}
		}
		#endregion Zoom Out - End

		#region Maximum Zoom
		[When("I zoom in my browser an excessive amount")]
		public void WhenIZoomInMyBrowserAnExcessiveAmount() {
			((IJavaScriptExecutor)webDriver).ExecuteScript("document.documentElement.style.fontSize = '500%';"); // Simulate excessive zoom
		}

		[Then("the player display will reach a maximum")]
		public void ThenThePlayerDisplayWillReachAMaximum() {
			var playerDisplay = webDriver.FindElement(By.Id("player-display"));
			var viewportHeight = webDriver.Manage().Window.Size.Height;
			var viewportWidth = webDriver.Manage().Window.Size.Width;
			Assert.IsTrue(playerDisplay.Size.Width <= (0.4 * viewportWidth) && playerDisplay.Size.Height <= (0.5 * viewportHeight),
					"Player display exceeds maximum size!");
		}

		[Then("the enemy display will reach a maximum")]
		public void ThenTheEnemyDisplayWillReachAMaximum() {
			var enemyDisplay = webDriver.FindElement(By.Id("enemy-display"));
			var viewportHeight = webDriver.Manage().Window.Size.Height;
			var viewportWidth = webDriver.Manage().Window.Size.Width;
			Assert.IsTrue(enemyDisplay.Size.Width <= (0.4 * viewportWidth) && enemyDisplay.Size.Height <= (0.5 * viewportHeight),
					"Enemy display exceeds maximum size!");
		}

		[Then("the dialog box will reach a maximum")]
		public void ThenTheDialogBoxWillReachAMaximum() {
			var dialogBox = webDriver.FindElement(By.Id("dialog-box"));
			var viewportHeight = webDriver.Manage().Window.Size.Height;
			var viewportWidth = webDriver.Manage().Window.Size.Width;
			Assert.IsTrue(dialogBox.Size.Width <= (viewportWidth) && dialogBox.Size.Height <= (0.4 * viewportHeight),
					"Dialog box exceeds maximum size!");
		}

		[Then("the action button display will reach a maximum")]
		public void ThenTheActionButtonDisplayWillReachAMaximum() {
			var actionButton = webDriver.FindElement(By.Id("action-button"));
			var viewportHeight = webDriver.Manage().Window.Size.Height;
			var viewportWidth = webDriver.Manage().Window.Size.Width;
			Assert.IsTrue(actionButton.Size.Width <= (0.2 * viewportWidth) && actionButton.Size.Height <= (0.5 * viewportHeight),
					"Action button exceeds maximum size!");
		}
		#endregion Maximum Zoom - End

		#region Minimum Zoom
		[When(@"I zoom out my browser an excessive amount")]
		public void WhenIZoomOutMyBrowserAnExcessiveAmount() {
			((IJavaScriptExecutor)webDriver).ExecuteScript("document.documentElement.style.fontSize = '10%';"); // Simulate excessive zoom out
		}

		[Then(@"the player display will reach a minimum")]
		public void ThenThePlayerDisplayWillReachAMinimum() {
			var playerDisplay = webDriver.FindElement(By.Id("player-display"));
			var viewportHeight = webDriver.Manage().Window.Size.Height;
			var viewportWidth = webDriver.Manage().Window.Size.Width;
			Assert.IsTrue(playerDisplay.Size.Width >= (0.25 * viewportWidth) && playerDisplay.Size.Height >= (0.25 * viewportHeight),
					"Player display is smaller than the minimum size!");
		}

		[Then(@"the enemy display will reach a minimum")]
		public void ThenTheEnemyDisplayWillReachAMinimum() {
			var enemyDisplay = webDriver.FindElement(By.Id("enemy-display"));
			var viewportHeight = webDriver.Manage().Window.Size.Height;
			var viewportWidth = webDriver.Manage().Window.Size.Width;
			Assert.IsTrue(enemyDisplay.Size.Width >= (0.25 * viewportWidth) && enemyDisplay.Size.Height >= (0.25 * viewportHeight),
					"Enemy display is smaller than the minimum size!");
		}

		[Then(@"the dialog box will reach a minimum")]
		public void ThenTheDialogBoxWillReachAMinimum() {
			var dialogBox = webDriver.FindElement(By.Id("dialog-box"));
			var viewportHeight = webDriver.Manage().Window.Size.Height;
			var viewportWidth = webDriver.Manage().Window.Size.Width;
			Assert.IsTrue(dialogBox.Size.Width >= (0.5 * viewportWidth) && dialogBox.Size.Height >= (0.20 * viewportHeight),
					"Dialog box is smaller than the minimum size!");
		}

		[Then(@"the action button display will reach a minimum")]
		public void ThenTheActionButtonDisplayWillReachAMinimum() {
			var actionButton = webDriver.FindElement(By.Id("action-button"));
			var viewportHeight = webDriver.Manage().Window.Size.Height;
			var viewportWidth = webDriver.Manage().Window.Size.Width;
			Assert.IsTrue(actionButton.Size.Width >= (0.1 * viewportWidth) && actionButton.Size.Height >= (0.25 * viewportHeight),
					"Action button is smaller than the minimum size!");
		}
		#endregion Minimum Zoom - End

		[AfterScenario]
		public void TearDown() {
			if(webDriver != null) {
				webDriver.Quit(); // Ensure the browser is closed
				webDriver?.Dispose(); // Clean up unmanaged resources
			}
		}
	}
}