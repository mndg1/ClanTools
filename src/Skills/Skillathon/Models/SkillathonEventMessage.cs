namespace Skillathon.Models;

internal record SkillathonEventMessage(string eventName, string SkillName, IEnumerable<Participant> Participants, bool terminate);