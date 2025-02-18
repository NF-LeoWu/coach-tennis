using System;
using lib;

public class BudgetService
{
    private readonly IBudgetRepo _budgetRepo;
    public BudgetService(IBudgetRepo budgetRepo)
    {
        _budgetRepo = budgetRepo;
    }
    public decimal Query(DateTime start, DateTime end)
    {
        if (start > end)
        {
            return 0m;
        }
        var budgets = _budgetRepo.GetAll();
        decimal total = 0;
        foreach (var budget in budgets)
        {
            var yearMonth = DateTime.ParseExact(budget.YearMonth, "yyyyMM", null);
            var daysInMonth = DateTime.DaysInMonth(yearMonth.Year, yearMonth.Month);
            var dailyAmount = budget.Amount / daysInMonth;
            var periodStart = new DateTime(yearMonth.Year, yearMonth.Month, 1);
            var periodEnd = periodStart.AddMonths(1).AddDays(-1);
            if (end < periodStart || start > periodEnd)
            {
                continue;
            }
            var effectiveStart = start > periodStart ? start : periodStart;
            var effectiveEnd = end < periodEnd ? end : periodEnd;
            var daysCovered = (effectiveEnd - effectiveStart).Days + 1;
            total += dailyAmount * daysCovered;
        }
        return total;
    }
}