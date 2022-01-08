using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskLiveCoding.Stage3
{
    public class EntryPoint
    {
        public async Task main()
        {
            await foreach(int status in ReturnMultipleValues())
            {
                Console.WriteLine(status);
            }
        }

        public async IAsyncEnumerable<int> ReturnMultipleValues()
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(500);
                yield return 1;
            }
        }
    }
}
