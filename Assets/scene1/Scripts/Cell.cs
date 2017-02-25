using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Cell : MonoBehaviour {
	
	private Vector3 screenPoint;
	private int row,column;
	GameObject cell;
	string name;
	public static string pattern="none";
	private int id,number,patternCount=1,patternDirection=1;
	string cellname;
	int[] members=new int[9];
	bool onHover,exist,exit;
	private Color32 color1,color2;
	Cell script1;
	public static int i,j;
	bool player1,player2;
	Simulation script2;
	Spawning script3;
	ScoreManager script4;
	float fadetime=0.3f;
	public float time=0;
	public bool changingColor;
	Color fromColor,toColor;

	void Start () {
		script2=GameObject.Find ("ButtonManager").GetComponent<Simulation> ();
		script3=GameObject.Find ("NetworkManager").GetComponent<Spawning> ();
		script4=GameObject.Find ("ButtonManager").GetComponent<ScoreManager> ();
		color1 = new Color32 (123, 123, 133, 255);
		findPosition ();
	}
	
	void findPosition(){
		name = this.gameObject.name;
		id = int.Parse (name)-1;
		int x = id / 30;
		row = x;
		int y = id % 30;
		column = y;

	}

	int getRow(int j){
		j -= 1;
		int x = j / 30;
		return x;

	}

	int getColumn(int j){
		j -= 1;
		int y = j % 30;
		return y;
	}

	public void setColor(Color color){
		changingColor = true;
		toColor = color;
		fromColor = GetComponent<Renderer> ().material.color;
		time = 0;
	}

	void Update () {
		if (changingColor){ 
			if(time<=1){
				time += Time.deltaTime / fadetime; 
				GetComponent<Renderer>().material.color = Color.Lerp(fromColor,toColor , time);
			}else{
				changingColor=false;
			}
		}
	}

	void createPattern(bool state){
		exist = true;
		if (pattern.Equals ("cell")) {
			members [0] = id + 1;
			patternCount=1;
		}else if (pattern.Equals ("blinker")) {
			if(patternDirection==1 || patternDirection==3){
				members [0] = id + 1;
				members [1] = id;
				members [2] = id+2;
			}else if(patternDirection==2 || patternDirection==4){
				members [0] = id + 1;
				members [1] = id+31;
				members [2] = id-29;
			}
			patternCount=3;
		} else if (pattern.Equals ("beacon")) {
			if(patternDirection==1){
				members [0] = id-30;
				members [1] = id-31;
				members [2] = id-1;
				members [3] = id + 32;
				members [4] = id+62;
				members [5] = id+61;
			}else if(patternDirection==2){
				members [0] = id-59;
				members [1] = id-58;
				members [2] = id-28;
				members [3] = id -1;
				members [4] = id+29;
				members [5] = id+30;
			}else if(patternDirection==3){
				members [0] = id-59;
				members [1] = id-60;
				members [2] = id-30;
				members [3] = id +3;
				members [4] = id+33;
				members [5] = id+32;
			}else if(patternDirection==4){
				members [0] = id+30;
				members [1] = id+60;
				members [2] = id+61;
				members [3] = id -28;
				members [4] = id-27;
				members [5] = id+3;
			}
			patternCount=6;
		} else if (pattern.Equals ("glider")) {
			if(patternDirection==1){
				members [0] = id -28;
				members [1] = id -29;
				members [2] = id -30;
				members [3] = id;
				members [4] = id +31;
			}else if(patternDirection==2){
				members [0] = id;
				members [1] = id -29;
				members [2] = id -28;
				members [3] = id+2;
				members [4] = id +32;
			}else if(patternDirection==3){
				members [0] = id-29;
				members [1] = id +2;
				members [2] = id +30;
				members [3] = id+31;
				members [4] = id +32;
			}else if(patternDirection==4){
				members [0] = id+30;
				members [1] = id +31;
				members [2] = id;
				members [3] = id+2;
				members [4] = id -30;
			}
			patternCount=5;
		} else if (pattern.Equals ("spaceship")) {
			if(patternDirection==1){
				members [0] = id -60;
				members [1] = id -59;
				members [2] = id -58;
				members [3] = id -57;
				members [4] = id -27;
				members [5] = id + 3;
				members [6] = id + 32;
				members [7] = id + 29;
				members [8] = id -31;
			}else if(patternDirection==2){
				members [0] = id -60;
				members [1] = id -58;
				members [2] = id +3;
				members [3] = id;
				members [4] = id +33;
				members [5] = id + 63;
				members [6] = id + 62;
				members [7] = id + 61;
				members [8] = id +30;
			}else if(patternDirection==3){
				members [0] = id -30;
				members [1] = id -27;
				members [2] = id -1;
				members [3] = id+29;
				members [4] = id +59;
				members [5] = id + 60;
				members [6] = id + 61;
				members [7] = id + 62;
				members [8] = id +33;
			}else if(patternDirection==4){
				members [0] = id -59;
				members [1] = id -60;
				members [2] = id -61;
				members [3] = id-31;
				members [4] = id -1;
				members [5] = id +29;
				members [6] = id + 60;
				members [7] = id + 62;
				members [8] = id -28;
			}
			patternCount=9;
		} else if (pattern.Equals ("r-pentomino")) {
			if(patternDirection==1){
				members [0] = id +1;
				members [1] = id+2;
				members [2] = id-29;
				members [3] = id +31;
				members [4] = id + 30;
			}else if(patternDirection==2){
				members [0] = id + 1;
				members [1] = id;
				members [2] = id + 2;
				members [3] = id - 30;
				members [4] = id + 31;
			}else if(patternDirection==3){
				members [0] = id + 1;
				members [1] = id;
				members [2] = id-29;
				members [3] = id - 28;
				members [4] = id + 31;
			}else if(patternDirection==4){
				members [0] = id +1;
				members [1] = id;
				members [2] = id + 2;
				members [3] = id -29;
				members [4] = id+32;
			}
			patternCount=5;
		} else if (pattern.Equals ("diehard1")) {
			if(patternDirection==1){
				members [0] = id + 1;
				members [1] = id;
				members [2] = id - 29;
			}else if(patternDirection==2){
				members [0] = id + 1;
				members [1] = id;
				members [2] = id +31;
			}else if(patternDirection==3){
				members [0] = id + 1;
				members [1] = id;
				members [2] = id +31;
			}else if(patternDirection==4){
				members [0] = id + 1;
				members [1] = id-29;
				members [2] = id;
			}
		} else if (pattern.Equals ("diehard2")) {
			if(patternDirection==1){
				members [0] = id-28;
				members [1] = id-29;
				members [2] = id-30;
				members [3] = id + 31;
			}else if(patternDirection==2){
				members [0] = id;
				members [1] = id+2;
				members [2] = id -28;
				members [3] = id + 32;
			}else if(patternDirection==3){
				members [0] = id-29;
				members [1] = id+30;
				members [2] = id + 31;
				members [3] = id + 32;
			}else if(patternDirection==4){
				members [0] = id;
				members [1] = id+2;
				members [2] = id -30;
				members [3] = id + 30;
			}
			patternCount=4;
		} else if (pattern.Equals ("block")) {
			members [0] = id + 1;
			members [1] = id + 2;
			members [2] = id + 31;
			members [3] = id + 32;
			patternCount=4;
		} else if (pattern.Equals ("beehive")) {
			if(patternDirection==1 || patternDirection==3){
				members [0] = id -29;
				members [1] = id + 31;
				members [2] = id+2;
				members [3] = id+30;
				members [4] = id -30;
				members [5] = id -1 ;
			}else if(patternDirection==2 || patternDirection==4){
				members [0] = id -29;
				members [1] = id + 2;
				members [2] = id;
				members [3] = id + 32;
				members [4] = id + 30;
				members [5] = id + 61;
			}
			patternCount=6;
		} else if (pattern.Equals ("boat")) {
			if(patternDirection==1){
				members [0] = id -29;
				members [1] = id;
				members [2] = id + 2;
				members [3] = id + 32;
				members [4] = id + 31;
			}else if(patternDirection==2){
				members [0] = id -29;
				members [1] = id;
				members [2] = id + 2;
				members [3] = id + 31;
				members [4] = id + 30;
			}else if(patternDirection==3){
				members [0] = id -29;
				members [1] = id -30;
				members [2] = id;
				members [3] = id + 2;
				members [4] = id + 31;
			}else if(patternDirection==4){
				members [0] = id-29;
				members [1] = id-28;
				members [2] = id +2;
				members [3] = id;
				members [4] = id + 31;
			}
			patternCount=5;
		}
		colorCells (patternCount,state);
	}

	void colorCells(int length,bool state){

		for(int i=0;i<length;i++){
			if((members[i]>0 && members[i]<510) || (members[i]>690 && members[i]<1201)){
				if(Grid.states[getRow(members[i]),getColumn(members[i])]==true){
					exist=false;
				}
			}else{
				exist=false;
			}
		}
		
		if(exist){
			for(int i=0;i<length;i++){
				cellname=members[i]+"";
				cell=GameObject.Find(cellname);
				if(state){
					if(Network.isServer && row<17){
						cell.GetComponent<Renderer> ().material.color = Color.black;
					}else if(Network.isClient && row>22){
						cell.GetComponent<Renderer> ().material.color = Color.black;
					}
				}else{
					cell.GetComponent<Renderer> ().material.color=color1;
				}
			}
		}
		onHover = true;
	}

	void OnMouseOver(){
		if(Grid.gameState.Equals ("placingKing") && ((Network.isServer && (row==1 && (column!=0 && column!=29)) || (Network.isClient && (row==38 && ((column!=0 && column!=29))))))){
			if(!Grid.livingCellSelected){
				if(!onHover){
					GetComponent<Renderer>().material.color=Color.black;
					onHover=true;
				}
			}
		}else if (Grid.gameState.Equals ("grid") && Grid.states[row,column]==false) {
			if (!onHover) {
				createPattern (true);
			}
			if (Input.GetKeyDown(KeyCode.Mouse1)) {
				createPattern (false);
				onHover=false;
				if (patternDirection == 4) {
					patternDirection = 1;
				} else {
					patternDirection++;
				}

			}   
		}
	}

	void OnMouseExit(){
		if(Grid.gameState.Equals ("placingKing") && ((Network.isServer && (row==1 && (column!=0 && column!=29)) || (Network.isClient && (row==38 && (column!=0 && column!=29)))))){
			GetComponent<Renderer>().material.color=color1;
			onHover=false;
		}else if (Grid.gameState.Equals ("grid") && Grid.states[row,column]==false) {
			createPattern (false);
			onHover = false;
		}
	}

	void OnMouseDown(){
		if (Grid.gameState.Equals ("placingKing") && ((Network.isServer && (row==1 && (column!=0 && column!=29)) || (Network.isClient && (row==38 && (column!=0 && column!=29)))))) {
			if(!Grid.livingCellSelected){
				script3.spawnKing(this.transform.position);
				Grid.kingCell [0] = row;
				Grid.kingCell [1] = column;
				Grid.livingCellSelected=true;
				if(Network.isServer){
					GetComponent<NetworkView> ().RPC ("setOtherKingCell", RPCMode.OthersBuffered);
				}else if(Network.isClient){
					GetComponent<NetworkView> ().RPC ("setOtherKingCell", RPCMode.Server);
				}
			}
		}else if (Grid.gameState.Equals ("grid")) {
			bool cellExists=true;
			for(int i=0;i<patternCount;i++){
				if(!((members[i]>0 && members[i]<510) || (members[i]>690 && members[i]<1201))){
					cellExists=false;
				}
			}
			if(cellExists){
				int count = int.Parse (script2.cellsLeftText.text);
				if (count >= patternCount) {
					for (int i=0; i<patternCount; i++) {
						script1 = GameObject.Find (members [i] + "").GetComponent<Cell> ();
						script1.setActive ();
						script4.decreaseCellScore();
					}
				} else {
					script2.waitingText.text = "No enough cells left!";
				}
			}
		}
	}

	[RPC] void setOtherKingCell(){
		Grid.otherKingCell [0] = row;
		Grid.otherKingCell [1] = column;
	} 

	void setActive(){ 
		if (Grid.gameState.Equals ("grid")) {
			if (Network.isServer && row < 17) {
				activate ();
				GetComponent<NetworkView> ().RPC ("activate", RPCMode.OthersBuffered);
			} else if (Network.isClient && row > 22) {
				activate ();
				GetComponent<NetworkView> ().RPC ("activate", RPCMode.Server);
			}
		}
	}

	[RPC] void activate(){ 

			if (row < 17 ) { 
				this.GetComponent<Renderer> ().material.color = Color.green;
			    Grid.states[row, column] = true;
				Grid.color [row, column] = 1;
			} else if (row > 22) {
				this.GetComponent<Renderer> ().material.color = Color.blue;
				Grid.states [row, column] = true;
				Grid.color [row, column] = 2;
			} else if (row > 16 && row < 23) {
				Grid.color [row, column] = 3;
			}
	}
	
}