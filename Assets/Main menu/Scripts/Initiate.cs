using UnityEngine;
using System.Collections;

public class Initiate : MonoBehaviour {
	
	public static void fade(string scene,Color color,float damp){
		GameObject init = new GameObject ();
		init.name="Fader";
		init.AddComponent<Fader> ();
		Fader scr = init.GetComponent<Fader> ();
		scr.fadeDamp = damp;
		scr.fadeScene = scene;
		scr.fadeColor = color;
		scr.start = true;
	}
}
