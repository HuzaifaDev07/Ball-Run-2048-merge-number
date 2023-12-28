using System.Collections.Generic;

namespace ArcadeIdle
{
    public class PlayerData
    {
        //Here you can save all the data you need
        //For example
        //public int LevelId;
        public bool Sounds = true;
        public bool Vibration = true;

        public bool GameStartPriceGiven = false;
        public bool PickupBallsShown = false;
        public bool PickUpPackage = false;
        public bool UnlockCounter = false;
        public bool ShownCounter = false;
        public bool AddBallsToMachine = false;
        public bool AddPackagesToGateway = false;

        public bool CricketBallMachineShown = false;
        public bool FootBallMachineShown = false;
        public bool BasketBallMachineShown = false;
        

        public int[] ResourcesCounts;

        public int PlayerCapacityLevel;

        public int EmployeeCount;

        public int BurgerFactoryCapacity;

        public int CounterCapacity;

        public int TutorialNumber = 0;

        public List<UnlockableItemData> UnlockableItemsData = new List<UnlockableItemData>();
        public List<UpgradableItemData> UpgradableItemsData = new List<UpgradableItemData>();
    }
}