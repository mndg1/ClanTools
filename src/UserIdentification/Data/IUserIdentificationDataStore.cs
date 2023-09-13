using UserIdentification.Entities;

namespace UserIdentification.Data;

internal interface IUserIdentificationDataStore
{
	Task StoreUser(UserIdEntity userId);

	Task<UserIdEntity?> GetUserId(string userName);
}
