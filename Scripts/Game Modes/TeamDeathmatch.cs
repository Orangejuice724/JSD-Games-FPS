using UnityEngine;
using System.Collections;

public class TeamDeathmatch : Gamemode {
	
	public int maxDeaths = 300;
	public int AUDeaths = 0;
	public int RUDeaths = 0;
	
	public TeamDeathmatch() : base()
	{
		
	}
	
	void Start () {
		auScore = maxDeaths;
		ruScore = maxDeaths;
	}
	
	void Update () {
		/*if(Input.GetKeyDown(KeyCode.A))
		{
			auDeath();
		}
		if(Input.GetKeyDown(KeyCode.R))
		{
			ruDeath();
		}*/
	}
	
	public void auWon()
	{
		finishGame(true, false);
	}
	
	public void ruWon()
	{
		finishGame(false, true);
	}
	
	public void auDeath()
	{
		updateScore(1, 0, true);
	}
	
	public void ruDeath()
	{
		updateScore(0, 1, true);
	}
}
