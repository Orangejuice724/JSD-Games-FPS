using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour {
	
	public float fireRate;
	public float minDamage;
	public float maxDamage;
	private float ActualDamage;
	private float fireTime;
	public float Range = 800;
	public Transform SpawnPoint;
	public Transform gun;
	public string GunName;
	public GameObject ImpactEffect;
	public Transform AimObj;
	public List<Sight> Sights = new List<Sight>();
	public int CurSight;
	public bool isAuto;
	public bool isPrimary;
	public bool isSecondary;
	public bool isFiring = false;
	
	public GameObject bulletNormal;
	public GameObject bulletTracer;
	public int tracerCounter = 0;
	public GameObject currentBullet;
	
	public Transform recoilHolder;
	
	public Vector3 RecoilRotation;
	public Vector3 RecoilKickback;
	
	public Vector3 CurrentRecoil1;
	public Vector3 CurrentRecoil2;
	public Vector3 CurrentRecoil3;
	public Vector3 CurrentRecoil4;
	
	//public Transform muzzleFlash;
	
	public int ammoInMag;
	public int ammoCap;
	public int ammoTotal;
	public int ammoTotalCap;
	public int ammoRemain;
	
	public bool isReload = false;
	
	void Start () {
		ammoCap = ammoInMag;
		ammoTotalCap = ammoTotal;
		//muzzleFlash.gameObject.SetActive(false);
	}
	
	void Update () {
		if(Input.GetButton("Fire1") && isAuto == true && ammoInMag > 0 && isReload == false)
			Fire();
		if(Input.GetButtonDown("Fire1") && isAuto == false && ammoInMag > 0 && isReload == false)
			Fire();
		
		if(Input.GetButton("Fire1") == false)
		{
			isFiring = false;
		}
		
		if(tracerCounter != 5)
		{
			currentBullet = bulletNormal;
		}
		if(tracerCounter == 5)
		{
			currentBullet = bulletTracer;
		}
		if(tracerCounter == 6)
		{
			tracerCounter = 0;
		}
		
		foreach(Sight s in Sights)
		{
			if(Sights.IndexOf(s) == CurSight)
			{
				s.obj.SetActive(true);
			}
			else
			{
				s.obj.SetActive(false);
			}
		}
		
		RecoilController();
		
		if(isFiring == false)
		{
			//muzzleFlash.gameObject.SetActive(false);
		}
	}
	
	public void Fire()
	{
		if(fireTime <= Time.time)
		{
			
			//KickBack = new Vector3(Random.Range(minKickBack.x, maxKickBack.x), Random.Range(minKickBack.y, maxKickBack.y), Random.Range(minKickBack.z, maxKickBack.z));
			//KickUp = new Quaternion(Random.Range(minKickUp.x, minKickUp.x), Random.Range(minKickUp.y, minKickUp.y), Random.Range(minKickUp.z, minKickUp.z), 0);
			
			//Recoil
			CurrentRecoil1 += new Vector3(RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y));
			CurrentRecoil3 += new Vector3(Random.Range(-RecoilKickback.x, RecoilKickback.x), Random.Range(-RecoilKickback.y, RecoilKickback.y), RecoilKickback.z) ;
			
			//transform.localPosition = KickBack;
			//transform.localRotation = KickUp;
			
			isFiring = true;
			//muzzleFlash.gameObject.SetActive(true);
			//muzzleFlash.transform.rotation = new Quaternion(Random.Range(0, 360), muzzleFlash.localRotation.y, muzzleFlash.localRotation.z, 0);
			fireTime = fireRate + Time.time;
			ActualDamage = Random.Range (minDamage, maxDamage);
			audio.Play();
			NetworkManager.instance.MyPlayer.manager.Client_PlaySound(GunName, SpawnPoint.position);
			RaycastHit hit;
			ammoInMag -= 1;
			
			tracerCounter ++;
			
			GameObject inst_bullet = Network.Instantiate(currentBullet, SpawnPoint.position, SpawnPoint.rotation, 0) as GameObject;
			
			inst_bullet.GetComponent<BulletManager>().bulletDamage = ActualDamage;
			//inst_bullet.rigidbody.AddForce(Vector3.forward * Time.deltaTime * inst_bullet.GetComponent<BulletManager>().bulletSpeed);
			
			/*if(Physics.Raycast (SpawnPoint.position, SpawnPoint.forward,out hit, Range)){
				
				if(hit.collider.rigidbody)
				{
					hit.collider.rigidbody.AddForceAtPosition((ActualDamage * 200) * transform.forward, hit.collider.transform.position);
				}
				
				PlayerController hitter = hit.transform.root.GetComponent<PlayerController>();
				Network.Instantiate(ImpactEffect, hit.point, Quaternion.FromToRotation(hit.normal, Vector3.up),0);
				
				if(hitter != null && hitter.MyPlayer.Team != NetworkManager.instance.MyPlayer.Team)
				{
					hitter.networkView.RPC ("Server_TakeDamage", RPCMode.All, ActualDamage);
					hitter.networkView.RPC ("findHitter", RPCMode.All, NetworkManager.instance.MyPlayer.PlayerName, name);
					
				}
			}*/
		}
	}
	
	public void RecoilController()
	{
		CurrentRecoil1 = Vector3.Lerp(CurrentRecoil1, Vector3.zero, 0.1f);
		CurrentRecoil2 = Vector3.Lerp(CurrentRecoil2, CurrentRecoil1, 0.1f);
		CurrentRecoil3 = Vector3.Lerp(CurrentRecoil3, Vector3.zero, 0.1f);
		CurrentRecoil4 = Vector3.Lerp(CurrentRecoil4, CurrentRecoil3, 0.1f);
		
		recoilHolder.localEulerAngles = CurrentRecoil2;
		recoilHolder.localPosition = CurrentRecoil4;
	}
	
	void FixedUpdate()
	{
		if(Input.GetButton("Fire2"))
		{
			AimObj.transform.localPosition = Vector3.Lerp(AimObj.transform.localPosition, Sights[CurSight].AimPos, 0.25F);
		}
		else
		{
			AimObj.transform.localPosition = Vector3.Lerp(AimObj.transform.localPosition, Vector3.zero, 0.25F);
		}
	}
}

[System.Serializable]
public class Sight
{
	public string Name;
	public GameObject obj;
	public Vector3 AimPos;
}