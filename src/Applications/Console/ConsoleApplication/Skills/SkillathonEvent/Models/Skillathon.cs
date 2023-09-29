namespace ConsoleApplication.Skills.SkillathonEvent.Models;

internal record Skillathon(string EventName, string SkillName, IEnumerable<Participant> Participants);
