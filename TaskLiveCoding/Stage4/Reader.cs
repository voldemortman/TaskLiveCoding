using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TaskLiveCoding.Stage4
{
    public class Reader
    {
        private readonly ChannelReader<int> _reader;

        public Reader(ChannelReader<int> reader)
        {
            _reader = reader;
        }

        public async Task ConsoleWriteStatus(Writer writer)
        {
            var writerTask = writer.TaskWithStatus();
            var readerCompletion = _reader.Completion;
            try
            {
                while (!writerTask.IsCompletedSuccessfully && !readerCompletion.IsCompleted)
                {
                    var status = await _reader.ReadAsync();
                    Console.WriteLine(status);
                    // ============ The correct way ============
                    //if (_reader.TryRead(out int status))
                    //{
                    //    Console.WriteLine(status);
                    //}
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
