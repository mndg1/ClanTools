using SkillathonEvent.Models;

namespace SkillathonEvent.Publishing;

internal interface ISkillathonPublisher
{
	Task PublishSkillathonEventAsync(Skillathon skillathon, EventMessageStatus messageStatus);
}
