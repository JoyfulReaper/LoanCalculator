using LoanCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanCalculator.Helpers
{
    public class LoanHelper
    {
        public static Loan GetPayments(Loan loan)
        {
            loan.Payment = CalculatePayment(loan.Amount, loan.Rate, loan.Term);

            var balance = loan.Amount;
            var totalInterest = 0.0m;
            var monthlyInterest = 0.0m;
            var monthlyPrincipal = 0.0m;
            var monthlyRate = CalculateMonthlyRate(loan.Rate);

            for(int i = 1; i <= loan.Term; i++)
            {
                monthlyInterest = CalculateMonthlyInterest(balance, monthlyRate);
                totalInterest += monthlyInterest;
                monthlyPrincipal = loan.Payment - monthlyInterest;
                balance -= monthlyPrincipal;

                var loanPayment = new LoanPayment()
                {
                    Month = i,
                    MonthlyInterest = monthlyInterest,
                    MonthlyPrincipal = monthlyPrincipal,
                    Balance = balance,
                    Payment = loan.Payment,
                    TotalInterest = totalInterest
                };

                loan.Payments.Add(loanPayment);
            }

            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.Amount + totalInterest;

            return loan;
        }

        private static decimal CalculateMonthlyInterest(decimal balance, decimal monthlyRate) => balance * monthlyRate;

        private static decimal CalculatePayment(decimal amount, decimal rate, int term)
        {
            var monthlyRate = CalculateMonthlyRate(rate);

            var rateD = Convert.ToDouble(monthlyRate);
            var amountD = Convert.ToDouble(amount);

            var paymentD = (amountD * rateD) / (1 - Math.Pow(1 + rateD, -term));

            return Convert.ToDecimal(paymentD);
        }

        private static decimal CalculateMonthlyRate(decimal rate) => rate / 1200;
    }
}
