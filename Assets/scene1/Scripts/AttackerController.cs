using UnityEngine;
using System.Collections;

public class AttackerController : MonoBehaviour {


	Vector3 target;
	float rotationDamping=5f,distance;
	public bool targetOn;
	Animator anim;
	bool reached;

	void Start () {
		anim = GetComponent<Animator> ();
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
			
			if(animator.GetCurrentAnimatorStateInfo (0).IsName("crouched_walking"))
				state=1;
			else if(animator.GetCurrentAnimatorStateInfo (0).IsName("idle"))
				state=0;
			else if(animator.GetCurrentAnimatorStateInfo (0).IsName("roundhouse_kick"))
				state=2;
			else if(animator.GetCurrentAnimatorStateInfo (0).IsName("flip_kick"))
				state=3;
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
		distance=Vector3.Distance (target, transform.position);
		if (targetOn && reached) {
			if(distance>5f){
				anim.SetInteger ("state", 1);
				rotateAround ();
				move (3f);
			}else if(distance<=5f && distance>=0.5f){
				anim.SetInteger ("state", 3);
				move (0.5f);
			}
		} else {
			if((targetOn && !reached) && distance>0.5f){
				anim.SetInteger ("state", 1);
				rotateAround ();
				move (3f);
			}else{
				anim.SetInteger ("state", 0);
			}
		}
	}

	public void setTarget(int row){
		int column = Grid.otherKingCell [1];
		int id = row * 30+(column+1);
		target= GameObject.Find(id+"").transform.position;
		targetOn = true;

	} 

	void move(float speed){
		transform.Translate (Vector3.forward * speed * Time.deltaTime);
	}

	void rotateAround(){
		if (target!=transform.position) {
			Quaternion rotation = Quaternion.LookRotation (target - transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationDamping);
		}
	}
	
	public void attackOtherKing(){
		reached = true;
		setTarget (Grid.otherKingCell [0]);
	}
}
