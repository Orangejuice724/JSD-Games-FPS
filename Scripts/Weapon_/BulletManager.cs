using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour {

	public float bulletSpeed;
	public float bulletDamage;
	public int bulletDamageValue = 100;
	public GameObject ImpactEffect;
	
	void Start () {
	
	}
	
	void Update () {
		if(bulletDamageValue == 0)
			this.gameObject.SetActive(false);
		manageBullet();
		this.rigidbody.AddRelativeForce(Vector3.forward, ForceMode.Impulse);
		
	}
	
	public void manageBullet()
	{
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		
		RaycastHit hit;
		
		if(Physics.Raycast(transform.position, fwd, out hit, 10))
		{
			bulletDamageValue -= 25;
			
			PlayerController hitter = hit.transform.root.GetComponent<PlayerController>();
			Network.Instantiate(ImpactEffect, hit.point, Quaternion.FromToRotation(hit.normal, Vector3.up),0);
				
			if(hitter != null && hitter.MyPlayer.Team != NetworkManager.instance.MyPlayer.Team)
			{
				hitter.networkView.RPC ("Server_TakeDamage", RPCMode.All, bulletDamage);
				hitter.networkView.RPC ("findHitter", RPCMode.All, NetworkManager.instance.MyPlayer.PlayerName, name);
			}
			
		}
	}
}
