namespace Skills.Models;

public class SkillSet
{
	internal IDictionary<string, Skill> Skills { get; init; }

	internal SkillSet(IEnumerable<string> skillNames) 
	{
		Skills = skillNames.ToDictionary(
			skillName => skillName, 
			skillName => new Skill(skillName, 0, 0));
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
