namespace McDonalds.Models.Customers
{
    public interface IPatienceStrategy
    {
        float CalculateBasePatience();
        bool IsSatisfied(float waitTime, float patience);
    }
}