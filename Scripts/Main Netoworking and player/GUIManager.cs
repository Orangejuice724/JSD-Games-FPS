using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour {
	
	public List<string> WeaponNames = new List<string>();
	public List<string> SightNames = new List<string>();
	public int CurrentWeapon;
	public int LastWeapon;
	public static GUIManager instance;
	public bool ShowBoard;
	
	void Start () {
		instance = this;
		foreach(Sight s in NetworkManager.instance.MyPlayer.manager.FirstPersonCont.Weapons[0].Sights.ToArray())
		{
			SightNames.Add(s.Name);
		}
	}
	
	void Update () {
		if(LastWeapon != CurrentWeapon)
		{
			ChangedGun();
		}
		
		ShowBoard = Input.GetKey(KeyCode.Tab);
	}
	
	void OnGUI()
	{
		if(NetworkManager.instance.MatchStarted == true && !NetworkManager.instance.MyPlayer.isAlive)	
		{
			//CurrentWeapon = GUILayout.SelectionGrid(CurrentWeapon, WeaponNames.ToArray(), 4);
			//NetworkManager.instance.MyPlayer.manager.FirstPersonCont.Weapons[CurrentWeapon].CurSight = GUILayout.SelectionGrid(NetworkManager.instance.MyPlayer.manager.FirstPersonCont.Weapons[CurrentWeapon].CurSight, SightNames.ToArray(), 4);
			/*if(GUI.Button(new Rect(0, 0, 150, 40), "Edit Loadout"))
			{
				Loadout.instance.editLoadout = true;
			}*/
		}
		
		if(NetworkManager.instance.MatchStarted == true && NetworkManager.instance.MyPlayer.isAlive == true)
		{
			if(GUI.Button(new Rect(0, 0, 100, 40), "Suicide"))
			{
				NetworkManager.instance.MyPlayer.manager.networkView.RPC ("Die", RPCMode.All);
			}
		}
		
		if(ShowBoard == true && NetworkManager.instance.MatchStarted == true)
		{
			GUILayout.BeginArea(new Rect(Screen.width / 4, Screen.height / 4, (Screen.width) - (Screen.width / 2), (Screen.height) - (Screen.height / 2)), GUIContent.none, "box");
			
			foreach(Player pl in NetworkManager.instance.PlayerList)
			{
				GUILayout.BeginHorizontal();
				
				GUILayout.Label (pl.PlayerName);
				GUILayout.Label (pl.Score.ToString());
				GUILayout.Label (pl.Kills.ToString());
				GUILayout.Label (pl.Deaths.ToString());
				
				GUILayout.EndHorizontal();
			}
			
			GUILayout.EndArea();
		}
		
		
	}
	
	void ChangedGun()
	{
		SightNames.Clear();
		LastWeapon = CurrentWeapon;
		foreach(Sight s in NetworkManager.instance.MyPlayer.manager.FirstPersonCont.Weapons[CurrentWeapon].Sights.ToArray())
		{
			SightNames.Add(s.Name);
		}
	}
}
