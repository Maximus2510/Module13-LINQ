using System;
using System.Collections.Generic;
using System.Linq;
using Task1.DoNotChange;

namespace Task1
{
    public static class LinqTask
    {
        public static IEnumerable<Customer> Linq1(IEnumerable<Customer> customers, decimal limit)
        {
            return customers.Where(c => c.Orders.Sum(o => o.Total) > limit);
        }


        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            return customers.Select(c =>
                (customer: c,
                suppliers: suppliers.Where(s => s.Country == c.Country && s.City == c.City)
                ));
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2UsingGroup(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            return customers.GroupJoin(
                suppliers,
                c => new { c.Country, c.City },
                s => new { s.Country, s.City },
                (c, supplierGroup) => (customer: c, supplier: supplierGroup)
                );
        }

        public static IEnumerable<Customer> Linq3(IEnumerable<Customer> customers, decimal limit)
        {
            return customers.Where(c =>
            {
                decimal sum = c.Orders.Sum(o => o.Total);
                return sum > limit && c.Orders.Any();
            });
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq4(
            IEnumerable<Customer> customers
        )
        {
            return customers
                .Where(c => c.Orders.Any())  // Exclude customers without any orders
                .Select(c => (
                customer: c,
                dateOfEntry: c.Orders.Min(o => o.OrderDate)
                ));
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq5(
            IEnumerable<Customer> customers
        )
        {
            return customers
                .Where(c => c.Orders.Any())  // Exclude customers without any orders
                .Select(c => (
                customer: c,
                dateOfEntry: c.Orders.Min(o => o.OrderDate)
            ))
            .OrderBy(t => t.dateOfEntry.Year)
            .ThenBy(t => t.dateOfEntry.Month)
            .ThenByDescending(t => t.customer.Orders.Sum(o => o.Total))
            .ThenBy(t => t.customer.CompanyName);
        }

        public static IEnumerable<Customer> Linq6(IEnumerable<Customer> customers)
        {
            return customers.Where(c => !string.IsNullOrEmpty(c.PostalCode) && c.PostalCode.Any(ch => !char.IsDigit(ch))
                                || string.IsNullOrEmpty(c.Region)
                                || c.Phone.IndexOf('(') == -1);
        }

        public static IEnumerable<Linq7CategoryGroup> Linq7(IEnumerable<Product> products)
        {
            /* example of Linq7result

             category - Beverages
	            UnitsInStock - 39
		            price - 18.0000
		            price - 19.0000
	            UnitsInStock - 17
		            price - 18.0000
		            price - 19.0000
             */

            throw new NotImplementedException();
        }

        public static IEnumerable<(decimal category, IEnumerable<Product> products)> Linq8(
                IEnumerable<Product> products,
            decimal cheap,
            decimal average,
            decimal expensive
)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<(string city, int averageIncome, int averageIntensity)> Linq9(
            IEnumerable<Customer> customers
        )
        {
            var result = customers.GroupBy(c => c.City)
                .Select(g => (
                    city: g.Key,
                    averageIncome: Convert.ToInt32(g.Average(c => c.Orders.Sum(o => o.Total))),
                    averageIntensity: Convert.ToInt32(Math.Round(g.Average(c => c.Orders.Length)))
            ))
                .OrderByDescending(r => r.averageIncome)
                .ThenBy(r => r.city);

            return result;
        }

        public static string Linq10(IEnumerable<Supplier> suppliers)
        {
            var countryNames = suppliers
                .Select(s => s.Country)
                .Distinct()
                .OrderBy(c => c.Length)
                .ThenBy(c => c)
                .ToList();

            return string.Join("", countryNames);
        }
    }
}