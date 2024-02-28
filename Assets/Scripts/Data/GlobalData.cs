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
	}
}
