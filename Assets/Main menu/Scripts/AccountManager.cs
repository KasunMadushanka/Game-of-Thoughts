using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AccountManager : MonoBehaviour {
	
	Texture2D image;
	string path;
	Snapshot script;
	string profileName;
	public RawImage userImage;
	public InputField nameInput;
	public Image panel;
	public Button continueButton,openCamButton,setButton,cancelButton;
	public Text message;

	void Start(){

		Application.runInBackground = true;
		disable ();

		path="file://E:1.jpg";
		WWW www = new WWW (path);
		userImage.texture = www.texture;

		script = GetComponent<Snapshot> ();
		profileName = System.IO.File.ReadAllText("E://savegame//profile.txt");
	}

	public void changeScene(){
		System.IO.File.WriteAllText ("E://savegame//profile.txt",nameInput.text);
		Initiate.fade ("game", Color.black, 2.5f);
	}

	public void loadPicture(){
		script.TakeSnapshot ();
		path="file://E:0.jpg";
		WWW www = new WWW (path);
		userImage.texture = www.texture;
	}

	public void disable(){
		panel.enabled = false;
		message.enabled = false;
		userImage.enabled = false;
		continueButton.enabled = false;
		continueButton.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
		continueButton.GetComponentInChildren<Text>().color = Color.clear;
		cancelButton.enabled = false;
		cancelButton.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
		cancelButton.GetComponentInChildren<Text>().color = Color.clear;
		openCamButton.enabled = false;
		openCamButton.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
		openCamButton.GetComponentInChildren<Text>().color = Color.clear;
		setButton.enabled = false;
		setButton.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
		setButton.GetComponentInChildren<Text>().color = Color.clear;
		nameInput.enabled = false;
		nameInput.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
	}

	public void setVisible(){
		panel.enabled = true;
		message.enabled = true;
		userImage.enabled = true;
		continueButton.enabled = true;
		continueButton.GetComponentInChildren<Text>().color = Color.black;
		cancelButton.enabled = true;
		cancelButton.GetComponentInChildren<Text>().color = Color.black;
		openCamButton.enabled = true;
		openCamButton.GetComponentInChildren<Text>().color = Color.black;
		setButton.enabled = true;
		setButton.GetComponentInChildren<Text>().color = Color.black;
		nameInput.enabled = true;
	}
	
}
