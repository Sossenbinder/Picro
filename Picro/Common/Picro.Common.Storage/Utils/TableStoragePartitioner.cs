using System;

namespace Picro.Common.Storage.Utils
{
	public static class TableStoragePartitioner
	{
		public static string Partition<T>(T? toPartition, int partitions = 16)
		{
			if (toPartition == null)
			{
				throw new ArgumentNullException(nameof(toPartition));
			}

			var stringifiedPartitionable = toPartition.ToString()!;

			var partition = stringifiedPartitionable[0] % partitions;

			return partition.ToString();
		}
	}
}