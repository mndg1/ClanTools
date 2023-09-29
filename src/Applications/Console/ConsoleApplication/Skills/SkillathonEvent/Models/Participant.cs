namespace ConsoleApplication.Skills.SkillathonEvent.Models;

internal record Participant(string UserName, IDictionary<DateOnly, int> ExperienceHistory);
