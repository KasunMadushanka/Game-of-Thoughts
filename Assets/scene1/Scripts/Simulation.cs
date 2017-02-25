#pragma warning disable 618

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading;
using System.IO;

public class Simulation : MonoBehaviour {
	
	private string name;
	private GameObject cell;
	public Camera main;
	ScoreManager script1;
	Spawning script2;
	AttackerController script4;
	Cell script5;
	public int redCount, greenCount;
	public static bool[,] newStates=new bool[40,30];
	public static int[,] newColor=new int[40,30];
	public static Color32 color1,color2,color3,color4;
	public static Material material1,material2,material3,material4;
	bool playmode,otherPlayerReady;
	public Text waitingText,cellsLeftText,finalScoreText,gameOverText;
	private int k;
	public RawImage userImage;
	public Text username;
	string path;
	int leadingRow;
	int round;
	WWW www;

	void Start(){

		StartCoroutine (loadCells ());

		FileInfo file = new FileInfo ("E://0.jpg");
		if(file.Exists) {
			path = "file://E:0.jpg";
		} else {
			path = "file://E:1.jpg";
		}
	
		www = new WWW (path);
		userImage.texture = www.texture;

		username.text=System.IO.File.ReadAllText("E://savegame//profile.txt");

		script1 = GameObject.Find ("ButtonManager").GetComponent<ScoreManager> ();
		script2 = GameObject.Find ("NetworkManager").GetComponent<Spawning> ();

		color1 = new Color32 (123, 123, 133, 255);
		color2 = new Color32 (76, 76, 79, 255);

	}

	IEnumerator loadCells(){
		int x = 0;
		for (int i=0; i<40; i++) {
			for(int j=0;j<30;j++){
				x+=1;
				Grid.cells[i,j]=GameObject.Find(x+"");
			}
			yield return null;
		}
	}

	[RPC] void startGame(){
		if (Grid.gameState.Equals ("initial")) {
			setCamera ();
			Grid.gameState="placingKing";
			otherPlayerReady=false;
		}else if (Grid.gameState.Equals ("placingKing")) {
			Grid.gameState = "fight";
			updateTime ();
		}
		waitingText.text = "";
	}

	public void startFight(){
		Grid.gameState="fight";
		updateTime ();
	}

	public void startGrid(){
		Grid.gameState = "grid";
		updateTime ();
	}

	public void OnClickReady(){
		if (otherPlayerReady) {
			startGame();
			if(Network.isServer){
				GetComponent<NetworkView> ().RPC ("startGame", RPCMode.OthersBuffered);
			}else if(Network.isClient){
				GetComponent<NetworkView> ().RPC ("startGame", RPCMode.Server);
			}
		} else {
			waitingText.text="Waiting for other player...";
			if(Network.isServer){
				GetComponent<NetworkView> ().RPC ("updateOtherPlayerState", RPCMode.OthersBuffered);
			}else if(Network.isClient){
				GetComponent<NetworkView> ().RPC ("updateOtherPlayerState", RPCMode.Server);
			}
		}
	}

	[RPC] public void updateOtherPlayerState(){
		otherPlayerReady = true;
	}

	public void setCamera(){
		CameraSwitcher script3 = GameObject.Find ("gliderfirst(Clone)").GetComponentInChildren<CameraSwitcher>();
		script3.disable ();
		main.depth=0;

	}
	
	void updateTime(){
		script1.startTimer ();
	}

	public void goToMainMenu(){
		Network.Disconnect ();
		MasterServer.UnregisterHost ();
		Cell.pattern = "none";
		Cell.i = 0;
		Cell.j = 0;
		Grid.states=new bool[40,30];
		Grid.tempStates = new bool[40, 30];
		Grid.color = new int[40, 30];
		Grid.kingCell = new int[2];
		Grid.otherKingCell = new int[2];
		Grid.livingCellSelected = false;
		Grid.gameState = "initial";
		NetworkManager.NetworkState=null;
		RhinoController.chasingRhino="no";
		RhinoController.chasing = false;
		ScoreManager.score1 = 0;
		ScoreManager.score2 = 0;
		Simulation.newColor = new int[40, 30];
		Simulation.material1 = null;
		Simulation.material2 = null;
		Simulation.material3 = null;
		Simulation.material4 = null;

		Initiate.fade ("main menu",Color.black, 2.5f);
	}

