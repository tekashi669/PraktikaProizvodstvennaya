using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobilkaDocument.Models;

namespace MobilkaDocument.Tests.Models
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void User_DefaultValues_ShouldBeEmpty()
        {
            // Arrange & Act
            var user = new User();

            // Assert
            Assert.AreEqual("", user.Email);
            Assert.IsFalse(user.IsAuthenticated);
        }

        [TestMethod]
        public void User_SetEmail_ShouldReturnCorrectValue()
        {
            // Arrange
            var user = new User();

            // Act
            user.Email = "test@mail.ru";

            // Assert
            Assert.AreEqual("test@mail.ru", user.Email);
        }

        [TestMethod]
        public void LoginResult_Success_ShouldReturnTrue()
        {
            // Arrange & Act
            var result = new LoginResult
            {
                Success = true,
                Message = "Вход выполнен"
            };

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Вход выполнен", result.Message);
        }

        [TestMethod]
        public void LoginResult_Failure_ShouldReturnFalse()
        {
            // Arrange & Act
            var result = new LoginResult
            {
                Success = false,
                Message = "Неверный пароль"
            };

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Неверный пароль", result.Message);
        }
    }
}