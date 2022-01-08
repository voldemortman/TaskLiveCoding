using System;
using System.Threading.Tasks;

namespace TaskLiveCoding.Stage2
{
    public class AsyncAllTheWay
    {
        public void EntryPoint()
        {
            try
            {
                NonAsyncCode();
            }
            catch (Exception e)
            {
                e.GetType();
            }
        }

        public async void NonAsyncCode()
        {
            try
            {
                await AsyncCode();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());
                throw;
            }
        }

        public async Task AsyncCode()
        {
            await Task.Delay(500);
            throw new OperationCanceledException();
        }
    }
}
