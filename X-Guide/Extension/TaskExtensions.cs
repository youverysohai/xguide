using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace X_Guide.Extension
{
    public static class TaskExtensions
    {
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var completedTask = await Task.WhenAny(task, Task.Delay(-1, cancellationToken));
            cancellationToken.ThrowIfCancellationRequested();
            return await task;
        }
    }
}
