using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lecture1.Tests
{
    [TestClass]
    public class PersonTests
    {
        readonly Person Person = new();
        string UserName = "Inigo.Montoya";
        string Password = "Secret";
        
        [TestInitialize]
        public void Initialize()
        {
            UserName = "Inigo.Montoya";
            Password = "YouKilledMyF@ther";
        }

        [TestMethod]
        //what_conditions_results
        public void Login_GivenValidUserNameAndPassword_Success()
        {

            bool success = Person.Login(UserName, Password);

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void Login_GivenInvalidPassword_Failure()
        {
            string Password = "InvalidPassword";

            bool result = Person.Login(UserName, Password);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Login_GivenInvalidUsername_Failure()
        {
            UserName = "InvalidUserName";

            bool result = Person.Login(UserName, Password);

            Assert.IsFalse(result);
        }

    }
}