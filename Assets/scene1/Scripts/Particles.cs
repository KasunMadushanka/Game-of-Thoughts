using UnityEngine;
using System.Collections;

public class Particles : MonoBehaviour {
	
		protected bool playing;
		
		public void Update(){
			if(Input.GetKeyDown(KeyCode.L)){   
			    if(!playing){
					GetComponent<ParticleSystem>().Play();
					playing=true;
				}else{
					GetComponent<ParticleSystem>().Stop();
					playing=false;
				}
			}
		}
}

