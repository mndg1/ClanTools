using Skillathon.Models;

namespace Skillathon.Publishing;

public interface ISkillathonPublisher
{
	Task PublishSkillathonAsync(SkillathonEvent skillathon, EventMessageStatus status);
}
