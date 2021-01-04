using System;
using System.Threading.Tasks;

namespace Picro.Common.Extensions
{
    public static class ActionExtensions
    {
        public static Func<Task>? MakeTaskCompatible(this Action? action)
        {
            if (action == null)
            {
                return null;
            }

            return () =>
            {
                action();
                return Task.CompletedTask;
            };
        }

        public static Func<T, Task>? MakeTaskCompatible<T>(this Action<T>? action)
        {
            if (action == null)
            {
                return null;
            }

            return x =>
            {
                action(x);
                return Task.CompletedTask;
            };
        }
    }
}