namespace SkillathonEvent.Modification;

internal interface ISkillathonDeleter
{
	Task DeleteSkillathonAsync(string eventName);
}
