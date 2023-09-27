using Skillathon.Models;

namespace Skillathon.Publishing;

internal interface ISkillathonPublisher
{
	Task PublishSkillathonEventAsync(SkillathonEvent skillathon, EventMessageStatus messageStatus);
}
