using JsonFlatFileDataStore;
using Microsoft.Extensions.Logging;
using Shared;
using SkillHistory.Models;
using Skills.Models;

namespace SkillHistory.Data;

internal class SkillHistoryDataFileService : ISkillHistoryDataService
{
	private readonly IDataStore _dataStore;
	private readonly ILogger<SkillHistoryDataFileService> _logger;

	public const string FILE_NAME = "skill_history.json";

	public SkillHistoryDataFileService(IEnumerable<INamedDataStore> dataStores, ILogger<SkillHistoryDataFileService> logger)
	{
		_dataStore = dataStores.First(dataStore => dataStore.FileName.Equals(FILE_NAME)).DataStore;
		_logger = logger;
	}

	public Task<HistoricSkillData> GetHistoricSkillDataAsync(Guid userId)
	{
		HistoricSkillData requestedHistory;

		try
		{
			requestedHistory = _dataStore.GetItem<HistoricSkillData>(userId.ToString());
		}
		catch (KeyNotFoundException)
		{
			requestedHistory = new(new Dictionary<DateOnly, SkillSet>());
			_logger.LogInformation("No prior data for {userId} found.", userId);
		}

		return Task.FromResult(requestedHistory);
	}

	public async Task StoreHistoricSkillDataAsync(Guid userId, HistoricSkillData data)
	{
		var historicData = await GetHistoricSkillDataAsync(userId);

		foreach (var item in data.SkillHistory)
		{
			historicData.SkillHistory[item.Key] = item.Value;
		}

		await _dataStore.ReplaceItemAsync(userId.ToString(), historicData, upsert: true);
	}
}
