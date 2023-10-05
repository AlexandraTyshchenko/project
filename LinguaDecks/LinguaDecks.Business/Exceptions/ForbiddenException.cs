﻿using System;

namespace LinguaDecks.Business.Exceptions
{
	public class ForbiddenException : Exception
	{
		public ForbiddenException()
		{
		}

		public ForbiddenException(string message)
			: base(message)
		{
		}

		public ForbiddenException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
