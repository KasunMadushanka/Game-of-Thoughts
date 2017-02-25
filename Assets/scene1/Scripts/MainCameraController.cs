using UnityEngine;
using System.Collections;

public class MainCameraController : MonoBehaviour {

	Vector3 pivot;
	float speed=3f;

	void Start () {
		pivot = new Vector3 (20,2,24);
	}

	void Update () {
		transform.RotateAround(pivot, Vector3.up, speed * Time.deltaTime);
	}
}
