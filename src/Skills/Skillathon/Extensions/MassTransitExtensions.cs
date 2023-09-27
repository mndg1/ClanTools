using MassTransit;
using Skillathon.Modification;

namespace Skillathon.Extensions;

public static class MassTransitExtensions
{
	public static IBusRegistrationConfigurator AddSkillathonConsumers(this IBusRegistrationConfigurator configurator)
	{
		configurator.AddConsumer<SkillathonUpdater>();

		return configurator;
	}
}
