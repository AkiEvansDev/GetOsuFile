using System;
using System.Threading;

namespace GetOsuFile
{
    public static class AsyncWork
    {
        public static void AsyncAction(Action action, Action complite)
        {
            Thread thread = new Thread(
                delegate ()
                {
                    action();
                    complite();
                })
            {
                IsBackground = true
            };
            thread.Start();
        }
    }
}
