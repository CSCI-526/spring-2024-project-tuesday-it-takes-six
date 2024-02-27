using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
// A global data class to store some global data
public class GlobalData : MonoBehaviour
{
	public static GlobalData Instance;
	public TimeTense tt;

	public bool playerDied;
    
	
	private void Awake()
	{
		Instance = this;
		this.playerDied = false;
	}


}
