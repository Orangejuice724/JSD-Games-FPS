using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	public static Menu instance;
	private string CurMenu;
	public string Name;
	public string MatchName;
	public int MaxPlayers;
	
	public int selected = 1;
	
	public GUISkin selectedBox;
	public GUISkin unSelectedBox;
	public GUISkin hoverBox;
	public Texture mainMenuBG;
	public Texture singlePlayerBG;
	
	void Start () {
		instance = this;
		CurMenu = "Main";
		Name = PlayerPrefs.GetString ("name");
		MaxPlayers = 8;
		MatchName = "Server " + Random.Range(0, 9999);
	}
	
	void Update () {
		if(CurMenu == "Main")
		{
			if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				if(selected != 4)
				{
					selected += 1;
				}
				else
				{
					selected = 1;
				}
			}
			if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				if(selected != 1)
				{
					selected -= 1;
				}
				else
				{
					selected = 4;
				}
			}
		}
	}
	
	void ToMenu(string menu)
	{
		CurMenu = menu;
	}
	
	void OnGUI() {
		if(CurMenu == "Main")
			Main ();
		if(CurMenu == "Host")
			Host ();
		if(CurMenu == "Lobby")
			Lobby ();
		if(CurMenu == "FindServer")
			MatchList ();
		if(CurMenu == "Levels")
			Levels();
		if(CurMenu == "SP")
			SinglePlayer();
		
		if(selected == 1)
		{
			if(Input.GetKeyDown(KeyCode.Return))
			{
				CurMenu = "SP";
			}
		}
		if(selected == 2)
		{
			if(Input.GetKeyDown(KeyCode.Return))
			{
				CurMenu = "Host";
			}
		}
		if(selected == 3)
		{
			if(Input.GetKeyDown(KeyCode.Return))
			{
				CurMenu = "FindServer";
			}
		}
		if(selected == 4)
		{
			if(Input.GetKeyDown(KeyCode.Return))
			{
				Application.Quit();
			}
		}
	}
	
	private void Main(){
		
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mainMenuBG, ScaleMode.StretchToFill, false, 0f);
		GUI.skin = unSelectedBox;
		GUI.Box(new Rect(0, 0.066f * Screen.height, 700, Screen.height - ((0.066f * Screen.height)) * 2), GUIContent.none);
		
		if(selected == 1)
		{
			GUI.skin = selectedBox;
			GUI.Box (new Rect(0, 0.106f * Screen.height, 750, 95), "Single Player");
			GUI.skin = unSelectedBox;
			GUI.Box (new Rect(0, 0.106f * Screen.height + 115, 675, 95), "Multiplayer");
			GUI.skin = unSelectedBox;
			GUI.Box (new Rect(0, 0.106f * Screen.height + 230, 675, 95), "Settings");
			GUI.skin = unSelectedBox;
			GUI.Box (new Rect(0, 0.881f * Screen.height - ((0.066f * Screen.height)), 675, 95), "Exit");
		}
		else if(selected == 2)
		{
			GUI.skin = unSelectedBox;
			GUI.Box (new Rect(0, 0.106f * Screen.height, 675, 95), "Single Player");
			GUI.skin = selectedBox;
			GUI.Box (new Rect(0, 0.106f * Screen.height + 115, 750, 95), "Multiplayer");
			GUI.skin = unSelectedBox;
			GUI.Box (new Rect(0, 0.106f * Screen.height + 230, 675, 95), "Settings");
			GUI.skin = unSelectedBox;
			GUI.Box (new Rect(0, 0.881f * Screen.height - ((0.066f * Screen.height)), 675, 95), "Exit");
		}
		else if(selected == 3)
		{
			GUI.skin = unSelectedBox;
			GUI.Box (new Rect(0, 0.106f * Screen.height, 675, 95), "Single Player");
			GUI.skin = unSelectedBox;
			GUI.Box (new Rect(0, 0.106f * Screen.height + 115, 675, 95), "Multiplayer");
			GUI.skin = selectedBox;
			GUI.Box (new Rect(0, 0.106f * Screen.height + 230, 750, 95), "Settings");
			GUI.skin = unSelectedBox;
			GUI.Box (new Rect(0, 0.881f * Screen.height - ((0.066f * Screen.height)), 675, 95), "Exit");
		}
		else if(selected == 4)
		{
			GUI.skin = unSelectedBox;
			GUI.Box (new Rect(0, 0.106f * Screen.height, 675, 95), "Single Player");
			GUI.skin = unSelectedBox;
			GUI.Box (new Rect(0, 0.106f * Screen.height + 115, 675, 95), "Multiplayer");
			GUI.skin = unSelectedBox;
			GUI.Box (new Rect(0, 0.106f * Screen.height + 230, 675, 95), "Settings");
			GUI.skin = selectedBox;
			GUI.Box (new Rect(0, 0.881f * Screen.height - ((0.066f * Screen.height)), 750, 95), "Exit");
		}
	}
	
	private void Host() {
		
		if(GUI.Button(new Rect(0, 0, 128, 32), "Start Game")){
			NetworkManager.instance.StartServer(MatchName, MaxPlayers);
			ToMenu("Lobby");
		}
		
		if(GUI.Button(new Rect(0, 33, 128, 32), "Main Menu")){
				ToMenu("Main");
		}
		if(GUI.Button(new Rect(0, 66, 128, 32), "Choose Map")){
				ToMenu("Levels");
		}
		
		MatchName = GUI.TextField(new Rect(130, 0, 128, 32), MatchName);
		GUI.Label (new Rect(260, 0, 128, 32), "Match Name");
		GUI.Label (new Rect(130, 33, 128, 32), "Max Players");
		MaxPlayers = Mathf.Clamp (MaxPlayers, 0, 32);
		if(GUI.Button (new Rect(210, 33, 32, 32), "+"))
			MaxPlayers += 2;
		if(GUI.Button (new Rect(274, 33, 32, 32), "-"))
			MaxPlayers -= 2;
		GUI.Label (new Rect(254, 33, 64, 32), MaxPlayers.ToString());
	}
	
	private void Lobby() {
		if(Network.isServer)
		{
			if(GUI.Button(new Rect(Screen.width - 128, Screen.height - 64, 128, 32), "Start Match")){
				NetworkManager.instance.networkView.RPC ("LoadLevel", RPCMode.All, NetworkManager.instance.CurLevel.LoadName);
			}
		}
		
		if(GUI.Button(new Rect(Screen.width - 128, Screen.height - 32, 128, 32), "Disconnect")){
			ToMenu("Main");
			Network.Disconnect();
		}
		
		GUILayout.BeginArea(new Rect(0, 0, 128, Screen.height), "Australia");
		GUILayout.Space(20);
		foreach(Player pl in NetworkManager.instance.PlayerList)
		{
			GUILayout.BeginHorizontal();
			if(pl.Team == NetworkManager.instance.CurLevel.Teams[0])
			{
				GUI.color = Color.blue;
				GUILayout.Box(pl.PlayerName);
			}
			GUI.color = Color.white;
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
		
		GUILayout.BeginArea(new Rect(256, 0, 128, Screen.height), "Russia");
		GUILayout.Space(20);
		foreach(Player pl in NetworkManager.instance.PlayerList)
		{
			GUILayout.BeginHorizontal();
			if(pl.Team == NetworkManager.instance.CurLevel.Teams[1])
			{
				GUI.color = Color.red;
				GUILayout.Box(pl.PlayerName);
			}
			GUI.color = Color.white;
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
	}
	
	private void MatchList()
	{
		if(GUI.Button(new Rect(0, 0, 128, 32), "Refresh")){
			MasterServer.RequestHostList("Conquer");
		}
		
		if(GUI.Button(new Rect(0, 33, 128, 32), "Main Menu")){
				ToMenu("Main");
		}
		
		GUILayout.BeginArea(new Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height), "Server List", "box");
		
		foreach(HostData hd in MasterServer.PollHostList())
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(hd.gameName);
			if(GUILayout.Button ("Connect"))
			{
				Network.Connect(hd);
				ToMenu ("Lobby");
			}
			GUILayout.EndHorizontal();
		}
		
		GUILayout.EndArea();
	}
	
	private void Levels()
	{
		foreach(Level lvl in NetworkManager.instance.ListOfLevels)
		{
			if(GUILayout.Button(lvl.PlayName))
				NetworkManager.instance.CurLevel = lvl;
		}
		
		if(GUILayout.Button("Back"))
				ToMenu("Host");
	}
	
	private void SinglePlayer()
	{
		GUI.skin = unSelectedBox; 
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), singlePlayerBG, ScaleMode.StretchToFill, false, 0f);
		GUI.Box(new Rect(0, 64, Screen.width, 830), GUIContent.none);
		
		//loadgame box, new game box, and dlc box
		GUI.skin = hoverBox;
		GUI.Box (new Rect(1200, 196, 660, 660), "New Campaign");
		GUI.Box (new Rect(521, 196, 660, 660), "Load Campaign");
		if(GUI.Button(new Rect(0, 894, 220, 79), "Back"))
			CurMenu = "Main";
	}
}
