using UnityEngine;
using System.Collections;

public class ChangeScene2: MonoBehaviour {
	
	public string scene;
	AccountManager script;

	void Start(){
		script = GameObject.Find ("center").GetComponent<AccountManager> ();
	}

	void OnMouseDown(){
		NetworkManager.NetworkState="client";
		script.setVisible ();
		
	}
}
