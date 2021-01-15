using System.Collections.Generic;

namespace Picro.Common.Extensions
{
	public static class LinkedListNodeExtensions
	{
		public static LinkedListNode<T> CircularNext<T>(this LinkedListNode<T> node, LinkedList<T> list)
		{
			return node!.Next ?? list.First!;
		}

		public static LinkedListNode<T> CircularPrev<T>(this LinkedListNode<T> node, LinkedList<T> list)
		{
			return node!.Previous ?? list.Last!;
		}
	}
}