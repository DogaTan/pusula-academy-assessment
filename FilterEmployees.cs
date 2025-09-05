#nullable enable
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

public static class EmployeeFilter{
    public static string FilterEmployees(IEnumerable<(string Name, int Age, string Department, decimal Salary, DateTime HireDate)> employees)
    {
        if (employees is null)
        {
            return JsonSerializer.Serialize(new
            {
                Names = Array.Empty<string>(),
                TotalSalary = 0m,
                AverageSalary = 0m,
                MinSalary = 0m,
                MaxSalary = 0m,
                Count = 0
            });
        }

        // Örnek 5'te (2017-12-31) dahil edildiği için eşik bu gündür.
        var cutoff = new DateTime(2017, 12, 31);

        var filtered = employees
            .Where(e =>
                   e.Age >= 25 && e.Age <= 40 &&
                   (string.Equals(e.Department, "IT", StringComparison.Ordinal) ||
                    string.Equals(e.Department, "Finance", StringComparison.Ordinal)) &&
                   e.Salary >= 5000m && e.Salary <= 9000m &&
                   e.HireDate >= cutoff)
            .ToList();

        if (filtered.Count == 0)
        {
            return JsonSerializer.Serialize(new
            {
                Names = Array.Empty<string>(),
                TotalSalary = 0m,
                AverageSalary = 0m,
                MinSalary = 0m,
                MaxSalary = 0m,
                Count = 0
            });
        }

        // Sıralama: uzunluğa göre azalan, sonra alfabetik artan
        var orderedNames = filtered
            .Select(e => e.Name ?? string.Empty)
            .OrderByDescending(n => n.Length)
            .ThenBy(n => n, StringComparer.Ordinal)
            .ToList();

        decimal total = filtered.Sum(e => e.Salary);
        int count = filtered.Count;
        decimal avg = Math.Round(total / count, 2);
        decimal min = filtered.Min(e => e.Salary);
        decimal max = filtered.Max(e => e.Salary);

        var result = new
        {
            Names = orderedNames,
            TotalSalary = total,
            AverageSalary = avg,
            MinSalary = min,
            MaxSalary = max,
            Count = count
        };

        return JsonSerializer.Serialize(result);
    }
    
}