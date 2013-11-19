using UnityEngine;
using System.Collections;

public class Gamemode : MonoBehaviour {
	
	public static Gamemode tdm = new TeamDeathmatch();
	
	public int auScore = 0;
	public int ruScore = 0;
	
	void Start () {
	
	}
	
	void Update () {
	
	}
	
	public void updateScore(int au, int ru, bool minus)
	{
		if(minus == true)
		{
			auScore = auScore - au;
			ruScore = ruScore - ru;
		}
		if(minus == false)
		{
			auScore = auScore + au;
			ruScore = ruScore + ru;
		}
	}
	
	public void finishGame(bool au, bool ru)
	{
		
	}
}
