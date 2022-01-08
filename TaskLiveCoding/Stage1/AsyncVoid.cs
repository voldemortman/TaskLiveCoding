using System;
using System.Threading.Tasks;

namespace TaskLiveCoding.Stage1
{
    public class AsyncVoid
    {
        public async void ReturnVoid()
        {
            await Task.Delay(500);
            throw new OperationCanceledException();
        }
    }
}
