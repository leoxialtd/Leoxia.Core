using System;
using System.Reflection;
using System.Threading;

namespace Leoxia.Threading
{
    public static class ThreadExtensions
    {
        public static void Abort(this Thread thread)
        {
            //MethodInfo abort = null;
            //foreach (MethodInfo m in thread.GetType().GetTypeInfo().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
            //{
            //    if (m.Name.Equals("AbortInternal") && m.GetParameters().Length == 0) abort = m;
            //}
            //if (abort == null)
            //{
            //    throw new Exception("Failed to get Thread.Abort method");
            //}
            //abort.Invoke(thread, new object[0]);
        }
    }
}
