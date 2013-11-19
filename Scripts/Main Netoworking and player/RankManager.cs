using UnityEngine;
using System.Collections;

public class RankManager : MonoBehaviour {

	public int CurLevel;
	public int NextLevel;
	public int ExpToLevel;
	public int Exp;
	public static RankManager instance;
	
	void Start () {
		Exp = PlayerPrefs.GetInt("XP");
		NextLevel = CurLevel + 1;
		instance = this;
	}
	
	void Update () {
		if(Exp >= ExpToLevel)
		{	
			ExpToLevel *= 2;
			CurLevel++;
			NextLevel++;
			PlayerPrefs.SetInt("XP", Exp);
		}
	}
}
