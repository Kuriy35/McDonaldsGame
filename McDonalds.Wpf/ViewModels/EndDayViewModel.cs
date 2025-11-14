using System.Windows;
using System.Windows.Input;
using McDonalds.Commands;

namespace McDonalds.ViewModels
{
    public class EndDayViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        public bool GoalsReached { get; }
        public double Money { get; }
        public double Satisfaction { get; }
        public string Message { get; }

        public ICommand RestartCommand { get; }
        public ICommand ExitCommand { get; }

        public EndDayViewModel(MainViewModel mainViewModel, bool goalsReached, double money, double satisfaction)
        {
            _mainViewModel = mainViewModel;
            GoalsReached = goalsReached;
            Money = money;
            Satisfaction = satisfaction;
            Message = goalsReached ? "Вітаємо! Цілі досягнуто!" : "На жаль, не всі цілі досягнуто. Спробуйте ще!";

            RestartCommand = new RelayCommand(Restart);
            ExitCommand = new RelayCommand(Exit);
        }

        private void Restart(object param)
        {
            _mainViewModel.ActivateSetupViewModel();
        }

        private void Exit(object param)
        {
            Application.Current.Shutdown();
        }
    }
}