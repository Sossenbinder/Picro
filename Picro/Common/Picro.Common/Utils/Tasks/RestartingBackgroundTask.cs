﻿using Microsoft.Extensions.Logging;
using Picro.Common.Extensions;
using Picro.Common.Extensions.Async;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Picro.Common.Utils.Tasks
{
	public class RestartingBackgroundTask
	{
		private readonly Func<CancellationToken?, Task> _taskToRun;

		private readonly ILogger _logger;

		private readonly string _name;

		private readonly Func<Exception, Task> _onException;

		private Guid _id;

		private int _restartCount;

		private readonly CancellationTokenSource _internalCancellationTokenSource;

		private RestartingBackgroundTask(
			Func<CancellationToken?, Task> taskToRun,
			ILogger logger,
			RestartingBackgroundTaskOptions restartingBackgroundTaskOptions)
		{
			_taskToRun = taskToRun;
			_logger = logger;
			_name = restartingBackgroundTaskOptions.Name;
			_onException = restartingBackgroundTaskOptions.OnException;

			_id = Guid.NewGuid();

			_internalCancellationTokenSource = restartingBackgroundTaskOptions.CancellationToken.HasValue
				? CancellationTokenSource.CreateLinkedTokenSource(restartingBackgroundTaskOptions.CancellationToken.Value)
				: new CancellationTokenSource();

			Task.Factory.StartNew(ExecuteTask, TaskCreationOptions.LongRunning);
		}

		public static RestartingBackgroundTask Start(
			Func<CancellationToken?, Task> taskToRun,
			ILogger logger,
			RestartingBackgroundTaskOptions restartingBackgroundTaskOptions)
		{
			return new(taskToRun, logger, restartingBackgroundTaskOptions);
		}

		private async Task ExecuteTask()
		{
			_logger.LogInformation($"Executing initial start of {nameof(RestartingBackgroundTask)} with id {_id}.");

			var cancellationToken = _internalCancellationTokenSource.Token;
			while (!_internalCancellationTokenSource.IsCancellationRequested)
			{
				try
				{
					await _taskToRun(cancellationToken).IgnoreTaskCancelledException();
				}
				catch (Exception e)
				{
					if (_onException != null)
					{
						await _onException(e);
					}

					_restartCount++;

					LogWithIdentity(message => _logger.LogWarning(message), $"Restarting for the #{_restartCount} time");
				}
			}
		}

		public void Cancel()
		{
			_internalCancellationTokenSource.Cancel();
		}

		private void LogWithIdentity(Action<string> logFunc, string message)
		{
			var sb = new StringBuilder();
			sb.Append("RestartingBackgroundTask ");

			if (!_name.IsNullOrEmpty())
			{
				sb.Append($"'{_name}' ");
			}

			sb.Append($"{_id} - ");

			sb.Append(message);

			logFunc(sb.ToString());
		}
	}
}