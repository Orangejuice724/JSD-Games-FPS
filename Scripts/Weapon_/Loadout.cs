using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Loadout : MonoBehaviour {
	
	public static Loadout instance;
	
	public bool editLoadout = false;
	public bool showPrimary = false;
	public bool showPrimarySlot1 = false;
	public bool showSecondary = false;
	public List<string> PrimaryWeapons = new List<string>();
	public List<string> SecondaryWeapons = new List<string>();
	public List<string> SightNames = new List<string>();
	public int CurrentPrimary;
	public int CurrentPrimarySlot1;
	public int ActualPrimary;
	public int LastPrimary;
	public int CurrentSecondary;
	public int LastSecondary;
	
	void Start () {
		foreach(Sight s in NetworkManager.instance.MyPlayer.manager.FirstPersonCont.Weapons[0].Sights.ToArray())
		{
			SightNames.Add(s.Name);
		}
		
		instance = this;
		
		LastPrimary = CurrentPrimary;
	}
	
	void Update () {
		//This stuff is because I couldn't think of something to do to replace it
		if(CurrentPrimary == 0)
		{
			ActualPrimary = 0;
		}
		else if(CurrentPrimary == 1)
		{
			ActualPrimary = 2;
		}
		else if(CurrentPrimary == 2)
		{
			ActualPrimary = 3;
		}
		
		//Back to normal now
		if(LastPrimary != CurrentPrimary)
		{
			updateWeaponsAndAttachments();
		}
	}
	
	void OnGUI()
	{
		
		if(NetworkManager.instance.MatchStarted == true && !NetworkManager.instance.MyPlayer.isAlive)	
		{
			//CurrentWeapon = GUILayout.SelectionGrid(CurrentWeapon, WeaponNames.ToArray(), 4);
			//NetworkManager.instance.MyPlayer.manager.FirstPersonCont.Weapons[CurrentWeapon].CurSight = GUILayout.SelectionGrid(NetworkManager.instance.MyPlayer.manager.FirstPersonCont.Weapons[CurrentWeapon].CurSight, SightNames.ToArray(), 4);
			if(GUI.Button(new Rect(0, 0, 150, 40), "Edit Loadout"))
			{
				editLoadout = true;
			}
		}
		else
		{
			editLoadout = false;
		}
		
		if(editLoadout == true)
			ChangeLoadout();
	}
	
	public void ChangeLoadout()
	{
		if(GUI.Button(new Rect(45, 45, 150, 80), "Primary"))
		{
			if(showPrimary == false)
			{
				showPrimary = true;
				showSecondary = false;
				showPrimarySlot1 = false;
			}
			else if(showPrimary == true)
			{
				showPrimary = false;
			}
			
			updateWeaponsAndAttachments();
			
			PrimaryWeapons.Clear();
			foreach(Gun gun in NetworkManager.instance.MyPlayer.manager.FirstPersonCont.Weapons.ToArray())
			{
				if(gun.isPrimary)
					PrimaryWeapons.Add(gun.name);
			}
		}
		if(GUI.Button(new Rect(45, 125, 75, 40), "Sight"))
		{
			if(showPrimarySlot1 == false)
			{
				showPrimary = false;
				showSecondary = false;
				showPrimarySlot1 = true;
			}
			else if(showPrimarySlot1 == true)
			{
				showPrimarySlot1 = false;
			}
		}
		if(GUI.Button(new Rect(120, 125, 75, 40), "Slot 1"))
		{
			
		}
		if(GUI.Button(new Rect(45, 165, 150, 80), "Secondary"))
		{
			if(showSecondary == false)
			{
				showSecondary = true;
				showPrimary = false;
				showPrimarySlot1 = false;
			}
			else if(showSecondary == true)
			{
				showSecondary = false;
			}
			
			SecondaryWeapons.Clear();
			foreach(Gun gun in NetworkManager.instance.MyPlayer.manager.FirstPersonCont.Weapons.ToArray())
			{
				if(gun.isSecondary)
					SecondaryWeapons.Add(gun.name);
			}
		}
		if(GUI.Button(new Rect(45, 245, 75, 40), "Slot 1"))
		{
			
		}
		if(GUI.Button(new Rect(120, 245, 75, 40), "Slot 2"))
		{
			
		}
		if(GUI.Button (new Rect(45, 285, 150, 40), "Apply"))
		{
			editLoadout = false;
			GUIManager.instance.CurrentWeapon = ActualPrimary;
			NetworkManager.instance.MyPlayer.manager.FirstPersonCont.Weapons[ActualPrimary].CurSight = CurrentPrimarySlot1;
			showSecondary = false;
			showPrimary = false;
			showPrimarySlot1 = false;
		}
		
		if(showPrimary)
		{
			CurrentPrimary = GUI.SelectionGrid(new Rect(195, 45, 550, 80), CurrentPrimary, PrimaryWeapons.ToArray(), 4);
		}
		if(showSecondary)
		{
			CurrentSecondary = GUI.SelectionGrid(new Rect(195, 165, 550, 80), CurrentSecondary, SecondaryWeapons.ToArray(), 4);
		}
		if(showPrimarySlot1)
		{
			CurrentPrimarySlot1 = GUI.SelectionGrid(new Rect(195, 125, 550, 40), CurrentPrimarySlot1, SightNames.ToArray(), 4);
		}
	}
	
	void updateWeaponsAndAttachments()
	{
		SightNames.Clear();
		LastPrimary = CurrentPrimary;
		foreach(Sight s in NetworkManager.instance.MyPlayer.manager.FirstPersonCont.Weapons[ActualPrimary].Sights.ToArray())
		{
			SightNames.Add(s.Name);
		}
	}
}
