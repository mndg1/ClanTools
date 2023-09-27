using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace SharedTestResources;

public class ClanToolsAutoDataAttribute : AutoDataAttribute
{
	public ClanToolsAutoDataAttribute() : base(() => CreateFixture().Customize(new AutoNSubstituteCustomization()))
	{
	}

	private static Fixture CreateFixture()
	{
		var fixture = new Fixture();

		fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));

		return fixture;
	}
}
