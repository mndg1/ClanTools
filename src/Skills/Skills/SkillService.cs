﻿using Skills.Data;
using Skills.Models;

namespace Skills;

internal class SkillService : ISkillService
{
	private readonly ISkillDataRetriever _skillDataRetriever;

	public SkillService(ISkillDataRetriever skillDataRetriever)
	{
		_skillDataRetriever = skillDataRetriever;
	}

	public Task<SkillSet> GetSkillSetAsync(string userName)
	{
		return _skillDataRetriever.GetSkillSetAsync(userName);
	}
}