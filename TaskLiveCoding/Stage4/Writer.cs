using System.Threading.Channels;
using System.Threading.Tasks;

namespace TaskLiveCoding.Stage4
{
    public class Writer
    {
        private readonly ChannelWriter<int> _writer;

        public Writer(ChannelWriter<int> writer)
        {
            _writer = writer;
        }

        public async Task TaskWithStatus()
        {
            try
            {
                for (int i = 0; i < 100; i += 10)
                {
                    _writer.TryWrite(i);
                    await Task.Delay(500);
                    //throw new OperationCanceledException();
                }
            }
            finally
            {
                // This is after you demonstrate channels
                _writer.Complete();
            }
        }
    }
}
