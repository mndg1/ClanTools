using MassTransit;
using MessagingContracts.Skills.Skillathon;
using MessagingContracts.Skills.Skillathon.Dtos;
using SkillathonEvent.Exceptions;
using SkillathonEvent.Models;

namespace SkillathonEvent.Publishing;

internal class SkillathonPublisher : ISkillathonPublisher
{
	private readonly IBus _bus;

	public SkillathonPublisher(IBus bus)
	{
		_bus = bus;
	}

	public async Task PublishSkillathonEventAsync(Skillathon skillathon, EventMessageStatus messageStatus)
	{
		var publishTask = messageStatus switch
		{
			EventMessageStatus.Created => _bus.Publish(CreateCreatedMessage(skillathon)),
			EventMessageStatus.Started => _bus.Publish(CreateStartedMessage(skillathon)),
			EventMessageStatus.Updated => _bus.Publish(CreateUpdatedMessage(skillathon)),
			EventMessageStatus.Terminated => _bus.Publish(CreateTerminatedMessage(skillathon)),
			EventMessageStatus.Deleted => _bus.Publish(CreateDeletedMessage(skillathon)),
			_ => throw new StatusNotSupportedException("The given status is not yet supported")
		};

		await publishTask;
	}

	private SkillathonCreated CreateCreatedMessage(Skillathon skillathon)
	{
		return new SkillathonCreated(skillathon.EventName, skillathon.SkillName);
	}

	private SkillathonStarted CreateStartedMessage(Skillathon skillathon)
	{
		var dtos = skillathon.Participants.Select(participant => new ParticipantDto(participant.Name, participant.ExperienceHistory));

		return new SkillathonStarted(skillathon.EventName, skillathon.SkillName, dtos);
	}

	private SkillathonUpdated CreateUpdatedMessage(Skillathon skillathon)
	{
		var dtos = skillathon.Participants.Select(participant => new ParticipantDto(participant.Name, participant.ExperienceHistory));

		return new SkillathonUpdated(skillathon.EventName, skillathon.SkillName, dtos);
	}

	private SkillathonTerminated CreateTerminatedMessage(Skillathon skillathon)
	{
		var dtos = skillathon.Participants.Select(participant => new ParticipantDto(participant.Name, participant.ExperienceHistory));

		return new SkillathonTerminated(skillathon.EventName, skillathon.SkillName, dtos);
	}

	private SkillathonDeleted CreateDeletedMessage(Skillathon skillathon)
	{
		return new SkillathonDeleted(skillathon.EventName);
	}
}