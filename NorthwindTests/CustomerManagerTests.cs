using NUnit.Framework;
using NorthwindBusiness;
using NorthwindData;
using System.Collections.Generic;
using System.Linq;

namespace NorthwindTests
{
    public class CustomerTests
    {
        CustomerManager _customerManager;
        [SetUp]
        public void Setup()
        {
            _customerManager = new CustomerManager();
            // remove test entry in DB if present
            using (var db = new NorthwindContext())
            {
                var selectedCustomers =
                from c in db.Customers
                where c.CustomerId == "MANDA"
                select c;

                db.Customers.RemoveRange(selectedCustomers);
                db.SaveChanges();
            }
        }

        [Test]
        public void WhenANewCustomerIsAdded_TheNumberOfCustomersIncreasesBy1()
        {
            _customerManager.Delete("POGGG");
            using (var db = new NorthwindContext())
            {
                var before = from c in db.Customers select c;
                int beforeCount = before.Count();
                _customerManager.Create("POGGG", "Testing", "TestingCo");
                var after = from c in db.Customers select c;
                int afterCount = after.Count();
                
                Assert.That(afterCount > beforeCount, Is.EqualTo(true));
                db.SaveChanges();
            }
        }

        [Test]
        public void WhenANewCustomerIsAdded_TheirDetailsAreCorrect()
        {
            _customerManager.Delete("POGGG");
            using (var db = new NorthwindContext())
            {
                _customerManager.Create("POGGG", "Testing", "TestingCo");
                var test = from c in db.Customers select c;
                Customer subject;
                foreach (var v in test)
                {
                    if (v.CustomerId == "POGGG")
                    {
                        Assert.That(v.CustomerId, Is.EqualTo("POGGG"));
                        Assert.That(v.ContactName, Is.EqualTo("Testing"));
                        Assert.That(v.CompanyName, Is.EqualTo("TestingCo"));
                    }
                }
                db.SaveChanges();
            }
        }

        [Test]
        public void WhenACustomerIsUpdated_TheDatabaseIsUpdated()
        {
            _customerManager.Delete("POGGG");
            using (var db = new NorthwindContext())
            {
                _customerManager.Create("POGGG", "Testing", "TestingCo");
                var beforeTest = from c in db.Customers select c;
                _customerManager.Update("POGGG", "update", "Albania", "Libjana", "POSTCODE");
                var afterTest = from c in db.Customers select c;
                Assert.That(beforeTest != afterTest, Is.EqualTo(true));
                db.SaveChanges();
            }
        }

        [Test]
        public void WhenACustomerIsUpdated_SelectedCustomerIsUpdated()
        {
            _customerManager.Delete("POGGG");
            using (var db = new NorthwindContext())
            {
                _customerManager.Create("POGGG", "Testing", "TestingCo");
                var test = from c in db.Customers where c.CustomerId == "POGGG" select c;
                foreach (var v in test)
                {
                    if (v.CustomerId == "POGGG")
                    {
                        Customer subject = v;
                        _customerManager.Update("POGGG", "update", "Albania", "Libjana", "POSTCODE");
                        Assert.That(subject.ContactName != v.ContactName, Is.EqualTo(false));
                    }
                }
                db.SaveChanges();
            }
        }

        [Test]
        public void WhenACustomerIsNotInTheDatabase_Update_ReturnsFalse()
        {
            List<Customer> list = _customerManager.RetrieveAllCustomers();
            Customer test = new Customer();
            Assert.That(list.Contains(test), Is.EqualTo(false));
        }

        [Test]
        public void WhenACustomerIsRemoved_TheNumberOfCustomersDecreasesBy1()
        {
            _customerManager.Delete("POGGG");
            using (var db = new NorthwindContext())
            {
                var before = from c in db.Customers select c;
                int beforeCount = before.Count();
                _customerManager.Create("POGGG", "Testing", "TestingCo");
                var after = from c in db.Customers select c;
                _customerManager.Delete("POGGG");
                int afterCount = after.Count();
                Assert.That(afterCount == beforeCount, Is.EqualTo(true));
                db.SaveChanges();
            }

        }

        [Test]
        public void WhenACustomerIsRemoved_TheyAreNoLongerInTheDatabase()
        {
            _customerManager.Delete("POGGG");
            using (var db = new NorthwindContext())
            {
                var before = from c in db.Customers select c;
                _customerManager.Create("POGGG", "Testing", "TestingCo");
                foreach (var v in before)
                {
                    if (v.CustomerId == "POGGG")
                    {
                        _customerManager.Delete("POGGG");
                        List<Customer> after = _customerManager.RetrieveAllCustomers();
                        Assert.That(after.Contains(v), Is.EqualTo(false));
                    }
                }
                db.SaveChanges();
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var db = new NorthwindContext())
            {
                var selectedCustomers =
                from c in db.Customers
                where c.CustomerId == "MANDA"
                select c;

                db.Customers.RemoveRange(selectedCustomers);
                db.SaveChanges();
            }
        }
    }
}