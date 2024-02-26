using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
// A global data class to store some global data
public class GlobalData : MonoBehaviour
{
	public static GlobalData Instance;
	public TimeTense tt;
    

    
	
	private void Awake()
	{
		Instance = this;
	}


}
