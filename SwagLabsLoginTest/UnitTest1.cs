using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;


namespace SwagLabsLoginTest
{
    public class Tests
    {
        private IWebDriver _driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            _driver = new EdgeDriver();
            _driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            _driver.Manage().Window.Maximize(); 
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));    
        }

        [Test]
        public void SuccessfulLoginTest()
        {
            //Step 1: Find the username and password fields 
            var UserNameField = _driver.FindElement(By.Id("user-name"));
            var PasswordField = _driver.FindElement(By.Id("password"));

            //Step 2: Enter valid credentials
            UserNameField.SendKeys("standard_user");
            PasswordField.SendKeys("secret_sauce");

            //Step 3: Click the login button
            var LoginButton = _driver.FindElement(By.Id("login-button"));
            LoginButton.Click();    

            //Step 3: Asertion to check if the login was successful
            string expectedUrl = "https://www.saucedemo.com/inventory.html";
            Assert.That(_driver.Url, Is.EqualTo(expectedUrl));
        }
        [Test]
        public void UnsuccessfulLoginTest() 
        { 
            //Step 1: Find the username and password fields
            var UserNameField = _driver.FindElement(By.Id("user-name"));
            var PasswordField = _driver.FindElement(By.Id("password"));

            //Step 2: Enter invalid credentials
            UserNameField.SendKeys("invalid_user");
            PasswordField.SendKeys("invalid_password");

            //Step 3: Click the login button
            var LoginButton = _driver.FindElement(By.Id("login-button"));
            LoginButton.Click();

            //Step 4: Wait for the error message to appear
            //IWebElement errorMessage = wait.Until(ExpectedConditions.
            //ElementIsVisible(By.ClassName("error-message-container")));
            IWebElement errorMessage = wait.Until(driver => driver.FindElement(By.ClassName("error-message-container")));
            

            //Step 4: Assertion to check if the login failed
            string expectedText = "Epic sadface: Username and password do not match any user in this service";
            Assert.That(expectedText, Is.EqualTo(errorMessage.Text));
        }
        [Test]
        public void CanRetryLoginAfterErrorMessageDismissal_WithValidCredentials()
        {
            //Step 1: Find the username and password fields
            var UserNameField = _driver.FindElement(By.Id("user-name"));
            var PasswordField = _driver.FindElement(By.Id("password"));

            //Step 2: Enter invalid credentials
            UserNameField.SendKeys("invalid_user");
            PasswordField.SendKeys("invalid_password");

            //Step 3: Click the login button
            var LoginButton = _driver.FindElement(By.Id("login-button"));
            LoginButton.Click();

            //Step 4: Wait for the error message to be visible and close it 
            //IWebElement closeButton = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("error-button")));
            IWebElement closeButton = wait.Until(driver => driver.FindElement(By.ClassName("error-button")));
            closeButton.Click();

            //Step 5: Enter valid credentials
            UserNameField.Clear();
            UserNameField.SendKeys("standard_user");
            PasswordField.Clear();
            PasswordField.SendKeys("secret_sauce");

            //Step 6: Click the login button again
            LoginButton.Click();

            //Step 7: Assertion to check if the login was successful
       
            string expectedUrl = "https://www.saucedemo.com/inventory.html";
            Assert.That(_driver.Url, Is.EqualTo(expectedUrl));
        }
        [TearDown]
        public void TearDown()
        {
            _driver.Quit();

        }
    }
}