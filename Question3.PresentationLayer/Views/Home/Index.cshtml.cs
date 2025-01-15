using Core.Presentation.Models;
using Core.Presentation.Models.DataTransferObjects;
using Core.Presentation.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace Question3.PresentationLayer.Views.Home
{
    public partial class Index : PageBase<ClientsViewModel>
    {
     
        public override void OnInit(ClientsViewModel viewModel)
        {
            base.OnInit(viewModel);

        }
    }
}
