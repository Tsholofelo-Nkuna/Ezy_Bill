using Core.Presentation.Models.DataTransferObjects;
using Core.Presentation.Models.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Presentation.Models;
using ClientManagement.Presentation.Models.DataTransferObjects;

namespace ClientManagement.UnitTests.DataProviders
{
    public static class FormComponentTestsDataProvider
    {
        public static IEnumerable<TestCaseData>FormComponentWorks_DP()
        {
            yield return new TestCaseData(
                new GenericListViewModel<ClientDto>(Enumerable.Empty<ClientDto>().Append(new ClientDto())),
                new ClientDto { Archived = true, PrimaryContactEmail = "tgnkuna768@gmail.com"},
                "tgnkuna768@gmail.com",
                true
                )
            .Returns(true);
        }

    }
}
