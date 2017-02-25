using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
	
	public GameObject bullet;
	public GameObject bulletHole;
	public float delayTime = 0.5f;
	
	private float counter = 0;
	
	void FixedUpdate ()	
	{
		if (GetComponent<NetworkView> ().isMine) {
			if(Grid.gameState.Equals("fight")){
				if (Input.GetKey (KeyCode.Mouse0) && counter > delayTime) {
					Network.Instantiate (bullet, transform.position, transform.rotation,0);
					GetComponent<AudioSource>().Play();
					counter = 0;
				}
				counter += Time.deltaTime;
			}
		}
	}
}
