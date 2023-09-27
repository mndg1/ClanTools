using MessagingContracts.Skills.Skillathon.Dtos;

namespace MessagingContracts.Skills.Skillathon;

public record SkillathonTerminated(
    string EventName,
    string SkillName,
	IEnumerable<ParticipantDto> Participants);