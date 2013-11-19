using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour {

	public InternetCalls calls;
	public string Username;
	public string Password;
	
	public string RegUsername;
	public string RegPassword;
	public string RegName;
	
	void Start () {
	
	}
	
	void Update () {
	
	}
	
	void OnGUI()
	{
		GUILayout.BeginArea(new Rect((Screen.width / 2) - 256, (Screen.height / 2) - 128, 512, 256), GUIContent.none, "box");
		
		GUILayout.BeginHorizontal();
		
		GUILayout.Label ("Username");
		Username = GUILayout.TextField(Username);
		
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		
		GUILayout.Label ("Password");
		Password = GUILayout.PasswordField(Password, '*');
		
		GUILayout.EndHorizontal();
		
		if(GUILayout.Button ("Login"))
		{
			calls.DoLogin(Username, Password);
		}
		
		GUILayout.BeginHorizontal();
		
		GUILayout.Label ("Username");
		RegUsername = GUILayout.TextField(RegUsername);
		
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		
		GUILayout.Label ("Password");
		RegPassword = GUILayout.PasswordField(RegPassword, '*');
		
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		
		GUILayout.Label ("Name");
		RegName = GUILayout.TextField(RegName);
		
		GUILayout.EndHorizontal();
		
		if(GUILayout.Button ("Register"))
		{
			calls.DoRegister(RegUsername, RegPassword, RegName);
		}
		
		GUILayout.EndArea();
	}
}
