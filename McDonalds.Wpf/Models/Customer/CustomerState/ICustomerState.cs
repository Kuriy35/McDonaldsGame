namespace McDonalds.Models.Customers
{
    public enum CustomerState
    {
        InQueue,
        Ordering,
        WaitingOrder,
        Eating,
        Leaving
    }

    public interface ICustomerState
    {
        void Handle(Customer customer, float deltaTime);
        CustomerState Type { get; }
    }

    public abstract class CustomerStateBase : ICustomerState
    {
        public abstract CustomerState Type { get; }

        public virtual void Handle(Customer customer, float deltaTime)
        {
            customer.CurrentWaitTime += deltaTime;
            customer.WaitTimeProgress = customer.Patience > 0
                ? (customer.CurrentWaitTime / customer.Patience)
                : 0.0f;
        }
    }
}