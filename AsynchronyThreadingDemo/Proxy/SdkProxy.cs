using AsynchronyThreadingDemo.Model;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace AsynchronyThreadingDemo.Proxy
{
    public class SdkProxy
    {
        #region Fields

        private ConcurrentDictionary<Guid, TaskCompletionSource<bool>> _pendingOrders;
        private ThirdPartySdkMock _sdkMock;

        #endregion Fields

        #region Constructor

        public SdkProxy()
        {
            _pendingOrders = new ConcurrentDictionary<Guid, TaskCompletionSource<bool>>();

            _sdkMock = new ThirdPartySdkMock();
            _sdkMock.OnOrderCompleted += OnOrderCompleted;
        }

        #endregion Constructor

        #region Methods

        public Task SubmitOrderAsync(decimal price)
        {
            Guid orderId = _sdkMock.SubmitOrder(price);
            Console.WriteLine($"OrderId {orderId} submitted with price {price}");

            var tcs = new TaskCompletionSource<bool>();
            _pendingOrders.TryAdd(orderId, tcs);

            return tcs.Task;
        }

        private void OnOrderCompleted(object sender, OrderOutcome e)
        {
            string successStr = e.Success ? "was successful" : "failed";
            Console.WriteLine($"OrderId {e.OrderId} {successStr}");

            _pendingOrders.TryRemove(e.OrderId, out var tcs);
            tcs.SetResult(e.Success);
        }

        #endregion Methods
    }
}
