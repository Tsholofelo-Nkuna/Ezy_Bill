using Core.Presentation.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.UnitTests.DataProviders
{
    public static class GenericGridListViewModelTestDataProvider
    {
        public static IEnumerable<TestCaseData> GettersAndSettersWorkFine_DP()
        {

            var Id1 = Guid.NewGuid();
            var Id2 = Guid.NewGuid();

            var expectedEmailAddress = "tgnkuna768@gmail.com";
            var expectedArchived = true;

            yield return new TestCaseData(
                 new ContactPersonDto { Id = Id2},
                 new ContactPersonDto[] {
                     new ContactPersonDto()
                     {
                         Archived = false,
                         Email = string.Empty,
                         Id = Id1,
                     },
                      new ContactPersonDto()
                     {
                         Archived = false,
                         Email = string.Empty,
                         Id = Id2,
                     }
                 },
             new ContactPersonDto[] {
                     new ContactPersonDto()
                     {
                         Archived = false,
                         Email = string.Empty,
                         Id = Id1,
                     },
                      new ContactPersonDto()
                     {
                         Archived =expectedArchived,
                         Email = expectedEmailAddress,
                         Id = Id2,
                     }
                 },
            expectedEmailAddress,
              expectedArchived
           
                ).Returns(true);

        }
    }
}
