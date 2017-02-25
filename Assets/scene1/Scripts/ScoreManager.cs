using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	Simulation script;
	LeaderBoard script3;
	public static int score1,score2;
	int time,minute,second;
	string min,sec;
	public Text cellsLeftText,timeText,waitingText,finalScoreText,username,gameOverText;
	int round;
	public Camera main;

	void Start(){
		script = GameObject.Find ("ButtonManager").GetComponent<Simulation> ();
		script3 = GameObject.Find ("ButtonManager").GetComponent<LeaderBoard> ();
		score1 = 0;
		score2 = 0;
		timeText.text = "";

	}

    public void updateCellScore(){

		score1=int.Parse(cellsLeftText.text)+20;

		if (score1 < 10) {
			cellsLeftText.text = "000" + score1.ToString ();
		} else if (score1 < 100) {
			cellsLeftText.text = "00" + score1.ToString ();
		} else if (score1 < 1000) {
			cellsLeftText.text = "0" + score1.ToString ();
		} else {
			cellsLeftText.text = score1.ToString ();
		}

	}

	public void updateFinalScore(){

		score2=int.Parse(finalScoreText.text)+10;

		if (score2 < 10) {
			finalScoreText.text = "00000" + score2.ToString ();
		} else if (score2 < 100) {
			finalScoreText.text = "0000" + score2.ToString ();
		} else if (score2 < 1000) {
			finalScoreText.text = "000" + score2.ToString ();
		} else if (score2 < 10000) {
			finalScoreText.text = "00" + score2.ToString ();
		} else if (score2 < 100000) {
			finalScoreText.text = "0" + score2.ToString ();
		} else {
			finalScoreText.text = score2.ToString ();
		}
	
	}

	public void decreaseCellScore(){
	
		score1=int.Parse(cellsLeftText.text)-1;
		
		if (score1 < 10) {
			cellsLeftText.text = "000" + score1.ToString ();
		} else if (score1 < 100) {
			cellsLeftText.text = "00" + score1.ToString ();
		} else if (score1 < 1000) {
			cellsLeftText.text = "0" + score1.ToString ();
		} else {
			cellsLeftText.text = score1.ToString ();
		}
	
	}

	public void startTimer(){
		if (Grid.gameState.Equals ("fight")) {
			StartCoroutine (countdown (45));
		} else if (Grid.gameState.Equals ("grid")) {
			StartCoroutine (countdown (30));
		}
	}

	public IEnumerator countdown(int time){
			while (time>=0) {
				minute = time / 60;
				second = time % 60;
				min = minute.ToString ();
				sec = second.ToString ();
				if (minute < 10) {
					min = "0" + min;
				}
				if (second < 10) {
					sec = "0" + sec;
				}
				time--;
				yield return new WaitForSeconds (1);
				timeText.text = min + ":" + sec;

			}
		if (Grid.gameState.Equals ("fight")) {
			script.startGrid();
		} else if (Grid.gameState.Equals ("grid")) {
			round+=1;
			if(round<4){
				script.simulate();
			    script.startFight();
			}else if(round==4){
				finish();

			}
		}
		    
	}

	[RPC] public void selectWinner(string score,string othername){

		int myScore = int.Parse(finalScoreText.text);
		int otherScore = int.Parse (score);

		if (myScore > otherScore) {
			gameOverText.text = "VICTORY!";
		} else if (myScore < otherScore) {
			gameOverText.text = "DEFEATED!";
		} else {
			gameOverText.text = "DRAW!";
		}
		
		System.IO.File.AppendAllText ("E://savegame//scores.txt",username.text.ToString()+","+myScore+"\r\n");
		System.IO.File.AppendAllText ("E://savegame//scores.txt",othername+","+otherScore+"\r\n");

		StartCoroutine (GrowTitleFont ());
		finishGame (gameOverText.text);
	}

	public void finish(){
		if (Network.isServer) {
			GetComponent<NetworkView> ().RPC ("selectWinner", RPCMode.OthersBuffered,finalScoreText.text,username.text);
		} else if (Network.isClient) {
			GetComponent<NetworkView> ().RPC ("selectWinner", RPCMode.Server,finalScoreText.text,username.text);
		}
	}

	IEnumerator GrowTitleFont(){
		int fontsize = 0;
		gameOverText.enabled=true;
		for (double count = 0; count < 100; count++) {
			yield return new WaitForSeconds (0.0001f);
			fontsize += 1;
			gameOverText.fontSize = (int)fontsize;
		}
		script3.display ();
		script3.setVisible ();
	}

	public void finishGame(string status){

		GameObject character = GameObject.Find ("attackingRhino");
		GameObject obj=character.transform.Find("Camera").gameObject;
		Camera cam= obj.GetComponent<Camera>();
		cam.depth=2;
		CameraRotator script1=character.GetComponentInChildren<CameraRotator>();
		script1.rotating=true;

		if (status.Equals ("VICTORY!")) {
			AttackerController script2 = character.GetComponent<AttackerController> ();
			script2.attackOtherKing ();
		}

	}

}
