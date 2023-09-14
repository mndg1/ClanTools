namespace Skillathon.Models;

public class SkillathonEvent
{
	public string EventName { get; set; }
	public string SkillName { get; set; }

    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    public IList<string> ParticipantNames { get; set; } = new List<string>();

    internal SkillathonEvent(string eventName, string skillName)
    {
        EventName = eventName;
        SkillName = skillName;
    }
}
