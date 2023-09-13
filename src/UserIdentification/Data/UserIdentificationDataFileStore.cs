using JsonFlatFileDataStore;
using UserIdentification.Entities;

namespace UserIdentification.Data;

internal class UserIdentificationDataFileStore : IUserIdentificationDataStore
{
	private readonly IDataStore _dataStore;

	public const string FILE_NAME = "user_identification.json";

	public UserIdentificationDataFileStore(IDataStore dataStore)
	{
		_dataStore = dataStore;
	}

	public async Task StoreUser(UserIdEntity userId)
	{
		var collection = _dataStore.GetCollection<UserIdEntity>();

		var hasUpdated = await collection.UpdateOneAsync(storedId => storedId.Guid == userId.Guid, userId);

		if (!hasUpdated)
		{
			await collection.InsertOneAsync(userId);
		}
	}

	public Task<UserIdEntity?> GetUserId(string userName)
	{
		var collection = _dataStore.GetCollection<UserIdEntity>();

		var userId = collection.AsQueryable()
			.FirstOrDefault(id => id.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

		return Task.FromResult(userId);
	}
}