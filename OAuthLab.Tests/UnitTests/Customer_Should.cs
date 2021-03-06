﻿using OAuthLab.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OAuthLab.Tests.UnitTests
{
    public class Customer_Should
    {
        private Customer customer;

        public Customer_Should()
        {
            customer = new Customer();
            customer.FirstName = "Maria";
            customer.LastName = "Anders";
        }

        [Fact]
        public void SecretHashHasValue()
        {
            Assert.Equal("???0?\n???~???QEW", customer.SecretHash);
        }
    }
}
