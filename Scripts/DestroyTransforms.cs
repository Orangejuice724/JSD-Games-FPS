using UnityEngine;
using System.Collections;

public class DestroyTransforms : MonoBehaviour {

	public Transform[] TransformsToDestroy;
	public Transform Explosion;
	
	void Start () {
	
	}
	
	
	void Update () {
	
	}
	void OnCollisionEnter(Collision collision) 
	{
		GameObject.Destroy(this.gameObject);
		print ("Collider Hit Something, May be myself i hit!");
        if(collision.gameObject.tag != "Jet")
		{
			foreach(Transform TransformsToInitiate in TransformsToDestroy)
			{
				Transform TRig = Instantiate(TransformsToInitiate, collision.transform.position, collision.transform.rotation) as Transform;
				
				TRig.rigidbody.AddForce(Vector3.forward * collision.relativeVelocity.magnitude, ForceMode.Impulse);
			}
			Instantiate (Explosion, collision.transform.position, collision.transform.rotation);
			print ("Destruction");
		}
	}
}
