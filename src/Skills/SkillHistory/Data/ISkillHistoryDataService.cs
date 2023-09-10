using SkillHistory.Models;

namespace SkillHistory.Data;

internal interface ISkillHistoryDataService
{
	Task<HistoricSkillData> GetHistoricSkillDataAsync(Guid userId);

	Task StoreHistoricSkillDataAsync(Guid userId, HistoricSkillData data);
}
