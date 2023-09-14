using Skillathon.Models;

namespace Skillathon.Publishing;

public interface ISkillathonPublisher
{
	ISet<ISkillathonSubscriber> Subscribers { get; }

	Task PublishSkillathonStart(SkillathonEvent skillathon);

	Task PublishSkillathonUpdate(SkillathonEvent skillathon);

	Task PublishSkillathonEnd(SkillathonEvent skillathon);
}
