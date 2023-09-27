using SkillathonEvent.Models;

namespace SkillathonEvent.Modification;

public interface ISkillathonUpdater
{
    Task UpdateSkillathonAsync(string eventName);
}
