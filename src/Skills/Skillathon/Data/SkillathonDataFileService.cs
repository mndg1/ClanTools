using JsonFlatFileDataStore;
using Skillathon.Models;

namespace Skillathon.Data;

internal class SkillathonDataFileService : ISkillathonDataService
{
	private readonly IDataStore _dataStore;

	public const string FILE_NAME = "skillathons.json";

	public SkillathonDataFileService(IDataStore dataStore)
	{
		_dataStore = dataStore;
	}

	public async Task StoreSkillathonAsync(SkillathonEvent skillathon)
	{
		var collection = _dataStore.GetCollection<SkillathonEvent>();

		var hasUpdated = await collection.UpdateOneAsync(existing => IsMatchingName(existing.EventName, skillathon.EventName), skillathon);

		if (!hasUpdated)
		{
			await collection.InsertOneAsync(skillathon);
		}
	}

	public Task<SkillathonEvent?> GetSkillathonAsync(string eventName)
	{
		var collection = _dataStore.GetCollection<SkillathonEvent>();

		var skillathon = collection.AsQueryable().FirstOrDefault(existing => IsMatchingName(existing.EventName, eventName));

		return Task.FromResult(skillathon);
	}

	public async Task DeleteSkillathonAsync(string eventName)
	{
		var collection = _dataStore.GetCollection<SkillathonEvent>();

		await collection.DeleteOneAsync(existing => IsMatchingName(existing.EventName, eventName));
	}

	private bool IsMatchingName(string existingName, string eventName)
	{
		return existingName.Equals(eventName, StringComparison.OrdinalIgnoreCase);
	}
}
