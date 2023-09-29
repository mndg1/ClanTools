using ConsoleApplication.Skills.SkillathonEvent;
using MassTransit;
using System.Runtime.CompilerServices;

namespace ConsoleApplication.Extensions;

public static class MassTransitExtensions
{
	public static IBusRegistrationConfigurator AddConsoleApplicationConsumers(this IBusRegistrationConfigurator configurator)
	{
		configurator.AddConsumer<SkillathonUpdateProcessor>();

		return configurator;
	}
}
