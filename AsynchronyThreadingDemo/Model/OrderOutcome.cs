using System;

namespace AsynchronyThreadingDemo.Model
{
    public class OrderOutcome
    {
        public Guid OrderId { get; set; }
        public bool Success { get; set; }

        public OrderOutcome(Guid orderId, bool success)
        {
            this.OrderId = orderId;
            this.Success = success;
        }
    }
}
