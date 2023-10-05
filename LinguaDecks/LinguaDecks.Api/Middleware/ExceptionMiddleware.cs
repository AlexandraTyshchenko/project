using System;
using System.Threading.Tasks;
using LinguaDecks.Business.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace LinguaDecks.Api.Middleware
{
	public sealed class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception exception)
			{
				int statusCode = exception switch
				{
					BadRequestException => StatusCodes.Status400BadRequest,
					ForbiddenException => StatusCodes.Status403Forbidden,
					NotFoundException => StatusCodes.Status404NotFound,
					_ => StatusCodes.Status500InternalServerError
				};

				context.Response.StatusCode = statusCode;
				context.Response.ContentType = "application/json";

				var errorResponse = new
				{
					Code = statusCode,
					Details = exception.Message
				};

				string errorResponseJson = JsonConvert.SerializeObject(errorResponse);
				await context.Response.WriteAsync(errorResponseJson);
			}
		}

	}
}
