using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Threading;

namespace TaskLiveCoding.Stage6
{
    public class Writer
    {
        private readonly ChannelWriter<int> _writer;

        public Writer(ChannelWriter<int> writer)
        {
            _writer = writer;
        }

        public async Task TaskWithStatus(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                for (int i = 0; i < 100; i += 10)
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
