using UnityEngine;
using System.Collections;

public class NetworkAnimController : MonoBehaviour {

	public float V;
	public float H;
	public NetworkAnimStates States;
	
	void Start () {
		
	}
	
	void Update () {
		V = Input.GetAxis("Vertical");
		H = Input.GetAxis("Horizontal");
		
		if(V < 0 && NetworkManager.instance.MyPlayer.manager.walkingstate == WalkingState.Walking)
		{
			States.SyncAnimations("walk", 1);
		}
		else if(V > 0 && NetworkManager.instance.MyPlayer.manager.walkingstate == WalkingState.Walking){
			States.SyncAnimations("walk", -1);
		}
		if(V < 0 && NetworkManager.instance.MyPlayer.manager.walkingstate == WalkingState.RunningPrimary)
		{
			States.SyncAnimations("run", 1.2F);
		}
		else if(V > 0 && NetworkManager.instance.MyPlayer.manager.walkingstate == WalkingState.RunningPrimary){
			States.SyncAnimations("run", -1.2F);
		}
		else{
			States.SyncAnimations("idle", 1);
		}
	}
}
