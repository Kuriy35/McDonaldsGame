using System.Windows;
using System.Windows.Input;
using McDonalds.Models.Restaurant;

namespace McDonalds.ViewModels
{
    public class TableViewModel : ViewModelBase
    {
        private Table _table;
        private CustomerViewModel _currentCustomer;

        public Table Table => _table;

        public bool IsOccupied
        {
            get => _table.IsOccupied;
            private set
            {
                if (_table.IsOccupied != value)
                {
                    if (value)
                    {
                        _table.OccupyTable(_currentCustomer?.Customer);
                    }
                    else
                    {
                        _table.FreeTable();
                    }
                    OnPropertyChanged();
                }
            }
        }

        public CustomerViewModel CurrentCustomer
        {
            get => _currentCustomer;
            set
            {
                if (_currentCustomer != value)
                {
                    _currentCustomer = value;
                    IsOccupied = value != null;
                    OnPropertyChanged();
                }
            }
        }

        public Point TablePosition => _table.TablePosition;

        public TableViewModel(Table table)
        {
            _table = table;
        }

        public void FreeTable()
        {
            CurrentCustomer = null;
            IsOccupied = false;
        }

        public void UpdateTable()
        {
            if (_table.IsOccupied != IsOccupied)
            {
                OnPropertyChanged(nameof(IsOccupied));

                if (!_table.IsOccupied && _currentCustomer != null)
                {
                    CurrentCustomer = null;
                }
            }
        }
    }
}