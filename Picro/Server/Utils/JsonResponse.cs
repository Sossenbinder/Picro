using Microsoft.AspNetCore.Mvc;
using Picro.Common.Web.DataTypes;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;

namespace Picro.Server.Utils
{
	public class JsonResponse : JsonResult
	{
		private readonly HttpStatusCode _statusCode;

		protected JsonResponse(object? data, bool success, HttpStatusCode statusCode)
			: base(new JsonResponsePayload
			{
				Success = success,
				Data = data,
			})
		{
			_statusCode = statusCode;
		}

		public static JsonResponse Success(object? data = null, bool internalSuccess = true)
		{
			return new(data, internalSuccess, HttpStatusCode.OK);
		}

		public static JsonResponse<TPayload> Success<TPayload>(TPayload data = default, bool internalSuccess = true)
		{
			return new(data, internalSuccess, HttpStatusCode.OK);
		}

		public static JsonResponse<TPayload> Error<TPayload>(TPayload data = default, bool internalSuccess = false)
		{
			return new(data, internalSuccess, HttpStatusCode.OK);
		}

		public static JsonResponse Error(object? data = null)
		{
			return new(data, false, HttpStatusCode.OK);
		}

		public override Task ExecuteResultAsync([NotNull] ActionContext context)
		{
			context.HttpContext.Response.StatusCode = (int)_statusCode;
			return base.ExecuteResultAsync(context);
		}
	}

	public class JsonResponse<T> : JsonResponse
	{
		public JsonResponse(T? data, bool success, HttpStatusCode statusCode)
			: base(data, success, statusCode)
		{
		}

		public static JsonResponse<T> Success(T? data = default, bool internalSuccess = true)
		{
			return new(data, internalSuccess, HttpStatusCode.OK);
		}

		public static JsonResponse<T> Error(T? data = default)
		{
			return new(data, false, HttpStatusCode.OK);
		}
	}
}