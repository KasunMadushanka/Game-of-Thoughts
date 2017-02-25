using UnityEngine;
using System.Collections;
public class CameraController : MonoBehaviour {
	
	public float speedH = 2.0f;
	public float speedV = 2.0f;
	GliderController script;
	public float yaw = 0.0f;
	public float pitch = 0.0f;

	void Start(){
		script = GetComponent<GliderController> ();
		if (Network.isServer) {
			yaw=45f;
		} else if(Network.isClient) {
			yaw=-45f;
		}
	}
	
	void Update () {
		yaw += speedH * Input.GetAxis("Mouse X");
	    pitch -= speedV * Input.GetAxis("Mouse Y");
		if(GetComponent<NetworkView>().isMine){
			if (!script.flying) {
				GetComponent<Rigidbody>().transform.eulerAngles = new Vector3 (0, yaw, 0.0f);
			} else {
				GetComponent<Rigidbody>().transform.eulerAngles = new Vector3 (pitch, yaw, 0.0f);
			}
		}
	}
}