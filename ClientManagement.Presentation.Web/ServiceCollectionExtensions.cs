using ClientManagement.Presentation.Web.Components.Pages.Clients.State.Details;
using ClientManagement.Presentation.Web.Components.Pages.Clients.State.Index;
using ClientManagement.Presentation.Web.Components.Pages.Invoices.State;
using ClientManagement.Presentation.Web.Components.Pages.Products.State;

namespace ClientManagement.Presentation.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStateManagers(this IServiceCollection services) {
            services.AddSingleton<IndexStateManager>();
            services.AddSingleton<DetailsStateManager>();
            services.AddSingleton<ProductStateManager>();
            services.AddSingleton<InvoiceStateManager>();
            return services;
        }
    }
}
