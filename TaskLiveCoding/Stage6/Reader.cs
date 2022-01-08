using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TaskLiveCoding.Stage6
{
    public class Reader
    {
        private readonly ChannelReader<int> _reader;

        public Reader(ChannelReader<int> reader)
        {
            _reader = reader;
        }

        public async Task TimeoutWrite(Writer writer)
        {
            var cts = new CancellationTokenSource();
            var writeStatusTask = ConsoleWriteStatus(writer, cts.Token);
            var t = await Task.WhenAny(writeStatusTask, Task.Delay(500));
            if (t == writeStatusTask)
            {
                Console.WriteLine("Task Completed");
            }
            else
            {
                Console.WriteLine("Task Did not complete");
            }
        }

        public async Task ConsoleWriteStatus(Writer writer, CancellationToken token)
        {
            var internalSource = new CancellationTokenSource();
            var combinedSource = CancellationTokenSource.CreateLinkedTokenSource(token, internalSource.Token);
            var writerTask = writer.TaskWithStatus(combinedSource.Token);
            var readerCompletion = _reader.Completion;
            try
            {
                while (!writerTask.IsCompletedSuccessfully && !readerCompletion.IsCompleted)
                {
                    if (_reader.TryRead(out int status))
                    {
                        Console.WriteLine(status);
                    }
                }
                await writerTask;
            }
            catch (ChannelClosedException)
            {
                Console.WriteLine("Channel closed");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation cancelled");
            }
        }
    }
}
