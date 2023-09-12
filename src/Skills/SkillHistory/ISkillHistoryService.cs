using SkillHistory.Models;
using Skills.Models;

namespace SkillHistory;

public interface ISkillHistoryService
{
	Task<HistoricSkillData> GetSkillHistoryAsync(Guid userId);

	Task<HistoricSkillData> GetSkillHistorySinceAsync(Guid userId, DateTime since);

	Task<HistoricSkillData> GetSkillHistoryPeriodAsync(Guid userId, DateTime periodStart, DateTime periodEnd);

	Task StoreCurrentDataAsync(Guid userId, SkillSet skillSet);

	Task StoreSkillDataAsync(Guid userId, SkillSet skillSet, DateTime date);
}
