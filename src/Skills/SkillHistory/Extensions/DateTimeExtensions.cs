namespace SkillHistory.Extensions;

internal static class DateTimeExtensions
{
	public static DateOnly ToUtcDateOnly(this DateTime date)
	{
		return DateOnly.FromDateTime(date.ToUniversalTime());
	}
}
