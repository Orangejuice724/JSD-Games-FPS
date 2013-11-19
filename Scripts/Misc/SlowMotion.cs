using UnityEngine;
using System.Collections;

public class SlowMotion : MonoBehaviour {

	public int SetSlomoSpeed = 0;
 
	// Use this for initialization
	void Start () {
	 
	}
	 
	// Update is called once per frame
	void Update () {
	 
	    if(Input.GetKeyDown(KeyCode.LeftShift))
	    	Time.timeScale = SetSlomoSpeed + .3f;
	 
	    if(Input.GetKeyUp(KeyCode.LeftShift))
	        Time.timeScale = 1f; 
	}
}
