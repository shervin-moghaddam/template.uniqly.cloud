using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace robotmanden.Code
{

    public class AsyncTaskHelperClass
    {
        /// <summary>
        /// Will return exceptions in case there are any!
        /// Example:
        /// var result = await AsyncTaskHelperClass.WhenAll(ListOfTasks.Task);
        /// will now throw all exceptions and prevent task.whenall from hanging and waiting for finish
        /// </summary>
        /// <param name="tasks"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
        /// <exception cref="Exception"></exception>
        public static async Task<IEnumerable<T>> WhenAll<T>(params Task<T>[] tasks)
        {
            var allTasks = Task.WhenAll(tasks);

            try
            {
                return await allTasks;
            }
            catch (Exception)
            {
                // ignore
            }

            throw allTasks.Exception ?? throw new Exception("AsyncTaskHelperClass threw one or more errors");
        }
    }
}