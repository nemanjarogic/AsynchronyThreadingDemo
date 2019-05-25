using System;
using System.Threading.Tasks;

namespace AsynchronyThreadingDemo.Model
{
    /// <summary>
    /// One scenario where TaskCompletionSource can be extremely well-suited is when you are given a third-party SDK which exposes events.
    /// </summary>
    public class ThirdPartySdkMock
    {
        public event EventHandler<OrderOutcome> OnOrderCompleted;

        /// <summary>
        /// SubmitOrder does not return any form of Task, and we can’t await it. 
        /// This doesn’t necessarily mean that it’s blocking. It might be using another form of asynchrony
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public Guid SubmitOrder(decimal price)
        {
            Guid orderId = Guid.NewGuid();

            // do a REST call over the network or something similar
            Task.Delay(3000).ContinueWith(task => OnOrderCompleted?.Invoke(this, new OrderOutcome(orderId, true)));

            return orderId;
        }
    }
}
