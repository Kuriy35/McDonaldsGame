using System;
using System.Windows.Input;
using McDonalds.Commands;
using McDonalds.Models.Core;

namespace McDonalds.ViewModels
{
    public class GameSetupViewModel : ViewModelBase
    {
        private GameDifficulty _selectedDifficulty;
        private bool _isGameSetupCompleted;

        public GameDifficulty SelectedDifficulty
        {
            get => _selectedDifficulty;
            set => SetProperty(ref _selectedDifficulty, value);
        }

        public bool IsGameSetupCompleted
        {
            get => _isGameSetupCompleted;
            set => SetProperty(ref _isGameSetupCompleted, value);
        }

        public ICommand StartGameCommand { get; }

        public event Action<GameDifficulty> GameSetupCompleted;

        public GameSetupViewModel()
        {
            SelectedDifficulty = GameDifficulty.Easy;
            IsGameSetupCompleted = false;
            StartGameCommand = new RelayCommand(_ =>
            {
                IsGameSetupCompleted = true;
                GameSetupCompleted?.Invoke(SelectedDifficulty);
            });
        }
    }
}