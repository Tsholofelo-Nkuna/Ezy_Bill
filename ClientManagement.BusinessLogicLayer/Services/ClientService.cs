using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Services.Base;
using ClientManagement.DataAccessLayer;
using ClientManagement.DataAccessLayer.Entities;
using System.Linq.Expressions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Core.Utils.Interfaces;
using ClientManagement.BusinessLogicLayer.Models;
using Core.Presentation.Models.DataTransferObjects;



namespace ClientManagement.BusinessLogicLayer.Services
{
    public class ClientService : GenericService<ClientDto, ClientEntity>, IClientService
    {
        public ClientService(
            WebDbContext dbContext, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor, 
            UserManager<IdentityUser> userManager,
            IAppStateManager<ApplicationState> appStateManager) : base(dbContext, mapper, httpContextAccessor, userManager, appStateManager)
        {
        }

        public async Task<bool> AddOrUpdate(ClientDto client)
        {
            if (client is { PrimaryContactEmail.Length: > 0 } or { PrimaryContactName.Length: > 0 } or { PrimaryContactPhone.Length: > 0 })
            {
               var removedContacts =  (await this.Get(x => x.Id == client.Id))
                    .SelectMany(x => x.ContactPerson)
                    .Select(x => new ContactPersonEntity { Id = x.Id});
                this._dbContext.RemoveRange(removedContacts);
                client.ContactPerson = client.ContactPerson
                   .Where(x => !x.IsPrimaryContact)
                   .Append(new()
                   {
                       Email = client.PrimaryContactEmail,
                       IsPrimaryContact = true,
                       Phone = client.PrimaryContactPhone,
                       Name = client.PrimaryContactName,
                   }).ToList();
               
            }

            if (client.Id != Guid.Empty)
            {
               return (await this.Update(new() { client })).Any();
            }
            else
            {
                return (await this.Insert(new() { client })).Any();
            }
        }

        public override Task<(IEnumerable<ClientDto> Items, int TotalRecords)> Get(PageRequestDto<ClientDto> pageRequest)
        {
            var pId =  this.CurrentProfileId;
            var query = this.GetQueryable(pageRequest.Filters);
            var filter = pageRequest.Filters;
           

            if(filter.CompanyName?.Trim() is string validCompanyName and { Length: > 0 })
            {
                query = query.Where(x => x.CompanyName.Contains(filter.CompanyName));
            }
            var pagedQuery = pageRequest.GetAllPages ? query.Include(x => x.ContactPerson).OrderByDescending(x => x.CreatedOn) : query.Include(x => x.ContactPerson).OrderByDescending(x => x.CreatedOn).Skip(pageRequest.PageIndex * pageRequest.PageSize).Take(pageRequest.PageSize);
            var returned = this._mapper.Map<List<ClientDto>>(pagedQuery.ToList());
            returned.ForEach(x =>
            {
                var primaryContact = x.ContactPerson.FirstOrDefault(x => x.IsPrimaryContact);
                x.PrimaryContactEmail  = primaryContact?.Email ?? string.Empty;
                x.PrimaryContactPhone = primaryContact?.Phone ?? string.Empty;
                x.PrimaryContactName = primaryContact?.Name ?? string.Empty;
            });
            return Task.FromResult<(IEnumerable<ClientDto> Items, int TotalRecords)>((returned.OrderByDescending(x => x.CreatedOn), query.Count()));
        }

        public override Task<IEnumerable<ClientDto>> Get(Expression<Func<ClientEntity, bool>> filter)
        {
           var clients =  this._entitySet
                .Where(filter)
                .Where(x=> x.ProfileId == this.CurrentProfileId).AsNoTracking().Include(x => x.ContactPerson)
                .OrderByDescending(x => x.CreatedOn)
                .ToList();
           return Task.FromResult<IEnumerable<ClientDto>>(this._mapper.Map<IEnumerable<ClientDto>>(clients));
        }

        public async override Task<bool> Delete(IEnumerable<Guid> identifiers)
        {
            var contactsToBeRemoved = (await this.Get(x => identifiers.Contains(x.Id)))
               .SelectMany(x => x.ContactPerson);
            var removedContacts = this._mapper.Map<IEnumerable<ContactPersonEntity>>(contactsToBeRemoved);
            this._dbContext.Contacts.RemoveRange(removedContacts);
            var removedClients = await this.Get(x => identifiers.Contains(x.Id));
            var removedClientE = this._mapper.Map<IEnumerable<ClientEntity>>(removedClients);
            this._entitySet.RemoveRange(removedClientE);
            return await this._dbContext.SaveChangesAsync() > 0;
        }
    }
}
