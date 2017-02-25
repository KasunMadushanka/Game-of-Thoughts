using UnityEngine;
using System.Collections;

public class Snapshot : MonoBehaviour {
	public string deviceName;
	WebCamTexture wct;

	public void openWebcam () {
		WebCamDevice[] devices = WebCamTexture.devices;
		deviceName = devices[0].name;
		wct = new WebCamTexture(deviceName, 400, 300, 12);
		wct.Play();
	}

	public Texture2D heightmap;
	public Vector3 size = new Vector3(100, 10, 100);

	private string _SavePath = "E:/"; 
	int _CaptureCounter = 0;
	
	public void TakeSnapshot()
	{
		Texture2D snap = new Texture2D(wct.width, wct.height);
		snap.SetPixels(wct.GetPixels());
		snap.Apply();
		
		System.IO.File.WriteAllBytes(_SavePath + _CaptureCounter.ToString() + ".jpg", snap.EncodeToJPG());
		++_CaptureCounter;

		wct.Stop ();
	}
}