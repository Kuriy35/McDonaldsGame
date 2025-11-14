using McDonalds.Models.Core;
using McDonalds.Models.Orders;
public abstract class KitchenEquipment
{
    public string Name { get; protected set; }
    public string IconPath { get; protected set; }
    public bool IsBusy { get; protected set; }
    public float ProcessingTime { get; protected set; }
    public float CurrentProcessingTime { get; protected set; }
    public float ProcessingProgress { get; set; }
    protected Product CurrentProduct { get; set; }
    public bool HasReadyProduct { get; protected set; }

    protected KitchenEquipment(string name, string iconPath)
    {
        Name = name;
        IconPath = iconPath;
        IsBusy = false;
        ProcessingTime = 5.0f;
        CurrentProcessingTime = 0;
        ProcessingProgress = 0.0f;
        CurrentProduct = null;
        HasReadyProduct = false;
    }

    public abstract bool CanProcess(Product product);
    public abstract void StartProcessing(Product product);
    public abstract void StartProcessing();
    public void UpdateProcessing(float deltaTime)
    {
        if (!IsBusy || CurrentProduct == null)
            return;

        CurrentProcessingTime += deltaTime;
        if (CurrentProcessingTime >= ProcessingTime)
        {
            CompleteProcessing();
        }
        else
        {
            ProcessingProgress = CurrentProcessingTime / ProcessingTime;
        }
    }

    protected virtual void CompleteProcessing()
    {
        if (CurrentProduct != null)
        {
            CurrentProduct.State = ProductState.Ready;
            HasReadyProduct = true;
        }

        IsBusy = false;
        CurrentProcessingTime = 0;
        ProcessingProgress = 1.0f;
    }

    public virtual Product TakeReadyProduct()
    {
        if (HasReadyProduct && CurrentProduct != null && CurrentProduct.State == ProductState.Ready)
        {
            Product readyProduct = CurrentProduct;
            CurrentProduct = null;
            HasReadyProduct = false;
            ProcessingProgress = 0.0f;

            ResourceManager.Instance.ConsumeResources(readyProduct);
            return readyProduct;
        }

        return null;
    }
}