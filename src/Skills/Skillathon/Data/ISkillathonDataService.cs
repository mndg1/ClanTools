﻿using Skillathon.Models;

namespace Skillathon.Data;

internal interface ISkillathonDataService
{
	Task StoreSkillathonAsync(SkillathonEvent skillathon);

	Task<SkillathonEvent?> GetSkillathonAsync(string eventName);

	Task<IEnumerable<SkillathonEvent>> GetSkillathonsAsync();

	Task DeleteSkillathonAsync(string eventName);
}
