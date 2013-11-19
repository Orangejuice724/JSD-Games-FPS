// using UnityEngine = Is saying that we want to use the unity engine, and all of its ability
using UnityEngine;
// using System.Collections = We're not importing System, we're importing the Collections inside of the System file
using System.Collections;

//public class Gamemode : MonoBehaviour
// public = is accessable to every script
// class = creating the brain to a human body
// Gamemode giving a name to the brain
// : Monobehaviour (Make sure you add the :(: = extend) otherwise you'll create an error) MonoBehaviour
// Monobehaviour, You don't really need to understand what MonoBehaviour is... Generally all of your classes are extending (:)
// Monobehaviour,  MonoBehaviour is just the core code for c#
public class Gamemode : MonoBehaviour {
	
	//I'll explain this after, but essentially for every gamemode we make, we want to create an instance of it here.
	public static Gamemode tdm = new TeamDeathmatch();
	
	//All variables(varibles?) will be added here so they can be accessed by the child classes (Will explain child class soon)
	//public (Any script can access it (private no script can access it))
	//When you want to create a number variable, there are four things you can use, Int, Float, Double and long
	// int = a number without a decimal point e.g 5
	// float = a number with decimal points (theres a maximum but cant thing of the top of my head haha) 5.546
	// Double = a number with two decimal places (generally a rounded float, or long) e.g 5.55
	// Long (dont really use this unless you are creating time) a number with an extremely long decimal point, e.g 5.234664234562345554
	//what we are doing
	// we create a public variable, make it an int, give it a name, and set the default value to be 0 then close
	// the statement with ;
	public int auScore = 0;
	public int ruScore = 0;
	public int gameTime = 0;
	
	//this is a constructor. it can be public, private or nothing, I choose nothing for Start and Update, as those are
	// generic and should never be called from any class. 
	// a constructor is what actually holds the code, and makes the code do stuff. Without a constructor, the code wont
	//execute
	//a void is something that doesn't return anything. so if we did bool, it would either return true or false
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
