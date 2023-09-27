namespace MessagingContracts.Skills.Skillathon;

public record EditSkillathon(
    string EventName,
    DateOnly? StartDate,
    DateOnly? EndDate);
