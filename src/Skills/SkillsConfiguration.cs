namespace Skills;

public class SkillsConfiguration
{
	public const string SECTION_NAME = "Skills";

	public IList<string> SkillNames { get; set; } = new List<string>();
}