	[RPC] IEnumerator play(){

		Color current;

		for(int k=0;k<90;k++){
			for (int i=0; i<40; i++) {
				for (int j=0; j<30; j++) {
					bool living = Grid.states [i, j];
					int count = GetLivingNeighbors (i, j);
					
					if (living && count < 2) {
						newStates [i, j] = false;
						if (i > 16 && i < 23) {
							newColor [i, j] = 3;
						} else {
							newColor [i, j] = 0;
						}
					} else if (living && (count == 2 || count == 3)) {
						newStates [i, j] = true;
						newColor [i, j] = Grid.color [i, j];
					} else if (living && count > 3) {
						newStates [i, j] = false;
						if (i > 16 && i < 23) {
							newColor [i, j] = 3;
						} else {
							newColor [i, j] = 0;
						}
					} else if (!living && count == 3) {
						newStates [i, j] = true;
						if (greenCount > redCount) {
							newColor [i, j] = 1;
						} else {
							newColor [i, j] = 2;
						}
					} else {
						if (i > 16 && i < 23) {
							newColor [i, j] = 3;
						} else {
							newColor [i, j] = 0;
						}
					}
				}
			}
			
			int x = 0;
		
			for (int i=0; i<40; i++) {
				for (int j=0; j<30; j++) {
					x += 1;
					cell = Grid.cells[i,j];
					script5=cell.GetComponent<Cell>();
					if (newColor [i, j] == 0) {
						script5.setColor(color1);
					} else if (newColor [i, j] == 1) {
						script5.setColor(Color.green);
					} else if (newColor [i, j] == 2) {
						script5.setColor(Color.blue);
					} else if (newColor [i, j] == 3) {
						script5.setColor(color2);
					}
				}
				    
					
			}

			for (int i=0; i<40; i++) {
				for (int j=0; j<30; j++) {
					if (i < 17 && newColor [i, j] == 2) {
						if (Network.isServer) {
							//waitingText.text="You are in danger";
						} else if (Network.isClient) {
							script1.updateFinalScore ();
						}
					} else if (i > 22 && newColor [i, j] == 1) {
						if (Network.isServer) {
							script1.updateFinalScore ();
						} else if (Network.isClient) {
							//waitingText.text="You are in danger";
						}
					}
				}
			}
		
			for (int i=0; i<40; i++) {
				for (int j=0; j<30; j++) {
					Grid.states [i, j] = newStates [i, j];
					Grid.color [i, j] = newColor [i, j];
				}
			}
			
			for (int i=0; i<40; i++) {
				for (int j=0; j<30; j++) {
					newStates [i, j] = false;
					newColor [i, j] = 0;
				}
			}

			yield return new WaitForSeconds(0.5f);
		}

		if (Network.isServer) {
			for (int i=0; i<40; i++) {
				for (int j=0; j<30; j++) {
					if (Grid.color [i, j] == 1) {
						leadingRow = i;
					}
				}
			}
		} else if (Network.isClient) {
			for (int i=39; i>=0; i--) {
				for (int j=0; j<30; j++) {
					if (Grid.color [i, j] == 2) {
						leadingRow = i;
					}
				}
			}
		}
		script4 = GameObject.Find ("attackingRhino").GetComponent<AttackerController> ();
		if (Network.isServer) {
			if(leadingRow>1){
				script4.setTarget(leadingRow);
			}
		} else if (Network.isClient) {
			if(leadingRow<38 && leadingRow!=0){
				script4.setTarget(leadingRow);
			}
		}
	}

