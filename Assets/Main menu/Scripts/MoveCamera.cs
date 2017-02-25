using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {
	
	public float speedH = 2.0f;
	public float speedV = 2.0f;
	
	private float yaw = 0.0f;
	private float pitch = 0.0f;
	
	void Update () {
		yaw += speedH * Input.GetAxis("Mouse X");
		pitch -= speedV * Input.GetAxis("Mouse Y");
		if ((pitch > -2 && pitch < 2) && (yaw > -2 && yaw < 2)) {
			transform.eulerAngles = new Vector3 (pitch, yaw, 0.0f);
		} else if (pitch < -2 && (yaw > -2 && yaw < 2)) {
			transform.eulerAngles = new Vector3 (-2, yaw, 0.0f);
		} else if (pitch > 2 && (yaw > -2 && yaw < 2)) {
			transform.eulerAngles = new Vector3 (2, yaw, 0.0f);
		} else if (yaw < -2 && (pitch > -2 && pitch < 2)) {
			transform.eulerAngles = new Vector3 (pitch, -2, 0.0f);
		} else if (yaw > 2 && (pitch > -2 && pitch < 2)) {
			transform.eulerAngles = new Vector3 (pitch, 2, 0.0f);
		}
	}
}