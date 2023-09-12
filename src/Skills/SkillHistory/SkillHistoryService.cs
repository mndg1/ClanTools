using Microsoft.Extensions.Logging;
using SkillHistory.Data;
using SkillHistory.Extensions;
using SkillHistory.Models;
using Skills.Models;

namespace SkillHistory;

internal class SkillHistoryService : ISkillHistoryService
{
	private readonly ISkillHistoryDataService _skillHistoryDataService;
	private readonly ILogger<SkillHistoryService> _logger;

	public SkillHistoryService(ISkillHistoryDataService skillHistoryDataService, ILogger<SkillHistoryService> logger)
	{
		_skillHistoryDataService = skillHistoryDataService;
		_logger = logger;
	}

	public async Task<HistoricSkillData> GetSkillHistoryAsync(Guid userId)
	{
		var historicData = await _skillHistoryDataService.GetHistoricSkillDataAsync(userId);

		return historicData;
	}

	public async Task<HistoricSkillData> GetSkillHistorySinceAsync(Guid userId, DateTime since)
	{
		return await GetSkillHistoryPeriodAsync(userId, since, DateTime.Now);
	}

	public async Task<HistoricSkillData> GetSkillHistoryPeriodAsync(Guid userId, DateTime periodStart, DateTime periodEnd)
	{
		var fullHistory = await GetSkillHistoryAsync(userId);

		if (!fullHistory.SkillHistory.Any())
		{
			_logger.LogWarning("No historic skill data found for {userId}.", userId);
			return fullHistory;
		}

		var requestedHistoryEntries = fullHistory.SkillHistory
			.Where(entry => IsWithinPeriod(entry.Key, periodStart.ToUtcDateOnly(), periodEnd.ToUtcDateOnly()))
			.ToDictionary(entry => entry.Key, entry => entry.Value);

		var requestedHistory = new HistoricSkillData(requestedHistoryEntries);

		if (!requestedHistory.SkillHistory.Any()) 
		{
			_logger.LogWarning("No historical skill data entries found in period `{periodStart} : {periodEnd}` for {userId}.", 
				periodStart, periodEnd, userId);
		}

		return requestedHistory;
	}

	public async Task StoreCurrentDataAsync(Guid userId, SkillSet skillSet)
	{
		await StoreSkillDataAsync(userId, skillSet, DateTime.Now);
	}

	public async Task StoreSkillDataAsync(Guid userId, SkillSet skillSet, DateTime date)
	{
		var dataMapping = new Dictionary<DateOnly, SkillSet>() 
		{
			{ DateOnly.FromDateTime(date.ToUniversalTime()), skillSet },
		};

		var dataToStore = new HistoricSkillData(dataMapping);

		await _skillHistoryDataService.StoreHistoricSkillDataAsync(userId, dataToStore);
	}

	private static bool IsWithinPeriod(DateOnly date, DateOnly periodStart, DateOnly periodEnd)
	{
		return date >= periodStart && date <= periodEnd;
	}
}
