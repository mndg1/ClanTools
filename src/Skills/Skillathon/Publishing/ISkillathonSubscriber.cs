using Skillathon.Models;

namespace Skillathon.Publishing;

public interface ISkillathonSubscriber
{
    Task ConsumeSkillathonStart(SkillathonEvent skillathonEvent);

    Task ConsumeSkillathonUpdate(SkillathonEvent skillathonEvent);

    Task ConsumeSkillathonEnd(SkillathonEvent skillathonEvent);
}
