using ClientManagement.UnitTests.DataProviders;
using Core.Presentation.Models.DataTransferObjects;
using Core.Presentation.Models.Base;


namespace ClientManagement.UnitTests
{
    [TestFixture]
   public class GenericeViewModelTests
    {


        [TestCaseSource(typeof(GenericViewModelTestDataProvider), nameof(GenericViewModelTestDataProvider.PropertySetterAndGetWorks_DP))
        ]

        #region "setter and getter tests"
        public bool PropertySetterAndGetWorks(ContactPersonDto contactP, bool expectedArchiveValue, string expectedEmailValue )
        {
          
            var vm2 = new GenericViewModel<ContactPersonDto>(contactP);
            vm2.Set("Archived", expectedArchiveValue);
            vm2.Set("Email", expectedEmailValue);
            return vm2.Get("Email", typeof(string)) == expectedEmailValue && vm2.Get("Archived", typeof(bool)) == true;
        }

        [TestCaseSource(typeof(GenericViewModelTestDataProvider), nameof(GenericViewModelTestDataProvider.PropertySetterAndGetWorks_DP_V2))
       ]
        public bool PropertySetterAndGetWorks_V2(ContactPersonDto contactP, string expectedArchiveValue, string expectedEmailValue)
        {

            var vm2 = new GenericViewModel<ContactPersonDto>(contactP);
            vm2.Set("Archived", expectedArchiveValue);
            vm2.Set("Email", expectedEmailValue);
            return vm2.Get("Email", typeof(string)) == expectedEmailValue && vm2.Get("Archived", typeof(bool)) == true;
        }

        [TestCase("Email", "nkuna@gmail.com","Archived", "True")]
        public void PropertySetterWorks_V1(string emailPropName, string emailValue, string archivePropName, bool archiveVal)
        {

            var vm = new GenericViewModel<ContactPersonDto>(new ContactPersonDto());
            Assert.That(vm.ViewModelState.Email == emailValue && vm.ViewModelState.Archived == archiveVal, Is.False);
            vm.Set(emailPropName,emailValue, (key, value, model) =>
            {
                 if(value is string strVal)
                {
                    model.Email = strVal;
                }
            });
            vm.Set(archivePropName, archiveVal, (key, value, model) =>
            {
                if(value is bool boolVal)
                {
                    model.Archived = boolVal;
                }
            });
            Assert.That(vm.ViewModelState.Email == emailValue && vm.ViewModelState.Archived == archiveVal, Is.True);

            vm.Set<string, bool>(!archiveVal)(archivePropName);
            vm.Set<string, string>("123456")(emailPropName);
            Assert.That(vm.ViewModelState.Email == "123456" && vm.ViewModelState.Archived == !archiveVal, Is.True);


        }
        #endregion

        #region "state change listener test"
        public ContactPersonDto? InitialContactPersonState_v1 { get; set; }
        public ContactPersonDto? ExpectedContactPersonState_v1 { get; set; }
        public GenericViewModel<ContactPersonDto>? GenericViewModel_v1 { get; set; }


        [Test]
        public void StateChangeListenerWorks()
        {
            this.InitialContactPersonState_v1 = new ContactPersonDto();
            Assert.That(string.IsNullOrEmpty(this.InitialContactPersonState_v1.Email)
                    && this.InitialContactPersonState_v1.IsPrimaryContact == false
                    && this.InitialContactPersonState_v1.Archived == false, Is.True);
            
            this.ExpectedContactPersonState_v1 = new ContactPersonDto()
            {
                Archived = true,
                Email = "test@gmail.com",
                IsPrimaryContact = true,
            };
            this.GenericViewModel_v1 = new GenericViewModel<ContactPersonDto>(new ContactPersonDto());
            this.GenericViewModel_v1.AddStateChangeListener(this.StateChangeListenerWorks_OnStateChangeWorks);

            this.GenericViewModel_v1.Set(nameof(this.ExpectedContactPersonState_v1.Email), this.ExpectedContactPersonState_v1.Email);
            this.GenericViewModel_v1.Set(nameof(this.ExpectedContactPersonState_v1.IsPrimaryContact), this.ExpectedContactPersonState_v1.IsPrimaryContact);
            this.GenericViewModel_v1.Set(nameof(this.ExpectedContactPersonState_v1.Archived), this.ExpectedContactPersonState_v1.Archived);

            
            Assert.That(
                this.InitialContactPersonState_v1.IsPrimaryContact == this.ExpectedContactPersonState_v1.IsPrimaryContact
                && this.InitialContactPersonState_v1.Email == this.ExpectedContactPersonState_v1.Email
                && this.InitialContactPersonState_v1.Archived == this.ExpectedContactPersonState_v1.Archived, Is.True);

            this.GenericViewModel_v1?.ClearStateChangeListeners();
        }

        private void StateChangeListenerWorks_OnStateChangeWorks(ContactPersonDto newState)
        {
            this.InitialContactPersonState_v1 = newState;
            Assert.That(this.GenericViewModel_v1.ViewModelState == newState, Is.True);
        }
        #endregion

    }
}