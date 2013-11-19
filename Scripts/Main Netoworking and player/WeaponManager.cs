using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour {

	public List<Gun> Weapons = new List<Gun>();
	public int CurrentWeapon;
	public static WeaponManager instance;
	
	void Start () {
		instance = this;
	}
	
	void Update () {
		CurrentWeapon = GUIManager.instance.CurrentWeapon;
		ApplyWeapon();
	}
	
	public void FixedUpdate()
	{
		if(Input.GetKeyDown(KeyCode.R))
			Reload();
	}
	
	public void Spawn()
	{
		ApplyWeapon();
		transform.root.GetComponent<PlayerController>().Server_GetGun(Weapons[CurrentWeapon].name);
		CurrentWeapon = GUIManager.instance.CurrentWeapon;
	}
	
	public void Reload()
	{
		if(Weapons[CurrentWeapon].ammoTotal > 0)
		{
			if(Weapons[CurrentWeapon].ammoInMag == 0)
			{
				if(Weapons[CurrentWeapon].ammoTotal > Weapons[CurrentWeapon].ammoCap - 1)
				{
					Weapons[CurrentWeapon].ammoTotal = Weapons[CurrentWeapon].ammoTotal -= Weapons[CurrentWeapon].ammoCap;
					Weapons[CurrentWeapon].ammoInMag = Weapons[CurrentWeapon].ammoInMag + Weapons[CurrentWeapon].ammoCap;
				}
				else if(Weapons[CurrentWeapon].ammoTotal < Weapons[CurrentWeapon].ammoCap)
				{
					Weapons[CurrentWeapon].ammoInMag = Weapons[CurrentWeapon].ammoTotal;
					Weapons[CurrentWeapon].ammoTotal = 0;
				}
			}
			if(Weapons[CurrentWeapon].ammoInMag !=0)
			{
				int neededAmmo;
				neededAmmo = Weapons[CurrentWeapon].ammoCap - Weapons[CurrentWeapon].ammoInMag;
				if(Weapons[CurrentWeapon].ammoTotal > neededAmmo - 1)
				{
					Weapons[CurrentWeapon].ammoTotal = Weapons[CurrentWeapon].ammoTotal - neededAmmo;
					Weapons[CurrentWeapon].ammoInMag = Weapons[CurrentWeapon].ammoInMag + neededAmmo;
				}
				else if(Weapons[CurrentWeapon].ammoTotal < neededAmmo)
				{
					Weapons[CurrentWeapon].ammoInMag = Weapons[CurrentWeapon].ammoInMag + Weapons[CurrentWeapon].ammoTotal;
					Weapons[CurrentWeapon].ammoTotal = 0;
				}
			}
		}
		
		Weapons[CurrentWeapon].gun.animation.Play("Reload");
		StartCoroutine(waitForReload(Weapons[CurrentWeapon].gun.animation["Reload"].length));
	}
	
	public void ApplyWeapon()
	{
		foreach(Gun Gu in Weapons)
		{
			if(Gu == Weapons[CurrentWeapon])
			{
				Gu.gameObject.SetActive(true);
			}
			else
			{
				Gu.gameObject.SetActive(false);
			}
		}
	}
	
	public static Gun FindWeapon(string Name)
	{
		foreach(Gun Gu in instance.Weapons)
		{
			if(Name == Gu.GunName)
			{
				return Gu;
			}
		}
		return null;
	}
	
	public IEnumerator waitForReload(float animTime)
	{
		Weapons[CurrentWeapon].isReload = true;
		yield return new WaitForSeconds(animTime);
		Weapons[CurrentWeapon].isReload = false;
	}
}
