namespace Skills.Models;

public class SkillSet
{
	public IDictionary<string, Skill> Skills { get; set; }

	public SkillSet(Dictionary<string, Skill> skills) 
	{ 
		Skills = skills;
	}

	internal SkillSet(IEnumerable<string> skillNames) 
	{
		Skills = skillNames.ToDictionary(
			skillName => skillName, 
			skillName => new Skill(skillName, 0, 0));
	}
}
