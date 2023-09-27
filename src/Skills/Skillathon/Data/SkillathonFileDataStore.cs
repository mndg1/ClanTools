using JsonFlatFileDataStore;
using Shared;
using SkillathonEvent.Models;

namespace SkillathonEvent.Data;

internal class SkillathonFileDataStore : ISkillathonDataStore
{
	private readonly IDataStore _dataStore;

	public const string FILE_NAME = "skillathons.json";

	public SkillathonFileDataStore(IEnumerable<INamedDataStore> dataStores)
	{
		_dataStore = dataStores.First(dataStore => dataStore.FileName.Equals(FILE_NAME)).DataStore;
	}

	public async Task StoreSkillathonAsync(Skillathon skillathon)
	{
		var collection = _dataStore.GetCollection<Skillathon>();

		var existing = collection.AsQueryable().FirstOrDefault(existing => IsMatchingName(existing.EventName, skillathon.EventName));

		if (existing is not null)
		{
			await collection.ReplaceOneAsync(existing => IsMatchingName(existing.EventName, skillathon.EventName), skillathon);
		}
		else
		{
			await collection.InsertOneAsync(skillathon);
		}
	}

	public Task<Skillathon?> GetSkillathonAsync(string eventName)
	{
		var collection = _dataStore.GetCollection<Skillathon>();

		var skillathon = collection.AsQueryable().FirstOrDefault(existing => IsMatchingName(existing.EventName, eventName));

		return Task.FromResult(skillathon);
	}

	public Task<IEnumerable<Skillathon>> GetSkillathonsAsync()
	{
		var collection = _dataStore.GetCollection<Skillathon>();

		return Task.FromResult(collection.AsQueryable());
	}

	public async Task DeleteSkillathonAsync(string eventName)
	{
		var collection = _dataStore.GetCollection<Skillathon>();

		await collection.DeleteOneAsync(existing => IsMatchingName(existing.EventName, eventName));
	}

	private bool IsMatchingName(string existingName, string eventName)
	{
		return existingName.Equals(eventName, StringComparison.OrdinalIgnoreCase);
	}
}
