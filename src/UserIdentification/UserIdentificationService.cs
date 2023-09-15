using Microsoft.Extensions.Logging;
using UserIdentification.Data;
using UserIdentification.Entities;

namespace UserIdentification;

internal class UserIdentificationService : IUserIdentificationService
{
	private readonly IUserIdentificationDataStore _userIdentificationDataService;
	private readonly IGuidProvider _guidProvider;
	private readonly ILogger<UserIdentificationService> _logger;

	public UserIdentificationService(
		IUserIdentificationDataStore userIdentificationDataService,
		IGuidProvider guidProvider,
		ILogger<UserIdentificationService> logger)
	{
		_userIdentificationDataService = userIdentificationDataService;
		_guidProvider = guidProvider;
		_logger = logger;
	}

	public async Task<Guid?> RegisterNewUser(string userName)
	{
		var isRegisterd = await IsRegistered(userName);

		if (isRegisterd)
		{
			_logger.LogInformation("Attempted to register user {userName} but {userName} is already a registered user.", userName);
			return null;
		}

		var guid = _guidProvider.CreateGuid();
		var userId = new UserIdEntity(userName, guid.ToString());

		await _userIdentificationDataService.StoreUser(userId);

		return guid;
	}

	public async Task<Guid?> GetUserId(string userName)
	{
		var userId = await _userIdentificationDataService.GetUserId(userName);

		if (userId is null)
		{
			_logger.LogWarning("No registered user {userName} was found.", userName);
			return null;
		}

		if (!Guid.TryParse(userId.Guid, out var guid)) 
		{
			_logger.LogWarning("Failed to convert {guidString} to a GUID.", userId.Guid);
			return null;
		}

		return guid;
	}

	public async Task UpdateUserName(string oldName, string newName)
	{
		var existingId = await GetUserId(oldName);

		if (!existingId.HasValue)
		{
			_logger.LogWarning("Could not update name of {oldName} because this user was not found.", oldName);
			return;
		}

		var existingNewId = await GetUserId(newName);

		if (existingNewId.HasValue) 
		{
			_logger.LogWarning("Could not update name of {oldName} to {newName} because a user named {newName} already exists.", 
				oldName, newName, newName);
			return;
		}

		var newUserId = new UserIdEntity(newName, existingId.Value.ToString());
		await _userIdentificationDataService.StoreUser(newUserId);
	}

	private async Task<bool> IsRegistered(string userName)
	{
		var existingGuid = await _userIdentificationDataService.GetUserId(userName);

		return existingGuid is not null;
	}
}
