namespace MessagingContracts.Skills.Skillathon.Dtos;

public record ParticipantDto(string Name, IDictionary<DateOnly, int> ExperienceHistory);
