namespace UserIdentification;

public interface IUserIdenificationService
{
	Task RegisterNewUser(string userName);

	Task<Guid?> GetUserId(string userName);

	Task UpdateUserName(string oldName, string newName);
}
