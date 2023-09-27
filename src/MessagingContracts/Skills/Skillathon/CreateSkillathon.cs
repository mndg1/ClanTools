namespace MessagingContracts.Skills.Skillathon;

public record CreateSkillathon(
    string EventName,
    string SkillName,
    DateOnly? StartDate = null,
    DateOnly? EndDate = null);