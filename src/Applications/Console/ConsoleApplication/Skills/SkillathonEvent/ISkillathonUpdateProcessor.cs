using ConsoleApplication.Skills.SkillathonEvent.Models;

namespace ConsoleApplication.Skills.SkillathonEvent;

internal interface ISkillathonUpdateProcessor
{
	Task ProcessSkillathon(Skillathon skillathon);
}
