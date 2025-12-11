using LocationSystem.Domain.Exceptions;
using LocationSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting; // 添加此 using 以解决 CS0246

namespace LocationSystem.Tests.Domain.ValueObjects
{
    [TestClass]
    public class EmailTest
    {
        [TestMethod]
        public void CreateEmail_NullEmail_Throw()
        {
            // Arrange
            Assert.Throws<BussinessRuleException>(()=> { new Email(null); });
        }
    }
}
