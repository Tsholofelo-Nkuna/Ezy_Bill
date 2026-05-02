using ClientManagement.UnitTests.DataProviders;
using Core.Presentation.Models.Base;
using Core.Presentation.Models.DataTransferObjects;


namespace ClientManagement.UnitTests
{
    public class GenericGridListViewModelTest
    {
        [TestCaseSource(typeof(GenericGridListViewModelTestDataProvider), nameof(GenericGridListViewModelTestDataProvider.GettersAndSettersWorkFine_DP))]
        public bool GettersAndSettersWork(ContactPersonDto subject, ContactPersonDto[] inputList, 
            ContactPersonDto[] expectedList,
            string expectedEmail, 
            bool expectedArchived)
        {
            var gViewGList = new GenericListViewModel<ContactPersonDto>(inputList);

            var itemAlreadySetWithExpectedValues = gViewGList.ViewModelState
                .Where(x => x.Id == subject.Id).Any(x => x.Archived == expectedArchived || x.Email == expectedEmail);
            Assert.That(itemAlreadySetWithExpectedValues, Is.False);

            gViewGList.Set(subject.Id, "Email", expectedEmail);
            var actualEmail = gViewGList.Get<string>(subject.Id, "Email");
            Assert.That(actualEmail == expectedEmail, Is.True);

            gViewGList.Set(subject.Id, "Archived", expectedArchived);
            Assert.That(gViewGList.Get<string>(subject.Id, "Email"), Is.EqualTo(expectedEmail));

            return expectedList
                .Where(x => x.Id == subject.Id)
                .Count(x => x.Email == expectedEmail && x.Archived == expectedArchived)
                == gViewGList.ViewModelState
                .Where(x => x.Id == subject.Id)
                .Count(x => x.Email == expectedEmail && x.Archived == expectedArchived);
           
        }
    }
}
