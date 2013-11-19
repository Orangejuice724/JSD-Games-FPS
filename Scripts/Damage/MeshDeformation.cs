using UnityEngine;
using System.Collections;
 
public class MeshDeformation : MonoBehaviour {
 
    public Vector3 relativeVelocity;
 
    void Update () {
 
       transform.Translate(relativeVelocity * Time.deltaTime);
    }
 
    void OnCollisionEnter (Collision collision) {
 
        MeshFilter mf = collision.collider.GetComponent<MeshFilter>();
       Mesh mesh = mf.mesh;
 
        Vector3[] vertices = mesh.vertices;
        Vector3 hitPoint = transform.InverseTransformPoint(collision.contacts[0].point);
        float hitRadius = relativeVelocity.magnitude;
        Vector3 hitDir = transform.InverseTransformDirection(-collision.contacts[0].normal);
 
        int i = 0;
        while (i < vertices.Length) {
            float distance = Vector3.Distance(vertices[i], hitPoint);
            Vector3 dir = (vertices[i] - hitPoint);
            if(dir.magnitude < hitRadius){
               float amount = 1 - dir.magnitude / hitRadius;
                Vector3 vertMove = hitDir * amount;
                vertices[i] += vertMove;
         }
         i++;
       }
 
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }
}