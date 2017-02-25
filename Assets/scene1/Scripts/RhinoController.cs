using UnityEngine;
using System.Collections;

public class RhinoController : MonoBehaviour {

	public Transform player;
	public float playerDistance,speed,rotationDamping;
	Animator anim;
	float y;
	public static string chasingRhino="no";
	public static bool chasing;

	void Start () {
		anim = GetComponent<Animator> ();
		player=GameObject.Find ("gliderfirst(Clone)").transform;
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info){  

		Vector3 syncPosition = Vector3.zero;			
		Quaternion syncRotation = Quaternion.identity;
		Animator animator = GetComponent<Animator> ();
		int state = 0;
		
		if (stream.isWriting)
		{  
			syncPosition = GetComponent<Rigidbody>().position;
			stream.Serialize(ref syncPosition);
			
			syncRotation = GetComponent<Rigidbody>().rotation;
			stream.Serialize(ref syncRotation);

			if(animator.GetCurrentAnimatorStateInfo (0).IsName("idle"))
				state=0;
			else if(animator.GetCurrentAnimatorStateInfo (0).IsName("crouched_walking"))
				state=1;
			else if(animator.GetCurrentAnimatorStateInfo (0).IsName("roundhouse_kick"))
				state=2;
			stream.Serialize(ref state);

		}
		else
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncRotation);
			stream.Serialize(ref state);
			
			GetComponent<Rigidbody> ().position=syncPosition;
			GetComponent<Rigidbody>().rotation=syncRotation;
			animator.SetInteger("state",state);
		}
	}

	void Update () {
		if (GameObject.Find ("My Player")!=null && (this.name.Equals("rhino1")||this.name.Equals("rhino2"))) {
				playerDistance = Vector3.Distance (player.position, transform.position);
				if (playerDistance < 15f && (player.position.y<=0.8 && player.position.y>=0.7)) {
					if(!chasing){
						chasing=true;
						chasingRhino=this.name;
					}
					LookAtPlayer ();
				    if(chasing && chasingRhino.Equals(this.name)){
						if(playerDistance>2f){
							anim.SetInteger("state",1);
							chase();
						}else{
							anim.SetInteger("state",2);
						}
				    }
				}else{
					anim.SetInteger("state",0);
					chasing=false;
				}
		}
	}

	void LookAtPlayer(){
			Quaternion rotation = Quaternion.LookRotation (player.position - transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationDamping);
	}

	void chase(){
			transform.Translate (Vector3.forward * speed * Time.deltaTime);
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.CompareTag ("bullet")) {
			print ("ok");

		}
	}
}
