using Microsoft.VisualStudio.TestTools.UnitTesting;
using DocumentsJournal.Models;
using System;

namespace DocumentsJournal.Tests
{
    [TestClass]
    public class DocumentTests
    {
        [TestMethod]
        public void DateReceivedFormatted_ReturnsCorrectFormat()
        {
            var doc = new Document { DateReceived = new DateTime(2026, 4, 21) };
            Assert.AreEqual("21.04.2026", doc.DateReceivedFormatted);
        }

        [TestMethod]
        public void ExecutionDeadlineFormatted_WhenNull_ReturnsNotSpecified()
        {
            var doc = new Document { ExecutionDeadline = null };
            Assert.AreEqual("Не указан", doc.ExecutionDeadlineFormatted);
        }

        [TestMethod]
        public void ExecutionDeadlineFormatted_WhenHasValue_ReturnsFormattedDate()
        {
            var doc = new Document { ExecutionDeadline = new DateTime(2026, 5, 1) };
            Assert.AreEqual("01.05.2026", doc.ExecutionDeadlineFormatted);
        }

        [TestMethod]
        public void Properties_SetAndGet_WorkCorrectly()
        {
            var doc = new Document
            {
                Id = 1,
                IncomingNumber = "001",
                Sender = "ООО Тест",
                Summary = "Содержание",
                Executor = "Иванов",
                Status = "В работе"
            };

            Assert.AreEqual(1, doc.Id);
            Assert.AreEqual("001", doc.IncomingNumber);
            Assert.AreEqual("ООО Тест", doc.Sender);
            Assert.AreEqual("Содержание", doc.Summary);
            Assert.AreEqual("Иванов", doc.Executor);
            Assert.AreEqual("В работе", doc.Status);
        }

        [TestMethod]
        public void DefaultValues_AreCorrect()
        {
            var doc = new Document();
            Assert.AreEqual(1, doc.NumberOfSheets);
            Assert.AreEqual(1, doc.NumberOfCopies);
            Assert.IsTrue(doc.IsControlled);
        }

        [TestMethod]
        public void DateReceived_CanBeSet()
        {
            var doc = new Document();
            var expected = new DateTime(2026, 4, 21);
            doc.DateReceived = expected;
            Assert.AreEqual(expected, doc.DateReceived);
        }

        [TestMethod]
        public void ExecutionDeadline_CanBeSetToNull()
        {
            var doc = new Document();
            doc.ExecutionDeadline = null;
            Assert.IsNull(doc.ExecutionDeadline);
        }

        [TestMethod]
        public void DocumentType_CanBeSet()
        {
            var doc = new Document { DocumentType = "Письмо" };
            Assert.AreEqual("Письмо", doc.DocumentType);
        }

        [TestMethod]
        public void Notes_CanBeSet()
        {
            var doc = new Document { Notes = "Тестовое примечание" };
            Assert.AreEqual("Тестовое примечание", doc.Notes);
        }

        [TestMethod]
        public void Resolution_CanBeSet()
        {
            var doc = new Document { Resolution = "Утвердить" };
            Assert.AreEqual("Утвердить", doc.Resolution);
        }

        [TestMethod]
        public void SenderAddress_CanBeSet()
        {
            var doc = new Document { SenderAddress = "г. Москва, ул. Ленина, 1" };
            Assert.AreEqual("г. Москва, ул. Ленина, 1", doc.SenderAddress);
        }

        [TestMethod]
        public void DocumentNumber_CanBeSet()
        {
            var doc = new Document { DocumentNumber = "123/ИСХ" };
            Assert.AreEqual("123/ИСХ", doc.DocumentNumber);
        }

        [TestMethod]
        public void IncomingNumber_Empty_IsValidCheck()
        {
            var doc = new Document { IncomingNumber = "" };
            Assert.IsTrue(string.IsNullOrWhiteSpace(doc.IncomingNumber));
        }

        [TestMethod]
        public void Sender_Empty_IsValidCheck()
        {
            var doc = new Document { Sender = "" };
            Assert.IsTrue(string.IsNullOrWhiteSpace(doc.Sender));
        }

        [TestMethod]
        public void Summary_Empty_IsValidCheck()
        {
            var doc = new Document { Summary = "" };
            Assert.IsTrue(string.IsNullOrWhiteSpace(doc.Summary));
        }

        [TestMethod]
        public void IsControlled_Default_IsTrue()
        {
            var doc = new Document();
            Assert.IsTrue(doc.IsControlled);
        }

        [TestMethod]
        public void IsControlled_CanBeSetToFalse()
        {
            var doc = new Document { IsControlled = false };
            Assert.IsFalse(doc.IsControlled);
        }
    }
}