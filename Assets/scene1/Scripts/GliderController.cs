using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GliderController: MonoBehaviour
{   
	ScoreManager script;
	Animator anim;
	public float speed,rotationSpeed,rotation;
	public bool flying;

	void Start(){
		script = GameObject.Find ("ButtonManager").GetComponent<ScoreManager> ();
		anim = GetComponent<Animator> ();
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{  
		Animator animator = GetComponent<Animator> ();
		CameraController script = GetComponent<CameraController> ();
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		Vector3 angles = Vector3.zero;
		bool syncKinematic=false;
		Quaternion syncRotation = Quaternion.identity;
		int state = 0;

		if (stream.isWriting)
		{   
			syncPosition = GetComponent<Rigidbody>().position;
		    stream.Serialize(ref syncPosition);

			syncPosition = GetComponent<Rigidbody>().velocity;
			stream.Serialize(ref syncVelocity);

			angles=GetComponent<Rigidbody>().transform.eulerAngles;
			syncRotation =  Quaternion.Euler(angles);
			stream.Serialize(ref syncRotation);

			syncKinematic=GetComponent<Rigidbody>().isKinematic;
			stream.Serialize(ref syncKinematic);

			if(animator.GetCurrentAnimatorStateInfo (0).IsName("gunplay1"))
				state=4;
			if(animator.GetCurrentAnimatorStateInfo (0).IsName("running"))
				state=2;
			else if(animator.GetCurrentAnimatorStateInfo (0).IsName("walking"))
				state=1;
			else if(animator.GetCurrentAnimatorStateInfo (0).IsName("idle"))
				state=0;
			else if(animator.GetCurrentAnimatorStateInfo (0).IsName("backflip"))
				state=3;
			else if(animator.GetCurrentAnimatorStateInfo (0).IsName("head_hit"))
				state=5;
			else if(animator.GetCurrentAnimatorStateInfo (0).IsName("treading_water"))
				state=6;
			else if(animator.GetCurrentAnimatorStateInfo (0).IsName("flying"))
				state=7;
			else if(animator.GetCurrentAnimatorStateInfo (0).IsName("left_strafe_walking"))
				state=8;
			else if(animator.GetCurrentAnimatorStateInfo (0).IsName("right_strafe_walking"))
				state=9;
			stream.Serialize(ref state);

		}
		else
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncVelocity);
			stream.Serialize(ref syncRotation);
			stream.Serialize(ref syncKinematic);
			stream.Serialize(ref state);

			GetComponent<Rigidbody> ().position=syncPosition;
			GetComponent<Rigidbody> ().rotation=syncRotation;
			GetComponent<Rigidbody> ().isKinematic=syncKinematic;
			animator.SetInteger("state",state);
		}
	}

	void Update()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			InputMovement();

		}
	}

    void InputMovement(){
		if (Input.GetKeyDown (KeyCode.L)) {
			if(flying){
					GetComponent<Rigidbody>().isKinematic=false;
					flying=false;
			}else{
					GetComponent<Rigidbody>().isKinematic=true;
					flying=true;
			}
		}
	

		if (Input.GetKey (KeyCode.Mouse0)) {
			anim.SetInteger ("state", 4);
		} else if (Input.GetKeyDown (KeyCode.LeftControl)) {
			anim.SetInteger ("state", 3);
		} else if (!Input.GetKey (KeyCode.LeftShift) && Input.GetKey (KeyCode.W)) {
			if (flying) {
				speed = 6f;
				anim.SetInteger ("state", 7);
			} else {
				speed = 3f;
				anim.SetInteger ("state", 1);
			}
			transform.Translate (Vector3.forward * speed * Time.deltaTime);
		} else if (Input.GetKey (KeyCode.LeftShift) && Input.GetKey (KeyCode.W)) {
			speed = 5f;
			anim.SetInteger ("state", 2);
			transform.Translate (Vector3.forward * speed * Time.deltaTime);
		} else if (Input.GetKey (KeyCode.A)) {
			anim.SetInteger ("state", 8);
			transform.Translate (Vector3.left * speed * Time.deltaTime);
		}else if(Input.GetKey (KeyCode.D)){
			anim.SetInteger ("state", 9);
			transform.Translate (Vector3.right * speed * Time.deltaTime);
		} else {
			if(flying){
					anim.SetInteger ("state", 6);
			}else{
					anim.SetInteger ("state", 0);
			}
		}

	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.CompareTag ("bullet")) {
			if(this.name.Equals("gliderfirst(Clone)")){
				anim.SetInteger ("state", 5);
				script.updateCellScore();
				script.updateFinalScore();
			}
		}
	}
	
}

	
