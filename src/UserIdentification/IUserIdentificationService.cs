namespace UserIdentification;

public interface IUserIdentificationService
{
	Task<Guid?> RegisterNewUser(string userName);

	Task<Guid?> GetUserId(string userName);

	Task UpdateUserName(string oldName, string newName);
}
