// Libraries/DocumentValidator.cs
using DocumentsJournal.Models;
using System.Collections.Generic;

public class DocumentValidator
{
    public static bool IsValid(Document doc)
    {
        return !string.IsNullOrWhiteSpace(doc.IncomingNumber) &&
               !string.IsNullOrWhiteSpace(doc.Sender) &&
               !string.IsNullOrWhiteSpace(doc.Summary);
    }

    public static string GetErrors(Document doc)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(doc.IncomingNumber))
            errors.Add("Входящий номер обязателен");
        if (string.IsNullOrWhiteSpace(doc.Sender))
            errors.Add("Отправитель обязателен");
        if (string.IsNullOrWhiteSpace(doc.Summary))
            errors.Add("Содержание обязательно");
        return string.Join(", ", errors);
    }
}