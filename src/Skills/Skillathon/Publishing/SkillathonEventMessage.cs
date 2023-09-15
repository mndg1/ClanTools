using Skillathon.Models;

namespace Skillathon.Publishing;

internal record SkillathonEventMessage(SkillathonEvent SkillathonEvent, EventMessageStatus MessageStatus);
