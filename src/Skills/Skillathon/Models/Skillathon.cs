namespace SkillathonEvent.Models;

public class Skillathon
{
	public string EventName { get; set; }
	public string SkillName { get; set; }

	public DateOnly? StartDate { get; set; }
	public DateOnly? EndDate { get; set; }
	public DateTime? LastUpdateTime { get; set; }

	public SkillathonState State { get; set; }

	public IList<Participant> Participants { get; set; } = new List<Participant>();

	public Skillathon() 
	{
		EventName = "EmptyEvent";
		SkillName = "Overall";
	}

	internal Skillathon(string eventName, string skillName)
	{
		EventName = eventName;
		SkillName = skillName;
	}

	public void RegisterParticipant(string userName)
	{
		if (ContainsPlayer(userName)) 
		{
			return;
		}

		var participant = new Participant()
		{
			Name = userName,
		};

		Participants.Add(participant);
	}

	public bool ContainsPlayer(string userName)
	{
		return Participants.Any(participant => participant.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
	}
}
