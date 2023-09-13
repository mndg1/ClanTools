using UserIdentification.Entities;

namespace UserIdentification.Data;

internal interface IUserIdentificationDataService
{
	Task StoreUser(UserIdEntity userId);

	Task<UserIdEntity?> GetUserId(string userName);
}
