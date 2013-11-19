using UnityEngine;
using System.Collections;
using System;

public class NetworkAnimStates : MonoBehaviour {

    public Animations CurrentAnim = Animations.idle;
    public GameObject ThirdPersonPlayer;
	
	void Start () {
	
	}
	
	void Update () {
		ThirdPersonPlayer.animation.CrossFade(Enum.GetName(typeof(Animations), CurrentAnim));
	}

    public void SyncAnimations(string AnimName, float speed)
    {
        CurrentAnim = (Animations)Enum.Parse(typeof(Animations), AnimName);
		animation[CurrentAnim.ToString()].speed = speed;
    }
}

public enum Animations
{
    idle,
    walk,
    run,
    jump_pose
}
