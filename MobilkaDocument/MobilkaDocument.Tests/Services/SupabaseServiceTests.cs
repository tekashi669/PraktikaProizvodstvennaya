using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobilkaDocument.Models;
using MobilkaDocument.Services;

namespace MobilkaDocument.Tests.Services
{
    [TestClass]
    public class SupabaseServiceTests
    {
        private SupabaseService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new SupabaseService();
        }

        [TestMethod]
        public async Task LoginAsync_InvalidCredentials_ShouldReturnFalse()
        {
            // Arrange
            var email = "wrong@mail.ru";
            var password = "wrongpassword";

            // Act
            var result = await _service.LoginAsync(email, password);

            // Assert
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public async Task GetDocumentsAsync_ShouldReturnList()
        {
            // Act
            var documents = await _service.GetDocumentsAsync();

            // Assert
            Assert.IsNotNull(documents);
        }
    }
}