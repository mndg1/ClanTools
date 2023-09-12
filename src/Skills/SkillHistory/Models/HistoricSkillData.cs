using Skills.Models;

namespace SkillHistory.Models;

public record HistoricSkillData(IDictionary<DateOnly, SkillSet> SkillHistory);
