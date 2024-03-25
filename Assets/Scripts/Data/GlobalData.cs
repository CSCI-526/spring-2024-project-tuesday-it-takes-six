using System;


// wrap in game namespace to prevent naming pollution
namespace Game
{
    public static class GlobalData
    {
        // please DO NOT leave heavy logics inside this class
        // if you must, create a data manager class instead

        // please use RECOGNIZABLE NAME for variables in this class

        public static TimeTenseDataManager TimeTenseData = new();

        public static PlayerStatusDataManager PlayerStatusData = new();
        public static CheckPointDataManager CheckPointData = new();
        public static LevelDataManager LevelData = new();

        public static void Init()
        {
            TimeTenseData.Init();
            PlayerStatusData.Init();
            CheckPointData.Init();
        }

        // Analytics
        public readonly static long _sessionID = DateTime.Now.Ticks;
        public static int numberEnemiesKilled = 0;
        public static int numberOfTimeSwitches = 0;
    }


    public static class Env
    {
        #if UNITY_EDITOR
            public static bool isDebug = true;
        #else
            public static bool isDebug = false;
        #endif
    }
}
