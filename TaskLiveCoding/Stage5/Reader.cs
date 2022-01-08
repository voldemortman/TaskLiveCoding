using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TaskLiveCoding.Stage5
{
    public class Reader
    {
        private readonly ChannelReader<int> _reader;

        public Reader(ChannelReader<int> reader)
        {
            _reader = reader;
        }

        // ======================== before ========================
        public async Task ConsoleWriteStatus(Writer writer)
        {
            var tokenSource = new CancellationTokenSource();
            var writerTask = writer.TaskWithStatus(tokenSource.Token);
            var readerCompletion = _reader.Completion;
            try
            {
                while (!writerTask.IsCompletedSuccessfully && !readerCompletion.IsCompleted)
                {
                    if (_reader.TryRead(out int status))
                    {
                        Console.WriteLine(status);
                        tokenSource.Cancel();
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

        // ======================== after ========================
        //public async Task ConsoleWriteStatus(Writer writer, CancellationToken token)
        //{
        //    var internalSource = new CancellationTokenSource();
        //    var combinedSource = CancellationTokenSource.CreateLinkedTokenSource(token, internalSource.Token);
        //    var writerTask = writer.TaskWithStatus(combinedSource.Token);
        //    var readerCompletion = _reader.Completion;
        //    try
        //    {
        //        while (!writerTask.IsCompletedSuccessfully && !readerCompletion.IsCompleted)
        //        {
        //            if (_reader.TryRead(out int status))
        //            {
        //                Console.WriteLine(status);
        //                combinedSource.Cancel();
        //            }
        //        }
        //        await writerTask;
        //    }
        //    catch (ChannelClosedException)
        //    {
        //        Console.WriteLine("Channel closed");
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        Console.WriteLine("Operation cancelled");
        //    }
        //}
    }
}
