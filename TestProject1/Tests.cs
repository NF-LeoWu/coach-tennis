using System;
using System.Collections.Generic;
using lib;
using NSubstitute;
using NUnit.Framework;

namespace TestProject1
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void InvalidDateRange()
        {
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            var budgetService = new BudgetService(budgetRepo);
            var actual = budgetService.Query(new DateTime(2025,2,18), new DateTime(2025,2,17));
            Assert.AreEqual(actual, 0m);
        }
        
        [Test]
        public void QueryFullDay()
        {
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget { YearMonth = "202502", Amount = 2800m }, // 2月預算 2800
                new Budget { YearMonth = "202503", Amount = 3100m }  // 3月預算 3100
            });
            
            var budgetService = new BudgetService(budgetRepo);
            var actual = budgetService.Query(new DateTime(2025,2,18), new DateTime(2025,2,18));
            Assert.AreEqual(actual, 100m);
        }
        
        [Test]
        public void QueryFullMonth()
        {
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget { YearMonth = "202502", Amount = 2800m }, // 2月預算 2800
                new Budget { YearMonth = "202503", Amount = 3100m }  // 3月預算 3100
            });
            
            var budgetService = new BudgetService(budgetRepo);
            var actual = budgetService.Query(new DateTime(2025,2,1), new DateTime(2025,2,28));
            Assert.AreEqual(actual, 2800m);
        }
        
        [Test]
        public void QueryCrossMonths()
        {
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget { YearMonth = "202502", Amount = 2800m }, // 2月預算 2800
                new Budget { YearMonth = "202503", Amount = 310m }  // 3月預算 3100
            });
            
            var budgetService = new BudgetService(budgetRepo);
            var actual = budgetService.Query(new DateTime(2025,2,27), new DateTime(2025,3,2));
            Assert.AreEqual(actual, 220m);
        }
        
        
        [Test]
        public void QueryManyDays()
        {
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget { YearMonth = "202502", Amount = 2800m }, // 2月預算 2800
                new Budget { YearMonth = "202503", Amount = 310m }  // 3月預算 3100
            });
            
            var budgetService = new BudgetService(budgetRepo);
            var actual = budgetService.Query(new DateTime(2025,2,1), new DateTime(2025,2,5));
            Assert.AreEqual(actual, 500m);
        }
        
        
        [Test]
        public void QueryIncludeNullMonth()
        {
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget { YearMonth = "202502", Amount = 2800m },
                new Budget { YearMonth = "202504", Amount = 300m }  
            });
            
            var budgetService = new BudgetService(budgetRepo);
            var actual = budgetService.Query(new DateTime(2025,2,1), new DateTime(2025,4,30));
            Assert.AreEqual(actual, 3100m);
        }
    }
}