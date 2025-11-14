using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace McDonalds.Views
{
    public partial class ProductSlot : UserControl, INotifyPropertyChanged
    {
        public ProductSlot()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ProductNameProperty =
            DependencyProperty.Register(
                "ProductName",
                typeof(string),
                typeof(ProductSlot),
                new PropertyMetadata("Product"));

        public static readonly DependencyProperty ProductIconProperty =
            DependencyProperty.Register(
                "ProductIcon",
                typeof(string),
                typeof(ProductSlot),
                new PropertyMetadata(null));

        public static readonly DependencyProperty QuantityProperty =
            DependencyProperty.Register(
                "Quantity",
                typeof(int),
                typeof(ProductSlot),
                new PropertyMetadata(0, OnQuantityChanged));

        public static readonly DependencyProperty ProductStateProperty =
            DependencyProperty.Register(
                "ProductState",
                typeof(string),
                typeof(ProductSlot),
                new PropertyMetadata("Ready"));

        public static readonly DependencyProperty IsNewAddedProperty =
            DependencyProperty.Register(
                "IsNewAdded",
                typeof(bool),
                typeof(ProductSlot),
                new PropertyMetadata(false));

        public string ProductName
        {
            get => (string)GetValue(ProductNameProperty);
            set => SetValue(ProductNameProperty, value);
        }

        public string ProductIcon
        {
            get => (string)GetValue(ProductIconProperty);
            set => SetValue(ProductIconProperty, value);
        }

        public int Quantity
        {
            get => (int)GetValue(QuantityProperty);
            set => SetValue(QuantityProperty, value);
        }

        public string ProductState
        {
            get => (string)GetValue(ProductStateProperty);
            set => SetValue(ProductStateProperty, value);
        }

        public bool IsNewAdded
        {
            get => (bool)GetValue(IsNewAddedProperty);
            set => SetValue(IsNewAddedProperty, value);
        }

        private static void OnQuantityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProductSlot slot)
            {
                int oldValue = (int)e.OldValue;
                int newValue = (int)e.NewValue;

                if (newValue > oldValue && oldValue > 0)
                {
                    slot.TriggerAddAnimation();
                }
            }
        }

        private async void TriggerAddAnimation()
        {
            IsNewAdded = true;
            await System.Threading.Tasks.Task.Delay(600);
            IsNewAdded = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}