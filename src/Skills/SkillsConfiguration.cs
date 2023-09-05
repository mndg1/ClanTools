namespace Skills;

public class SkillsConfiguration
{
	public const string SECTION_NAME = "Skills";

	public int ApiRetryAmount {  get; set; }
	public float ApiRetryInterval { get; set; }
	public float ApiRetryExponent { get; set; }

	public IList<string> SkillNames { get; set; } = new List<string>();
}
