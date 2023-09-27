using System.Runtime.Serialization;

namespace SkillathonEvent.Exceptions;

internal class StatusNotSupportedException : Exception
{
	public StatusNotSupportedException()
	{
	}

	public StatusNotSupportedException(string? message) : base(message)
	{
	}

	public StatusNotSupportedException(string? message, Exception? innerException) : base(message, innerException)
	{
	}

	protected StatusNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
