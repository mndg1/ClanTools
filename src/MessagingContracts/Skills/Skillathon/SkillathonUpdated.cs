using MessagingContracts.Skills.Skillathon.Dtos;

namespace MessagingContracts.Skills.Skillathon;

public record SkillathonUpdated(
    string EventName,
    string SkillName,
    IEnumerable<ParticipantDto> Participants);