using UnityEngine;
using System.Collections;

// so here we create the class. But instead of extending MonoDevelop, you extend Gamemode.. So the class Gamemode.
//when you extend a class you get all of the functions(constructors) that are in the class you extend.
// so as an example this is the child class, and Gamemode is the parents class. Everytime you call a function in here, thats
// in the gamemode class, its like taking money from your parents.. (shitty metaphor/simile)
public class TeamDeathmatch : Gamemode {
	
	public int maxDeaths = 300;
	public int AUDeaths = 0;
	public int RUDeaths = 0;
	public int maxTime = 3000;
	
	// we create this, so we can create this class in the Gamemode class. we add : base() because we want to reffer to the
	// Gamemode class. so if we do this, so we say base, which means the gamemode.. base(maxTime max time is an int.)
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
