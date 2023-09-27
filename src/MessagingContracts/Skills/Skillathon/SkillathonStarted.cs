using MessagingContracts.Skills.Skillathon.Dtos;

namespace MessagingContracts.Skills.Skillathon;

public record SkillathonStarted(
    string EventName,
    string SkillName,
	IEnumerable<ParticipantDto> Participants);