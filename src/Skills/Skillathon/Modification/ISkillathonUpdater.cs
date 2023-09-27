using Skillathon.Models;

namespace Skillathon.Modification;

public interface ISkillathonUpdater
{
    Task UpdateSkillathonAsync(string eventName);
}
