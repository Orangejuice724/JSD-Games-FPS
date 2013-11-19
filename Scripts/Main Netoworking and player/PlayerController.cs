using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	
	public Transform FirstPerson;
	public CharacterController CharCont;
	public CharacterMotor CharMotor;
	public WeaponManager FirstPersonCont;
	public Transform ThirdPerson;
	public Transform WalkAnimationHolder;
	public Transform JumpAnimationHolder;
	public Player MyPlayer;
	public Vector3 CurPos;
	public Quaternion CurRot;
	public GameObject deadRag;
	public Player LastShotBy;
	public string LastShotByName;
	public string LastShotByGun;
	public List<ThirdPersonGuns> Guns = new List<ThirdPersonGuns>();
	public AudioClip hit;
	
	public WalkingState walkingstate = WalkingState.Idle;
	public float WalkSpeed;
	public float RunSpeed;
	public float VelocityMagnitude;
	
	public bool WasStanding;
	
	void Start () {
		if(networkView.isMine)
		{
			MyPlayer = NetworkManager.getPlayer(networkView.owner);
			MyPlayer.manager = this;
		}
		FirstPerson.gameObject.SetActive(false);
		ThirdPerson.gameObject.SetActive (false);
		DontDestroyOnLoad(gameObject);
	}
	
	[RPC]
	public void RequestPlayer(string Nameee)
	{
		networkView.RPC("GiveMyPlayer", RPCMode.OthersBuffered, Nameee);
	}
	
	[RPC]
	public void GiveMyPlayer(string n)
	{
		StartCoroutine(GivePlayer(n));
	}
	
	IEnumerator GivePlayer(string nn)
	{
		while(!NetworkManager.HasPlayer(nn))
		{
			yield return new WaitForEndOfFrame();
		}
		MyPlayer = NetworkManager.getPlayer(nn);
		MyPlayer.manager = this;
	}
	
	void Update () {
		
	}
	
	void FixedUpdate()
	{
		SpeedController();
		AnimationController();
		VelocityMagnitude = CharCont.velocity.magnitude;
	}
	
	public void SpeedController()
	{
		if((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && VelocityMagnitude > 0)
		{
			if(Input.GetButton("Sprint"))
			{
				walkingstate = WalkingState.RunningPrimary;
				CharMotor.movement.maxForwardSpeed = RunSpeed;
				CharMotor.movement.maxSidewaysSpeed = RunSpeed;
				CharMotor.movement.maxBackwardsSpeed = RunSpeed / 2;
			}
			else
			{
				walkingstate = WalkingState.Walking;
				CharMotor.movement.maxForwardSpeed = WalkSpeed;
				CharMotor.movement.maxSidewaysSpeed = WalkSpeed;
				CharMotor.movement.maxBackwardsSpeed = WalkSpeed / 2;
			}
		}
		else
		{
			walkingstate = WalkingState.Idle;
		}
	}
	
	public void AnimationController()
	{
		if(walkingstate == WalkingState.RunningPrimary)
		{
			WalkAnimationHolder.animation["SprintPrimary"].speed = VelocityMagnitude / RunSpeed * 1.2F;
			WalkAnimationHolder.animation.CrossFade("SprintPrimary", 0.2F);
		}
		else if(walkingstate == WalkingState.Walking)
		{
			WalkAnimationHolder.animation["FirstPersonWalk"].speed = VelocityMagnitude / WalkSpeed * 1.2F;
			WalkAnimationHolder.animation.CrossFade("FirstPersonWalk", 0.2F);
		}
		else
		{
			WalkAnimationHolder.animation.CrossFade("idle", 0.2F);
		}
	}
	
	public void Client_PlaySound(string GunName, Vector3 soundPoint)
	{
		networkView.RPC("Server_PlaySound", RPCMode.Others, GunName, soundPoint);
	}
	
	[RPC]
	public void Server_PlaySound(string GunName, Vector3 soundPoint)
	{
		AudioSource.PlayClipAtPoint(WeaponManager.FindWeapon(GunName).gameObject.audio.clip, soundPoint);
	}
	
	[RPC]
	public void findHitter(string name, string gun)
	{
		LastShotBy = NetworkManager.getPlayer(name);
		LastShotByName = name;
		LastShotByGun = gun;
	}
	
	[RPC]
	public void ShowMyKill(string MyName)
	{
		//MyPlayer.Score += xp;
		NetworkManager.getPlayer(MyName).Score += 100;
		NetworkManager.getPlayer(MyName).Kills ++;
	}
	
	[RPC]
	public void GetKill(int xp)
	{
		RankManager.instance.Exp += xp;
		networkView.RPC("ShowMyKill", RPCMode.All, MyPlayer.PlayerName);
		//MyPlayer.Score += xp;
		//MyPlayer.Kills ++;
	}
	
	[RPC]
	void Server_TakeDamage(float Damage)
	{
		networkView.RPC ("Client_TakeDamage", RPCMode.Server, Damage);
	}
	
	[RPC]
	void Client_TakeDamage(float Damage)
	{
		MyPlayer.Health -= Damage;
		//LastShotBy.manager.networkView.RPC("GetKill", LastShotBy.OnlinePlayer, 50);
		//audio.clip = hit;
		//audio.Play();
		
		if(MyPlayer.Health <= 0)
		{
			networkView.RPC ("Die", RPCMode.All);
			//Die ();
			MyPlayer.Deaths ++;
			MyPlayer.isAlive = false;
			MyPlayer.Health = 0;
			//Instantiate(deadRag, ThirdPerson.position, ThirdPerson.rotation);
			LastShotBy.manager.networkView.RPC("GetKill", LastShotBy.OnlinePlayer, 100);
		}
	}
	
	[RPC]
	void Spawn()
	{
		MyPlayer.isAlive = true;
		MyPlayer.Health = 100;
		if(networkView.isMine)
		{
			FirstPerson.gameObject.SetActive(true);
			ThirdPerson.gameObject.SetActive (false);
		}
		else
		{
			FirstPerson.gameObject.SetActive(false);
			ThirdPerson.gameObject.SetActive (true);
		}
	}
	
	[RPC]
	void Die()
	{
		MyPlayer.isAlive = false;
		//MyPlayer.Deaths ++;
		FirstPerson.gameObject.SetActive(false);
		ThirdPerson.gameObject.SetActive (false);
		Instantiate(deadRag, ThirdPerson.position, ThirdPerson.rotation);
		if(LastShotBy != null)
		{
			KillFeed.instance.Server_SendKillFeed(LastShotByName, LastShotByGun, MyPlayer.PlayerName);
		}
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if(stream.isWriting)
		{
			CurPos = FirstPerson.position;
			CurRot = FirstPerson.rotation;
			stream.Serialize(ref CurPos);
			stream.Serialize(ref CurRot);
			char Ani = (char)GetComponent<NetworkAnimStates>().CurrentAnim;
			stream.Serialize(ref Ani);
		}
		else
		{
			stream.Serialize(ref CurPos);
			stream.Serialize(ref CurRot);
			ThirdPerson.position = CurPos;
			ThirdPerson.rotation = CurRot;
			char Ani = (char)0;
			stream.Serialize(ref Ani);
			GetComponent<NetworkAnimStates>().CurrentAnim = (Animations)Ani;
		}
	}
	
	public void Server_GetGun(string name)
	{
		networkView.RPC("Client_GetGun", RPCMode.All, name);
		Debug.Log(name + " Server");
	}
	
	[RPC]
	public void Client_GetGun(string name)
	{
		foreach(ThirdPersonGuns TPG in Guns)
		{
			if(TPG.Name == name)
			{
				TPG.Obj.SetActive(true);
				Debug.Log(TPG.Name + " Client - Weapon Active");
			}
			else
			{
				TPG.Obj.SetActive(false);
				Debug.Log(TPG.Name + " Client - Weapon Inactive");
			}
		}
	}
}

[System.Serializable]
public class ThirdPersonGuns
{
	public string Name;
	public GameObject Obj;
}

public enum WalkingState
{
	Idle,
	Walking,
	RunningPrimary,
	RunningSecondary
}