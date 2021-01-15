using System;
using System.Threading.Tasks;

namespace Picro.Common.Extensions.Async
{
	public static class TaskExtensions
	{
		public static async Task<bool> WaitAsync(this Task task, TimeSpan timeSpan)
		{
			await task.ContinueWith(_ => Task.Delay(timeSpan));

			return task.IsCompleted;
		}

		public static async Task<bool> WaitAsync<T>(this Task<T> task, TimeSpan timeSpan)
		{
			await task.ContinueWith(_ => Task.Delay(timeSpan));

			return task.IsCompleted;
		}

		public static Task IgnoreTaskCancelledException(this Task task)
		{
			return task.ContinueWith(continuedTask => continuedTask.Exception?.Handle(ex => ex is TaskCanceledException));
		}
	}
}