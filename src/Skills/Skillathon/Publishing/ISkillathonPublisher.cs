using Skillathon.Models;

namespace Skillathon.Publishing;

internal interface ISkillathonPublisher
{
	Task PublishSkillathonAsync(SkillathonEvent skillathon);
}
