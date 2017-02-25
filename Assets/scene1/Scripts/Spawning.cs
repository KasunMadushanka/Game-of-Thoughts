using UnityEngine;
using System.Collections;

public class Spawning : MonoBehaviour {

	public GameObject playerPrefab,kingPrefab,rhinoPrefab,attackingRhinoPrefab;
	public GameObject myPlayer,otherPlayer,king,rhino1,rhino2,attackingRhino;
	float speed = 2f;
	float x1=0,x2=0,z=0,yrotation=0,y=-4;

	void Start () {
	
	}

	void Update () {
		if (king != null && king.transform.position!=new Vector3(x2,0.7f,z)) {
			king.transform.position = Vector3.Lerp (king.transform.position, new Vector3 (x2, 0.7f, z), speed * Time.deltaTime);
			rhino1.transform.position = Vector3.Lerp (rhino1.transform.position, new Vector3 (x1, 0.7f, z - 3f), speed * Time.deltaTime);
			rhino2.transform.position = Vector3.Lerp (rhino2.transform.position, new Vector3 (x1, 0.7f, z+3f), speed * Time.deltaTime);
			attackingRhino.transform.position = Vector3.Lerp (attackingRhino.transform.position, new Vector3 (x1, 0.7f, z), speed * Time.deltaTime);
		}
	}

	public void SpawnPlayer()
	{
		if (Network.isServer) {
			myPlayer=(GameObject)Network.Instantiate(playerPrefab, new Vector3 (-1, 0.7f, 3), Quaternion.Euler(360,45,0), 0);
		} else {
			myPlayer=(GameObject)Network.Instantiate (playerPrefab, new Vector3 (35, 0.7f, 3), Quaternion.Euler(360,-45,0), 0);
		}

		myPlayer.name = "My Player";
		
	}

	public void spawnKing(Vector3 position){
		
		if (Network.isServer) {
			x1 = position.x+2f;
			yrotation=90;
		} else if (Network.isClient) {
			x1 = position.x -2f;
			yrotation=270;
		}
		x2 = position.x;
		z=position.z;
		
		king=(GameObject)Network.Instantiate(kingPrefab, new Vector3(position.x,y,position.z-1f), Quaternion.Euler(360,yrotation,0), 0);
		king.name="king";
		rhino1 =(GameObject) Network.Instantiate (rhinoPrefab, new Vector3 (x1, y, position.z - 3f), Quaternion.Euler (360, yrotation, 0), 0);
		rhino1.name="rhino1";
		rhino2 =(GameObject) Network.Instantiate (rhinoPrefab, new Vector3 (x1, y, position.z+3f), Quaternion.Euler (360, yrotation, 0), 0);
		rhino2.name="rhino2";
		attackingRhino=(GameObject) Network.Instantiate (attackingRhinoPrefab, new Vector3 (x1, y, position.z), Quaternion.Euler (360, yrotation, 0), 0);
		attackingRhino.name="attackingRhino";
	}
}
