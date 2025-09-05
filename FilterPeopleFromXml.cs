#nullable enable
using System;
using System.Linq;
using System.Xml.Linq;
using System.Text.Json;
using System.Collections.Generic;

public static class PeopleXmlFilter{
    public static string FilterPeopleFromXml(string xmlData)
    {
        var emptyResult = new
        {
            Names = Array.Empty<string>(),
            TotalSalary = 0m,
            AverageSalary = 0m,
            MaxSalary = 0m,
            Count = 0
        };

        if (string.IsNullOrWhiteSpace(xmlData))
            return JsonSerializer.Serialize(emptyResult);

        try
        {
            var doc = XDocument.Parse(xmlData);

            
            var cutoff = new DateTime(2019, 1, 1);

            
            var matches = doc
                .Descendants("Person")
                .Select(p => new
                {
                    Name = (string?)p.Element("Name") ?? "",
                    AgeOk = int.TryParse((string?)p.Element("Age"), out var age) && age > 30,
                    DeptOk = string.Equals((string?)p.Element("Department"), "IT", StringComparison.Ordinal),
                    SalaryParsed = decimal.TryParse((string?)p.Element("Salary"), out var sal) ? sal : (decimal?)null,
                    HireParsed = DateTime.TryParse((string?)p.Element("HireDate"), out var hd) ? hd : (DateTime?)null
                })
                .Where(x =>
                    x.AgeOk &&
                    x.DeptOk &&
                    x.SalaryParsed.HasValue && x.SalaryParsed.Value > 5000m &&
                    x.HireParsed.HasValue && x.HireParsed.Value < cutoff
                )
                .Select(x => new
                {
                    x.Name,
                    Salary = x.SalaryParsed!.Value
                })
                .ToList();

            if (matches.Count == 0)
                return JsonSerializer.Serialize(emptyResult);

            var names = matches
                .Select(m => m.Name)
                .OrderBy(n => n, StringComparer.Ordinal)
                .ToList();

            decimal total = matches.Sum(m => m.Salary);
            int count = matches.Count;
            decimal avg = count > 0 ? Math.Round(total / count, 2) : 0m;
            decimal max = matches.Max(m => m.Salary);

            var result = new
            {
                Names = names,
                TotalSalary = total,
                AverageSalary = avg,
                MaxSalary = max,
                Count = count
            };

            return JsonSerializer.Serialize(result);
        }
        catch
        {
            // Geçersiz XML vb. durumlarda boş sonuç döndür.
            return JsonSerializer.Serialize(emptyResult);
        }
    }
    
}
