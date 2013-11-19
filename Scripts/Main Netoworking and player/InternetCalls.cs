using UnityEngine;
using System.Collections;

public class InternetCalls : MonoBehaviour {
	
	
	
	void Start () {
	
	}
	
	void Update () {
	
	}
	
	public void DoLogin(string User, string Pass)
	{
		Debug.Log ("Start of DoLogin");
		WWWForm www = new WWWForm();
		www.AddField("user", User);
		www.AddField("password", Pass);
		
		WWW W = new WWW("http://www.warheroes.dx.am/login.php", www);
		StartCoroutine(Login(W, User));
		Debug.Log ("End of DoLogin");
	}
	
	public void DoRegister(string User, string Pass, string RegName)
	{
		Debug.Log ("Start of DoRegister");
		WWWForm www = new WWWForm();
		www.AddField("user", User);
		www.AddField("password", Pass);
		www.AddField("name", RegName);
		
		WWW W = new WWW("http://www.warheroes.dx.am/register.php", www);
		StartCoroutine(Register(W));
		Debug.Log ("End of DoRegister");
	}
	
	IEnumerator Register(WWW w)
	{
		yield return w;
		if(w.error == null)
		{
				//PlayerPrefs.SetString("name", User);
				Debug.Log("It Worked");
			
			Debug.Log (w.text.ToString());
		}
		else
		{
			Debug.Log("Couldn't Connect: " + w.error.ToString());
		}
	}
	
	IEnumerator Login(WWW w, string User)
	{
		yield return w;
		if(w.error == null)
		{
			if(w.text == "login-SUCCESS")
			{
				PlayerPrefs.SetString("name", User);
				Debug.Log("It Worked");
				Application.LoadLevel(1);
			}
		}
		else
		{
			Debug.Log("Couldn't Connect: " + w.error.ToString());
		}
	}
}
