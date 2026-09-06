using ClientManagement.BusinessLogicLayer.Interfaces.Base;
using ClientManagement.DataAccessLayer.Entities;
using ClientManagement.Models.DataTransferObjects;


namespace ClientManagement.BusinessLogicLayer.Interfaces
{
    public interface IUserProfileService : IGenericService<UserProfileDto, UserProfileEntity>
    {
        Task<UserProfileDto> CreateProfile(string userId, ProfileDto newProfile);
        Task<IEnumerable<UserProfileDto>> GetAllProfiles();
    }
}
