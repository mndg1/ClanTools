namespace Skillathon.Models;

public class Participant
{
	public string Name { get; init; } = string.Empty;
	public IDictionary<DateOnly, int> ExperienceHistory { get; set; } = new Dictionary<DateOnly, int>();
}