using ClientManagement.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.UnitTests
{
    [TestFixture]
    public class ValidatorTests
    {
        [TestCase]
        public void Validators_Works()
        {
           var r = 1.5d.ToString();
           var results =   Convert.ToDouble(1.5);
            Assert.That(results > 0,Is.True);
        }
    }
}
