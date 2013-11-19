using UnityEngine;
using System.Collections;

public class KnifeManager : MonoBehaviour {
	
	public bool isKnifing;
	public Transform knifePoint;
	
	public bool canKnifeBackStand;
	
	void Start () {
	
	}
	
	void Update () {
		checkForKnife();
	}
	
	public void checkForKnife()
	{
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		
		RaycastHit hit;
		
		if(Physics.Raycast(transform.position, fwd, out hit, 0.3F))
		{
			Debug.DrawRay(transform.position, fwd, Color.gray, 0.1F);
			if(hit.transform.root.tag == "back")
				canKnifeBackStand = true;
			print(hit.transform.root.tag.ToString());
		}
		else
		{
			canKnifeBackStand = false;
		}
	}
}
