using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// wrap in game namespace to prevent naming pollution
namespace Game
{
    public static class GlobalData
    {
        // please make all variables in data manager PRIVATE

        // please DO NOT leave heavy logics inside this class
        // if you must, create a data manager class instead

        // please use RECOGNIZABLE NAME for variables in this class


        public static TimeTenseDataManager TimeTenseData = new TimeTenseDataManager();

        public static bool playerDied = false;

        // Analytics
        public static int numberEnemiesKilled = 0;
        public static int numberOfTimeSwitches = 0;

		private static bool _hasReachedCheckpoint = false;
		private static Vector2 _lastCheckpointPosition;
        private static string _currentSceneName = "";

        public static string CurrentSceneName
        {
            get { return _currentSceneName; }
            set { _currentSceneName = value; }
        }


        // Public accessor for hasReachedCheckpoint
        public static bool HasReachedCheckpoint
        {
            get { return _hasReachedCheckpoint; }
            set { _hasReachedCheckpoint = value; }
        }

        // Public accessor for lastCheckpointPosition
        public static Vector2 LastCheckpointPosition
        {
            get { return _lastCheckpointPosition; }
            set { _lastCheckpointPosition = value; }
        }
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
