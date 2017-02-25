using UnityEngine;
using System.Collections;

public class CameraRotator : MonoBehaviour {

	GameObject targetCharacter;
	public bool rotating;
	float speed=50f;

	void Start () {
		targetCharacter=GameObject.Find("attackingRhino");
	}
	

	void Update () {
		if (rotating) {
			transform.RotateAround(targetCharacter.transform.position, Vector3.up, speed * Time.deltaTime);
		}
	}
}
