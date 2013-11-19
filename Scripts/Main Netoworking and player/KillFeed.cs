using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KillFeed : MonoBehaviour {

	public List<KillFeedInfo> KFI = new List<KillFeedInfo>();
	public static KillFeed instance;
	
	void Start () {
		instance = this;
	}
	
	void Update () {
	
	}
	
	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, 200));
		
		foreach(KillFeedInfo k in KFI)
		{
			GUILayout.BeginHorizontal();
			
			if(NetworkManager.getPlayer(k.Killer).Team == NetworkManager.instance.MyPlayer.Team)
			{
				GUI.color = Color.green;
				GUILayout.Label (k.Killer);
			}
			else
			{
				GUI.color = Color.red;
				GUILayout.Label (k.Killer);
			}
			GUI.color = Color.white;
			GUILayout.Label (k.Gun);
			if(NetworkManager.getPlayer(k.Killed).Team == NetworkManager.instance.MyPlayer.Team)
			{
				GUI.color = Color.green;
				GUILayout.Label (k.Killed);
			}
			else
			{
				GUI.color = Color.red;
				GUILayout.Label (k.Killed);
			}
			GUI.color = Color.white;
			GUILayout.EndHorizontal();
		}
		
		GUILayout.EndArea();
	}
	
	public void Server_SendKillFeed(string killer, string Gun, string Killed)
	{
		networkView.RPC ("Client_SendKillFeed", RPCMode.Server, killer, Gun, Killed);
	}
	
	[RPC]
	public void Client_SendKillFeed(string killer, string Gun, string Killed)
	{
		ConvertParamsToClass(killer, Gun, Killed);
	}
	
	private void ConvertParamsToClass(string killer, string Gun, string Killed)
	{
		KillFeedInfo k = new KillFeedInfo(killer, Gun, Killed);
		KFI.Add(k);
	}
}

[System.Serializable]
public class KillFeedInfo
{
	public KillFeedInfo(string K, string G, string V)
	{
		Killer = K;
		Gun = G;
		Killed = V;
	}
	
	public string Killer;
	public string Gun;
	public string Killed;
}
