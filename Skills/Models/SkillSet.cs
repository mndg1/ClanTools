namespace Skills.Models;

public class SkillSet
{
	internal IDictionary<string, Skill> Skills { get; init; } = new Dictionary<string, Skill>();

	internal SkillSet() 
	{
	}

	public Skill GetSkill(string skillName)
	{
		return Skills[skillName];
	}

	public IEnumerable<Skill> GetSkills()
	{
		return Skills.Values;
	}
}
