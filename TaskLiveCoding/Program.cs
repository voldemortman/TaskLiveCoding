using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TaskLiveCoding
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Foo().Wait();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.GetType());
            }
        }

        static async Task Foo()
        {
            var channel = Channel.CreateUnbounded<int>();
            var reader = channel.Reader;
            var taskWithProgress = new TaskWithProgress(channel);
            var cts = new CancellationTokenSource();
            cts.CancelAfter(3500);
            var t = taskWithProgress.DoSomethingAsync(cts.Token);
            var readerCompletion = reader.Completion;
            try
            {
                while (!t.IsCompletedSuccessfully && !readerCompletion.IsCompleted)
                {
                    if (reader.TryRead(out int status))
                    {
                        Console.WriteLine(status);
                    }
                }
                await t;
            }
            catch (ChannelClosedException e)
            {
                Console.WriteLine("channel closed:" + e.GetType());
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine("operation canceld:" + e.GetType());
                throw;
            }
        }
    }
}
