namespace SkillathonEvent.Modification;

internal interface ISkillathonEditor
{
	Task SetStartDateAsync(string eventName, DateOnly? date);
	
	Task SetEndDateAsync(string eventName, DateOnly? date);
}
