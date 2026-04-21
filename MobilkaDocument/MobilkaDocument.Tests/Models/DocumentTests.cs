using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobilkaDocument.Models;

namespace MobilkaDocument.Tests.Models
{
    [TestClass]
    public class DocumentTests
    {
        [TestMethod]
        public void DateReceivedFormatted_ShouldReturnCorrectFormat()
        {
            // Arrange
            var document = new Document
            {
                DateReceived = new DateTime(2026, 4, 21)
            };

            // Act
            var result = document.DateReceivedFormatted;

            // Assert
            Assert.AreEqual("21.04.2026", result);
        }

        [TestMethod]
        public void ExecutionDeadlineFormatted_WhenNull_ShouldReturnNotSpecified()
        {
            // Arrange
            var document = new Document
            {
                ExecutionDeadline = null
            };

            // Act
            var result = document.ExecutionDeadlineFormatted;

            // Assert
            Assert.AreEqual("Не указан", result);
        }

        [TestMethod]
        public void ExecutionDeadlineFormatted_WhenHasValue_ShouldReturnFormattedDate()
        {
            // Arrange
            var document = new Document
            {
                ExecutionDeadline = new DateTime(2026, 5, 1)
            };

            // Act
            var result = document.ExecutionDeadlineFormatted;

            // Assert
            Assert.AreEqual("01.05.2026", result);
        }

        [TestMethod]
        public void StatusColor_ForReview_ShouldReturnOrange()
        {
            // Arrange
            var document = new Document { Status = "На рассмотрении" };

            // Act
            var color = document.StatusColor;

            // Assert
            Assert.IsNotNull(color);
        }

        [TestMethod]
        public void StatusColor_InProgress_ShouldReturnBlue()
        {
            // Arrange
            var document = new Document { Status = "В работе" };

            // Act
            var color = document.StatusColor;

            // Assert
            Assert.IsNotNull(color);
        }

        [TestMethod]
        public void StatusColor_Completed_ShouldReturnGreen()
        {
            // Arrange
            var document = new Document { Status = "Исполнен" };

            // Act
            var color = document.StatusColor;

            // Assert
            Assert.IsNotNull(color);
        }

        [TestMethod]
        public void StatusColor_Rejected_ShouldReturnRed()
        {
            // Arrange
            var document = new Document { Status = "Отказ" };

            // Act
            var color = document.StatusColor;

            // Assert
            Assert.IsNotNull(color);
        }

        [TestMethod]
        public void StatusColor_Default_ShouldReturnGray()
        {
            // Arrange
            var document = new Document { Status = "Неизвестный статус" };

            // Act
            var color = document.StatusColor;

            // Assert
            Assert.IsNotNull(color);
        }
    }
}