	public void onPlayClick(){
		simulate ();
		if (Network.isServer) {
			GetComponent<NetworkView> ().RPC ("simulate", RPCMode.OthersBuffered);
		} else if (Network.isClient) {
			GetComponent<NetworkView> ().RPC ("simulate", RPCMode.Server);
		}
	}

	[RPC] public void simulate(){
	
		StartCoroutine (play ());


	}

	public void onPauseClick(){
		if (Network.isServer) {
			GetComponent<NetworkView> ().RPC ("stopGame", RPCMode.OthersBuffered);
		}else if(Network.isClient){
			GetComponent<NetworkView> ().RPC ("stopGame", RPCMode.Server);
		}
	}

	[RPC] public void stopGame(){
		playmode = false;
	}

	public int GetLivingNeighbors(int x, int y)
	{
		int count = 0;
		redCount = 0;
		greenCount = 0;
		
		// Check cell on the right.
		if (y + 1 <= 29) {
			if (Grid.states [x, y+1]){
				count++;
				if(Grid.color[x,y+1]==1){
					greenCount++;
				}else{
					redCount++;
				}
			}
		}
		
		// Check cell on the bottom right.
		if (x-1 >= 0 && y+1 <= 29) {
			if (Grid.states[x - 1, y + 1]){
				count++;
				if(Grid.color[x-1,y+1]==1){
					greenCount++;
				}else{
					redCount++;
				}
			}
		}
		
		// Check cell on the bottom.
		if (x - 1 >= 0) {
			if (Grid.states[x-1, y]){
				count++;
				if(Grid.color[x-1,y]==1){
					greenCount++;
				}else{
					redCount++;
				}
			}
		}
		
		// Check cell on the bottom left.
		if (x - 1 >= 0 && y - 1 >= 0) {
			if (Grid.states[x - 1, y - 1]){
				count++;
				if(Grid.color[x-1,y-1]==1){
					greenCount++;
				}else{
					redCount++;
				}
			}
		}
		
		// Check cell on the left.
		if (y - 1 >= 0) {
			if (Grid.states[x, y-1]){
				count++;
				if(Grid.color[x,y-1]==1){
					greenCount++;
				}else{
					redCount++;
				}
			}
		}
		
		// Check cell on the top left.
		if (x + 1 <= 39 && y - 1 >= 0) {
			if (Grid.states[x + 1, y - 1]){
				count++;
				if(Grid.color[x+1,y-1]==1){
					greenCount++;
				}else{
					redCount++;
				}
			}
		}
		
		// Check cell on the top.
		if (x + 1 <= 39) {
			if (Grid.states[x+1, y ]){
				count++;
				if(Grid.color[x+1,y]==1){
					greenCount++;
				}else{
					redCount++;
				}
			}
		}
		
		// Check cell on the top right.
		if (x + 1 <= 39 && y +1 <= 29) {
			if (Grid.states[x + 1, y + 1]){
				count++;
				if(Grid.color[x+1,y+1]==1){
					greenCount++;
				}else{
					redCount++;
				}
			}
		}
		
		return count;
	}

	public void setCell(){
		Cell.pattern = "cell";
	}

	public void setBlinker(){
		Cell.pattern = "blinker";
	}
	
	public void setBeacon(){
		Cell.pattern = "beacon";
	}
	
	public void setGlider(){
		Cell.pattern = "glider";
	}

	public void setSpaceship(){
		Cell.pattern = "spaceship";
	}
	
	public void setPentomino(){
		Cell.pattern = "r-pentomino";
	}
	
	public void setDiehard1(){
		Cell.pattern = "diehard1";
	}

	public void setDiehard2(){
		Cell.pattern = "diehard2";
	}
	
	public void setBlock(){
		Cell.pattern = "block";
	}
	
	public void setBeehive(){
		Cell.pattern = "beehive";
	}

	public void setBoat(){
		Cell.pattern = "boat";
	}
	
}
