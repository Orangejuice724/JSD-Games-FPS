using UnityEngine;
using System.Collections;

public class VehicleDestruction : MonoBehaviour {

	public Transform replacementObject;
	public Transform heliObject;
	public string Tag;
	public int destroyed = 0;
	
	void Start () {
	
	}
	
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision collision) 
	{
		print ("Collider Hit Something, May be myself i hit!");
        if(collision.gameObject.tag != "Jet" && destroyed == 0)
		{
			destroyed += 1;
			Instantiate(replacementObject, this.transform.position, this.transform.rotation);
			heliObject.rigidbody.AddForceAtPosition(Vector3.forward * 10000, collision.transform.position);
			foreach(Transform child in transform)
			{
				if(child.name == "AI")
				{
					GameObject.Destroy(child);
				}
			}
			if(!heliObject == null)
				heliObject.rigidbody.useGravity = true;
			print ("Not Main Object Hit");
			GameObject.Destroy(this.gameObject);
		}
    }
}
