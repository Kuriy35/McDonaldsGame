namespace McDonalds.Models.Customers
{
    public class InQueueState : CustomerStateBase
    {
        public override CustomerState Type => CustomerState.InQueue;

        public override void Handle(Customer customer, float deltaTime)
        {
            base.Handle(customer, deltaTime);

            if (customer.WaitTimeProgress > 1.0f)
            {
                customer.State = new LeavingState();
            }
        }
    }

    public class OrderingState : CustomerStateBase
    {
        public override CustomerState Type => CustomerState.Ordering;

        public override void Handle(Customer customer, float deltaTime)
        {
            base.Handle(customer, deltaTime);

            if (customer.CurrentOrder != null)
            {
                customer.State = new WaitingOrderState();
            }
        }
    }

    public class WaitingOrderState : CustomerStateBase
    {
        public override CustomerState Type => CustomerState.WaitingOrder;

        public override void Handle(Customer customer, float deltaTime)
        {
            base.Handle(customer, deltaTime);

            if (customer.WaitTimeProgress > 1.0f)
            {
                customer.State = new LeavingState();
            }
        }
    }

    public class EatingState : CustomerStateBase
    {
        public override CustomerState Type => CustomerState.Eating;

        public override void Handle(Customer customer, float deltaTime)
        {
            customer.CurrentWaitTime += deltaTime;

            float eatingTime = 3.0f + (customer.CurrentOrder?.Products.Count * 3.0f ?? 0);
            customer.WaitTimeProgress = customer.CurrentWaitTime / eatingTime;

            if (customer.WaitTimeProgress > 1.0f)
            {
                customer.State = new LeavingState();
            }
        }
    }

    public class LeavingState : CustomerStateBase
    {
        public override CustomerState Type => CustomerState.Leaving;

        public override void Handle(Customer customer, float deltaTime)
        {
        }
    }
}