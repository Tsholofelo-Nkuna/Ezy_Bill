using Core.Presentation.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.UnitTests.DataProviders
{
    public static class GenericViewModelTestDataProvider
    {
       public static IEnumerable<TestCaseData> PropertySetterAndGetWorks_DP()
        {
            yield 
                return new TestCaseData(new ContactPersonDto { 
                    Archived = false, Email = "" }, true, "tgnkuna768@gmail.com").Returns(true);
       
        }

        public static IEnumerable<TestCaseData> PropertySetterAndGetWorks_DP_V2()
        {
           
            yield return new TestCaseData(new ContactPersonDto
            {
                Archived = false,
                Email = ""
            }, bool.TrueString, "nkuna@gmail.com").Returns(true);
        }
    }
}
