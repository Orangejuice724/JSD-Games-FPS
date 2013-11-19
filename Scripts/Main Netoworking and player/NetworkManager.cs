using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {
	
	public string PlayerName;
	public string MatchName;
	public static NetworkManager instance;
	public List<Player> PlayerList = new List<Player>();
	public Player MyPlayer;
	public GameObject SpawnPlayer;
	public bool MatchStarted;
	public bool MatchLoaded;
	public Level CurLevel;
	public GameModes CurGamemode;
	public List<Level> ListOfLevels = new List<Level>();
	public List<GameModes> ListOfGames = new List<GameModes>();
	private int TeamIndex;
	
	public int rteam = 0;
	public int bteam = 0;
	
	//public List<Transform> SpawnPoints = new List<Transform>();
	//public GameObject[] SpawnPoints;
	
	void Awake()
	{
		instance = this;
		PlayerName = PlayerPrefs.GetString("name");
		//MasterServer.ipAddress = "127.0.0.1";
		//MasterServer.port = 23466;
		//Network.natFacilitatorIP = "127.0.0.1";
	}
	
	void Start () {
		DontDestroyOnLoad(gameObject);
		
	}
	
	void Update () {
	}
	
	public void StartServer(string ServerName, int MaxPlayers){
		Network.InitializeSecurity();
		Network.InitializeServer(MaxPlayers, 24465, true);
		MasterServer.RegisterHost("Conquer", ServerName, "");
		Debug.Log ("Started Server");
	}
	
	void OnPlayerConnected(NetworkPlayer id)
	{
		//networkView.RPC("Server_PlayerJoined", RPCMode.Server, PlayerName, id);
		foreach(Player pl in PlayerList)
		{
			networkView.RPC("getLevel", id, CurLevel.LoadName, MatchStarted);
			networkView.RPC ("Client_PlayerJoined", id, pl.PlayerName, CurLevel.Teams.IndexOf(pl.Team), pl.OnlinePlayer);
		}
	}
	
	void OnConnectedToServer()
	{
		int pteam = 0;
		foreach(Player pl in NetworkManager.instance.PlayerList)
		{
			if(pl.Team == NetworkManager.instance.CurLevel.Teams[0])
			{
				bteam += 1;
			}
			else
			{
				rteam += 1;
			}
		}
		if(rteam > bteam)
		{
			pteam = 0;
		}
		else if(bteam > rteam)
		{
			pteam = 1;
		}
		else
		{
			pteam = Random.Range(0, 1);
		}
		networkView.RPC("Server_PlayerJoined", RPCMode.Server, PlayerName, pteam, Network.player);
	}
	
	void OnServerInitialized()
	{
		Server_PlayerJoined (PlayerName, Random.Range(0, CurLevel.Teams.Count), Network.player);
	}
	
	void OnPlayerDisconnected(NetworkPlayer id)
	{
		networkView.RPC("RemovePlayer", RPCMode.All, id);
		Network.Destroy (getPlayer(id).manager.gameObject);
		Network.RemoveRPCs(id);
	}
	
	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		foreach(Player pl in PlayerList)
		{
			Network.Destroy(pl.manager.gameObject);
		}
		PlayerList.Clear ();
		Application.LoadLevel(0);
	}
	
	[RPC]
	public void Server_PlayerJoined(string Username, int Team, NetworkPlayer id) {
		networkView.RPC("Client_PlayerJoined", RPCMode.All, Username, Team, id);
	}
	
	[RPC]
	public void Client_PlayerJoined(string Username, int Team, NetworkPlayer id) {
		Player temp = new Player();
		temp.PlayerName = Username;
		temp.OnlinePlayer = id;
		temp.Team = CurLevel.Teams[Team];
		PlayerList.Add(temp);
		if(Network.player == id)
		{
			MyPlayer = temp;
			GameObject LastPlayer = Network.Instantiate(SpawnPlayer, Vector3.zero, Quaternion.identity, 0) as GameObject;
			LastPlayer.networkView.RPC("RequestPlayer", RPCMode.AllBuffered, Username);
		}
	}
	
	[RPC]
	public void RemovePlayer(NetworkPlayer id)
	{
		Player temp = new Player();
		foreach(Player pl in PlayerList)
		{
			if(pl.OnlinePlayer == id)
			{
				temp = pl;
			}
		}
		if(temp != null)
		{
			PlayerList.Remove (temp);
		}
	}
	
	[RPC]
	public void RemoveAllPlayers()
	{
		Player temp = new Player();
		foreach(Player pl in PlayerList)
		{
			PlayerList.Remove (pl);
			print ("Player Removed");
			Network.Destroy(pl.manager.gameObject);
			MasterServer.UnregisterHost();
			Application.LoadLevel(0);
		}
	}
	
	[RPC]
	public void LoadLevel(string loadName)
	{
		MatchStarted = true;
		Application.LoadLevel(loadName);
	}
	
	[RPC]
	public Level getLevel(string LoadName, bool isStarted)
	{
		foreach(Level lvl in ListOfLevels)
		{
			if(LoadName == lvl.LoadName)
			{
				CurLevel = lvl;
				return lvl;
			}
		}
		if(isStarted)
		{
			LoadLevel (LoadName);
		}
		
		return null;
	}
	
	public static Player getPlayer(NetworkPlayer id)
	{
		foreach(Player pl in instance.PlayerList)
		{
			if(pl.OnlinePlayer == id)
			{
				return pl;
			}
		}
		return null;
	}
	
	public static bool HasPlayer(string n)
	{
		foreach(Player pl in instance.PlayerList)
		{
			if(pl.PlayerName == n)
			{
				return true;
			}
		}
		return false;
	}
	
	public static Player getPlayer(string id)
	{
		foreach(Player pl in instance.PlayerList)
		{
			if(pl.PlayerName == id)
			{
				return pl;
			}
		}
		return null;
	}
	
	void OnGUI()
	{
		if(MatchStarted == true && !MyPlayer.isAlive)
		{
			if(GUI.Button(new Rect(Screen.width - 50, 0, 50, 20), "Spawn"))
			{
				MyPlayer.manager.FirstPersonCont.Spawn();
				int SpawnIndex = Random.Range(0, LevelManager.instance.SpawnPoints.Length - 1);
				MyPlayer.manager.FirstPerson.localPosition = LevelManager.instance.SpawnPoints[SpawnIndex].transform.position;
				MyPlayer.manager.FirstPerson.localRotation = LevelManager.instance.SpawnPoints[SpawnIndex].transform.rotation;
				MyPlayer.manager.networkView.RPC("Spawn", RPCMode.All);
				MyPlayer.manager.FirstPersonCont.Weapons[MyPlayer.manager.FirstPersonCont.CurrentWeapon].ammoInMag = MyPlayer.manager.FirstPersonCont.Weapons[MyPlayer.manager.FirstPersonCont.CurrentWeapon].ammoCap;
				MyPlayer.manager.FirstPersonCont.Weapons[MyPlayer.manager.FirstPersonCont.CurrentWeapon].ammoTotal = MyPlayer.manager.FirstPersonCont.Weapons[MyPlayer.manager.FirstPersonCont.CurrentWeapon].ammoTotalCap;
			}
		}
	}
}

[System.Serializable]
public class Player {
	public string PlayerName;
	public NetworkPlayer OnlinePlayer;
	public float Health = 100;
	public PlayerController manager;
	public bool isAlive;
	public int Deaths;
	public int Kills;
	public int Score;
	public string Team;
}

[System.Serializable]
public class Level
{
	public string LoadName;
	public string PlayName;
	public List<string> Teams = new List<string>();
}

[System.Serializable]
public class GameModes
{
	public string gameName;
}
