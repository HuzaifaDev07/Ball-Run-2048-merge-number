namespace ArcadeIdle
{
    public static class Constants
    {
        //Here we keep all public static data, which you might need somewhere
        public const string PLAYER_TAG = "Player";
        public const string DATA_KEY = "data_key";
        
        public const string SETTINGS = "Settings";
    }

    public enum AnimationType
    {
        PickItem, DropItem, None, Sale,
    }
    public enum AnimationState
    {
        Running, Complete,
    }
    public enum CustomerHungerStatus
    {
        Hungry, Full, Holding,
    }
    public enum OrderStatus
    {
        Empty, Full,
    }
    public enum TableStatus
    {
        Vaccant,Occupied,
    }
    public enum TrashType
    {
        Dustbin,TrashBox,
    }
    public enum ChairStatus
    {
        Vaccant, Occupied,Trashed,
    }
    public enum MoneyAtPlace
    {
        MainDoor,CashCounter,EatingTable,
    }
    public enum DoorState
    {
        Open,Close
    }
    public enum UnlockItemAt
    {
        MainDoor, Table, BurgerMachine,Counter,Cashier,HR_Office,Upgrade_Office,
    }
    public enum EnteringFrom
    {
        Inside,Outside,ToClose,
    }
}
