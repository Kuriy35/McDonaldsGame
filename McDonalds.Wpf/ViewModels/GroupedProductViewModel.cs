using McDonalds.Models.Orders;

namespace McDonalds.ViewModels
{
    public class GroupedProductViewModel : ViewModelBase
    {
        private string _name;
        private int _count;
        private string _state;
        private string _iconPath;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        public string State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        public string IconPath
        {
            get => _iconPath;
            set => SetProperty(ref _iconPath, value);
        }

        public GroupedProductViewModel(string name, int count, ProductState state, string iconPath)
        {
            _name = name;
            _count = count;
            _state = state.ToString();
            _iconPath = iconPath;
        }
    }
}