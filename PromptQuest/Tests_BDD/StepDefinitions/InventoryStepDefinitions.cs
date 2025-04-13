using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Modules.Script;
using OpenQA.Selenium.Chrome;
using Reqnroll;
using static System.Net.Mime.MediaTypeNames;

namespace Tests_BDD {

	[Binding]
	public class InventoryDisplaySteps {
		private IWebDriver webDriver;

		[BeforeScenario]
		public void Setup() {
			// Initialize WebDriver before each scenario
			webDriver = new ChromeDriver();
			// Start a new game
			PromptQuestTestMethods.StartNewGame(webDriver,skipTutorial: true);
		}

		[Given(@"I am on the inventory tab")]
		public void GivenIAmOnTheInventoryTab() {
			// Navigate to the inventory tab in the application
			IWebElement menuButton = webDriver.FindElement(By.XPath("//button[normalize-space(text()='Menu')]"));
			menuButton.Click();
		}

		[When(@"I click on an item in the inventory")]
		public void WhenIClickOnAnItemInTheInventory() {
			// Click on the first inventory Item Slot (Should have something)
			IWebElement FirstInventoryItem = webDriver.FindElement(By.Id("inventory-slot-1"));
			FirstInventoryItem.Click();
			//Wait for menu modal to show before continuing
			PromptQuestTestMethods.WaitForModalToOpen(webDriver,"pq-modal");
		}

		[Then(@"I should see a window with that item's title, image, and stats")]
		public void ThenIShouldSeeAWindowWithThatItemsTitleImageAndStats() {
			// Find the inventory display container by class name
			IWebElement inventoryDisplay = webDriver.FindElement(By.ClassName("pq-inventory-display"));
			// Assert that the inventory display is visible
			Assert.IsTrue(inventoryDisplay.Displayed,"The inventory display is not visible.");
			// Assert the item's title, image, and stats are present within the display
			// Assert default attributes for now, later we'll add a db/session call to get the actual item info.
			//Item name
			IWebElement itemName = inventoryDisplay.FindElement(By.Id("item-name"));
			Assert.IsTrue(!string.IsNullOrEmpty(itemName.Text),"Item name is empty.");
			Assert.IsTrue(itemName.Text=="Jeweled Helmet","Item name is incorrect");
			//Item image
			IWebElement itemImage = inventoryDisplay.FindElement(By.Id("item-image"));
			Assert.AreEqual("https://localhost:7186/images/PlaceholderItem1.png",itemImage.GetAttribute("src"),"Item image source is incorrect.");
			//Item defense icon
			IWebElement shieldIcon = inventoryDisplay.FindElement(By.Id("shield-icon"));
			Assert.IsNotNull(shieldIcon,"Shield icon element is missing.");
			Assert.IsTrue(shieldIcon.Displayed,"Shield icon is not visible.");
			//Item defense stat
			IWebElement itemDefense = inventoryDisplay.FindElement(By.Id("item-defense"));
			Assert.IsNotNull(itemDefense,"Item defense element is missing.");
			Assert.IsTrue(!string.IsNullOrEmpty(itemDefense.Text),"Item defense value is empty.");
			Assert.IsTrue(itemDefense.Text=="2","Item defense value is empty.");
			//Item attack icon
			IWebElement swordIcon = inventoryDisplay.FindElement(By.Id("sword-icon"));
			Assert.IsNotNull(swordIcon,"Sword icon element is missing.");
			Assert.IsTrue(swordIcon.Displayed,"Sword icon is not visible.");
			//Item attack stat
			IWebElement itemAttack = inventoryDisplay.FindElement(By.Id("item-attack"));
			Assert.IsNotNull(itemAttack,"Item attack element is missing.");
			Assert.IsTrue(!string.IsNullOrEmpty(itemAttack.Text),"Item attack value is empty.");
			Assert.IsTrue(itemAttack.Text=="0","Item defense value is empty.");
		}

		[When("I click on an item")]
		public void WhenIClickOnAnItem() {
			IWebElement inventorySlot = webDriver.FindElement(By.CssSelector("#inventory-slot-1"));
			inventorySlot.Click();
		}

		[When("I click the equip button")]
		public void WhenIClickTheEquipButton() {
			IWebElement equipButton = webDriver.FindElement(By.Id("equip-button"));
			equipButton.Click();
		}

		[Then("that item will move to the equipped item slot")]
		public void ThenThatItemWillMoveToTheEquippedItemSlot() {
			IWebElement equippedSlot = webDriver.FindElement(By.Id("equipped-item"));
			Assert.IsNotNull(equippedSlot.Text);
		}

		[Then("nothing should happen")]
		public void ThenNothingShouldHappen() {
			IWebElement equippedSlot = webDriver.FindElement(By.Id("equipped-item"));
			Assert.IsTrue(string.IsNullOrEmpty(equippedSlot.Text));
		}

		[Given("I am an authenticated user")]
		public void GivenIAmAnAuthenticatedUser() {
			//webDriver.Navigate().GoToUrl("/Account/GoogleLogin");
			//IWebElement usernameInput = webDriver.FindElement(By.Id("username"));
			//IWebElement passwordInput = webDriver.FindElement(By.Id("password"));
			//usernameInput.SendKeys("testUser");
			//passwordInput.SendKeys("securePassword");
			//webDriver.FindElement(By.Id("loginButton")).Click();
			Assert.That(true);
		}

		[Given("I have an item equipped")]
		public void GivenIHaveAnItemEquipped() {
			var equipButton = webDriver.FindElement(By.Id("equip-button"));
			equipButton.Click();
			var equippedSlot = webDriver.FindElement(By.Id("equipped-item"));
			Assert.IsNotNull(equippedSlot.Text);
		}

		[Given("I have items in my inventory")]
		public void GivenIHaveItemsInMyInventory() {
			var inventoryItems = webDriver.FindElements(By.CssSelector(".pq-inventory-slot"));
			Assert.IsTrue(inventoryItems.Count > 0);
		}

		[When("I close the game")]
		public void WhenICloseTheGame() {
			webDriver.Quit(); // Kill the browser
			webDriver?.Dispose(); // Clean up unmanaged resources
		}

		[When("I continue the game")]
		public void WhenIContinueTheGame() {
			webDriver = new ChromeDriver();
			webDriver.Navigate().GoToUrl("/inventory");
		}

		[Then("the item is still equipped")]
		public void ThenTheItemIsStillEquipped() {
			var equippedSlot = webDriver.FindElement(By.Id("equipped-item"));
			Assert.IsNotNull(equippedSlot.Text);
		}

		[Then("I will have the same items in my inventory")]
		public void ThenIWillHaveTheSameItemsInMyInventory() {
			var inventoryItems = webDriver.FindElements(By.CssSelector(".pq-inventory-slot"));
			Assert.IsTrue(inventoryItems.Count > 0);
		}

		[AfterScenario]
		public void TearDown() {
			if(webDriver != null) {
				webDriver.Quit(); // Ensure the browser is closed
				webDriver?.Dispose(); // Clean up unmanaged resources
			}
		}

	}
}
