using Business;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BillsToPay.Test
{
    public class InterestCalculationUnitTest
    {
        private readonly IBillBusiness _billBusiness;
        private readonly IEnumerable<InterestRule> _rules;

        public InterestCalculationUnitTest() 
        {
            var host = WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            var provider = host.Services.CreateScope().ServiceProvider;
            _billBusiness = provider.GetRequiredService<IBillBusiness>();

            _rules = new List<InterestRule>
            {
                new InterestRule { InterestRuleId = 1, DelayDays = 3, Penalty = 0.02m, InterestPerDay = 0.001m },
                new InterestRule { InterestRuleId = 2, DelayDays = 4, Penalty = 0.03m, InterestPerDay = 0.002m },
                new InterestRule { InterestRuleId = 3, DelayDays = 6, Penalty = 0.05m, InterestPerDay = 0.003m }
            };
        }

        [Fact]
        public void CompareValueWithoutCorrection_Test()
        {
            // Arrange
            var model = new Bill
            {                
                DueDate = DateTime.Now,
                PaymentDate = DateTime.Now,
                ValueOriginal = 100.0m
            };

            // Act
            (int delayDays, decimal valueCorrected) = _billBusiness.InterestCalculation(model, _rules.ToArray());

            // Assert
            Assert.True((delayDays == 0) && (valueCorrected.Equals(model.ValueOriginal)));
        }

        [Fact]
        public void CompareValueWithCorrection_Test()
        {
            // Arrange
            var model = new Bill
            {
                DueDate = DateTime.Now,
                PaymentDate = DateTime.Now.AddDays(1),
                ValueOriginal = 100.0m
            };

            // Act
            (int delayDays, decimal valueCorrected) = _billBusiness.InterestCalculation(model, _rules.ToArray());

            // Assert
            Assert.True((delayDays == 1) && (valueCorrected.Equals(102.1m)));
        }


        [Fact]
        public void CompareDueDateBiggerThanPaymentDate_Test()
        {
            // Arrange
            var model = new Bill
            {
                DueDate = DateTime.Now.AddDays(1),
                PaymentDate = DateTime.Now,
                ValueOriginal = 100.0m
            };

            // Act
            (int delayDays, decimal valueCorrected) = _billBusiness.InterestCalculation(model, _rules.ToArray());

            // Assert
            Assert.True((delayDays == 0) && (valueCorrected.Equals(model.ValueOriginal)));
        }


        [Fact]
        public void CompareValueWithCorrectionOfThreeDays_Test()
        {
            // Arrange
            var model = new Bill
            {
                DueDate = DateTime.Now,
                PaymentDate = DateTime.Now.AddDays(3),
                ValueOriginal = 100.0m
            };

            // Act
            (int delayDays, decimal valueCorrected) = _billBusiness.InterestCalculation(model, _rules.ToArray());

            // Assert
            Assert.True((delayDays == 3) && (valueCorrected.Equals(102.3m)));
        }

        [Fact]
        public void CompareValueWithCorrectionOfFourDays_Test()
        {
            // Arrange
            var model = new Bill
            {
                DueDate = DateTime.Now,
                PaymentDate = DateTime.Now.AddDays(4),
                ValueOriginal = 100.0m
            };

            // Act
            (int delayDays, decimal valueCorrected) = _billBusiness.InterestCalculation(model, _rules.ToArray());

            // Assert
            Assert.True((delayDays == 4) && (valueCorrected.Equals(103.8m)));
        }

        [Fact]
        public void CompareValueWithCorrectionOfFiveDays_Test()
        {
            // Arrange
            var model = new Bill
            {
                DueDate = DateTime.Now,
                PaymentDate = DateTime.Now.AddDays(5),
                ValueOriginal = 100.0m
            };

            // Act
            (int delayDays, decimal valueCorrected) = _billBusiness.InterestCalculation(model, _rules.ToArray());

            // Assert
            Assert.True((delayDays == 5) && (valueCorrected.Equals(104m)));
        }

        [Fact]
        public void CompareValueWithCorrectionOfSixDays_Test()
        {
            // Arrange
            var model = new Bill
            {
                DueDate = DateTime.Now,
                PaymentDate = DateTime.Now.AddDays(6),
                ValueOriginal = 100.0m
            };

            // Act
            (int delayDays, decimal valueCorrected) = _billBusiness.InterestCalculation(model, _rules.ToArray());

            // Assert
            Assert.True((delayDays > 5) && (valueCorrected.Equals(106.8m)));
        }

    }
}
