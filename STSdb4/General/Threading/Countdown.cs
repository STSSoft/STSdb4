using System.Threading;

namespace STSdb4.General.Threading
{
    public class Countdown
    {
        private long count; 
        
        public void Increment()
        {
            Interlocked.Increment(ref count);
        }

        public void Decrement()
        {
            Interlocked.Decrement(ref count);
        }

        public void Wait()
        {
            SpinWait wait = new SpinWait();

            wait.SpinOnce();

            while (Count > 0)
                Thread.Sleep(1);
        }

        public long Count
        {
            get { return Interlocked.Read(ref count); }
        }
    }
}
