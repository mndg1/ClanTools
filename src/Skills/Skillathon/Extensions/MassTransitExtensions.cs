using MassTransit;
using SkillathonEvent.Modification;

namespace SkillathonEvent.Extensions;

public static class MassTransitExtensions
{
	public static IBusRegistrationConfigurator AddSkillathonConsumers(this IBusRegistrationConfigurator configurator)
	{
		configurator.AddConsumer<SkillathonCreator>();
		configurator.AddConsumer<SkillathonUpdater>();
		configurator.AddConsumer<SkillathonEditor>();
		configurator.AddConsumer<SkillathonDeleter>();

		return configurator;
	}
}
