using System.Windows.Input;
using McDonalds.Commands;

namespace McDonalds.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        private GameSetupViewModel _gameSetupViewModel;
        private GameViewModel _gameViewModel;

        public MainViewModel()
        {
            _gameSetupViewModel = new GameSetupViewModel();
            _gameViewModel = new GameViewModel(this);

            CurrentViewModel = _gameSetupViewModel;

            ActivateSetupViewModel();
        }

        public void ActivateSetupViewModel()
        {
            _gameSetupViewModel.GameSetupCompleted += (difficulty) =>
            {
                _gameViewModel.StartNewGame(difficulty);
                CurrentViewModel = _gameViewModel;
            };
            CurrentViewModel = _gameSetupViewModel;
        }
    }
}