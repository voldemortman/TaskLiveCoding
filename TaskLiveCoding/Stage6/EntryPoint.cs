using System.Threading.Channels;
using System.Threading;

namespace TaskLiveCoding.Stage6
{
    public class EntryPoint
    {
        private readonly Channel<int> _channel;

        public EntryPoint()
        {
            _channel = Channel.CreateUnbounded<int>();
        }

        public void main()
        {
            var channelReader = _channel.Reader;
            var channelWriter = _channel.Writer;
            var writer = new Writer(channelWriter);
            var reader = new Reader(channelReader);
            var cts = new CancellationTokenSource();
            reader.ConsoleWriteStatus(writer, cts.Token).GetAwaiter().GetResult();
        }
    }
}
