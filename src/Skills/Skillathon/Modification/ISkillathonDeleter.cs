namespace Skillathon.Modification;

internal interface ISkillathonDeleter
{
	Task DeleteSkillathonAsync(string eventName);
}
