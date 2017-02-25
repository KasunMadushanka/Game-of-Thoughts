using UnityEngine;
using System.Collections;

public class KingController : MonoBehaviour {

	Animator anim;
	ScoreManager script;

	void Start () {
		anim = GetComponent<Animator> ();
		script = GameObject.Find ("ButtonManager").GetComponent<ScoreManager> ();
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info){  
		
		Vector3 syncPosition = Vector3.zero;
		Quaternion syncRotation = Quaternion.identity;
		
		if (stream.isWriting)
		{   
			syncPosition = GetComponent<Rigidbody>().position;
			stream.Serialize(ref syncPosition);
			
			syncRotation = GetComponent<Rigidbody>().rotation;
			stream.Serialize(ref syncRotation);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncRotation);
			
			GetComponent<Rigidbody> ().position=syncPosition;
			GetComponent<Rigidbody>().rotation=syncRotation;
		}
	}

	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag("attackingRhino")) {
			anim.SetInteger("state",1);

		}
	}
}
