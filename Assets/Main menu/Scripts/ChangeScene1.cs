using UnityEngine;
using System.Collections;

public class ChangeScene1: MonoBehaviour {

	public string scene;
	AccountManager script;

	void Start(){
		script = GameObject.Find ("center").GetComponent<AccountManager> ();
	}

	void OnMouseDown(){
		NetworkManager.NetworkState = "server";
		script.setVisible ();

	}
}
