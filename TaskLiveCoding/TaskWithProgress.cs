using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TaskLiveCoding
{
    public class TaskWithProgress
    {
        private readonly ChannelWriter<int> _writer;
        public TaskWithProgress(ChannelWriter<int> writer)
        {
            _writer = writer;
        }

        public async Task DoSomethingAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                for (int i = 0; i <= 100; i += 10)
                {
                    _writer.TryWrite(i);
                    await Task.Delay(500);
                    token.ThrowIfCancellationRequested();
                }
            }
            finally
            {
                _writer.Complete();
            }
        }
    }
}
