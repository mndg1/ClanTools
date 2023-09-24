﻿using JsonFlatFileDataStore;
using Shared;
using Skillathon.Models;

namespace Skillathon.Data;

internal class SkillathonDataFileService : ISkillathonDataService
{
	private readonly IDataStore _dataStore;

	public const string FILE_NAME = "skillathons.json";

	public SkillathonDataFileService(IEnumerable<INamedDataStore> dataStores)
	{
		_dataStore = dataStores.First(dataStore => dataStore.FileName.Equals(FILE_NAME)).DataStore;
	}

	public async Task StoreSkillathonAsync(SkillathonEvent skillathon)
	{
		var collection = _dataStore.GetCollection<SkillathonEvent>();

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

	public Task<SkillathonEvent?> GetSkillathonAsync(string eventName)
	{
		var collection = _dataStore.GetCollection<SkillathonEvent>();

		var skillathon = collection.AsQueryable().FirstOrDefault(existing => IsMatchingName(existing.EventName, eventName));

		return Task.FromResult(skillathon);
	}

	public Task<IEnumerable<SkillathonEvent>> GetSkillathonsAsync()
	{
		var collection = _dataStore.GetCollection<SkillathonEvent>();

		return Task.FromResult(collection.AsQueryable());
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
