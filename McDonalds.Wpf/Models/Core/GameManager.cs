using McDonalds.Models.Factory;
using McDonalds.Models.Orders;

namespace McDonalds.Models.Core
{

    public class GameManager
    {
        private static GameManager _instance;
        private static readonly object _lock = new object();
        private ProductFlyweightFactory _flyweightFactory;

        public GameDifficulty CurrentDifficulty { get; private set; }
        public Restaurant.Restaurant CurrentRestaurant { get; private set; }
        public double Money { get; private set; }
        public TimeSpan WorkdayDuration { get; private set; }
        public TimeSpan CurrentTime { get; private set; }
        public bool IsGameRunning { get; private set; }
        public GameGoals CurrentGoals { get; private set; }
        public int SatisfiedCustomersCount { get; private set; }
        public int TotalCustomersCount { get; private set; }
        public double CustomersSatisfaction { get; private set; }
        public ProductFlyweightFactory FlyweightFactory => _flyweightFactory;
        private GameManager()
        {
            Money = 0;
            CurrentTime = TimeSpan.Zero;
            IsGameRunning = false;
            _flyweightFactory = new ProductFlyweightFactory();
        }

        public static GameManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GameManager();
                    }
                    return _instance;
                }
            }
        }

        public void UpdateBalance(double money)
        {
            Money += money;
        }

        public void CalculateCustomersSatisfaction()
        {
            if (TotalCustomersCount == 0)
                CustomersSatisfaction = 0;
            else
                CustomersSatisfaction = (float)SatisfiedCustomersCount / TotalCustomersCount;
        }

        public void UpdateCustomersCount(int satisfiedCustomersCount, int totalCustomersCount)
        {
            SatisfiedCustomersCount = satisfiedCustomersCount;
            TotalCustomersCount = totalCustomersCount;
        }

        public void StartGame(GameDifficulty difficulty)
        {
            CurrentDifficulty = difficulty;
            DifficultyFactory factory = DifficultyFactory.GetFactory(difficulty);

            CurrentRestaurant = factory.CreateRestaurant();
            CurrentGoals = factory.CreateGameGoals();

            WorkdayDuration = CurrentGoals.WorkdayDuration;
            CurrentTime = TimeSpan.Zero;
            Money = 0;
            SatisfiedCustomersCount = 0;
            TotalCustomersCount = 0;
            CustomersSatisfaction = 0;

            IsGameRunning = true;
        }

        public void UpdateGame(TimeSpan deltaTime)
        {
            if (!IsGameRunning) return;

            CurrentTime += deltaTime;

            if (CurrentTime >= WorkdayDuration)
            {
                EndGame();
            }
        }

        public bool EndGame()
        {
            IsGameRunning = false;
            return AreGoalsReached();
        }

        public bool AreGoalsReached()
        {
            if (CurrentGoals == null) return false;

            float satisfactionRate = TotalCustomersCount > 0
                ? (float)CustomersSatisfaction * 100 / TotalCustomersCount
                : 0;

            return Money >= CurrentGoals.MinimumMoney &&
                   satisfactionRate >= CurrentGoals.MinimumSatisfaction;
        }
    }

    public class GameGoals
    {
        public int MinimumSatisfaction { get; set; }
        public double MinimumMoney { get; set; }
        public TimeSpan WorkdayDuration { get; set; }
    }
}