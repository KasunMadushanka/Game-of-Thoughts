using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;

public class LeaderBoard : MonoBehaviour {

	public Text player1,player2,player3,player4,player5,player6,player7,player8,player9,player10;
	public Text score1,score2,score3,score4,score5,score6,score7,score8,score9,score10;
	public Text rank1,rank2,rank3,rank4,rank5,rank6,rank7,rank8,rank9,rank10;
	public Text playerText,scoreText,rankText;
	Color32 nextColor=Color.green;

	List<string> nameList;
	string names;
	public Image panel;
	bool panelOn;

	void Start () {
		nameList = new List<string> ();
		disable ();
		display ();
	}

	public void disable(){

		panel.enabled = false;

		playerText.enabled = false;
		scoreText.enabled = false;
		rankText.enabled = false;

		player1.enabled = false;
		player2.enabled = false;
		player3.enabled = false;
		player4.enabled = false;
		player5.enabled = false;
		player6.enabled = false;
		player7.enabled = false;
		player8.enabled = false;
		player9.enabled = false;
		player10.enabled = false;

		score1.enabled = false;
		score2.enabled = false;
		score3.enabled = false;
		score4.enabled = false;
		score5.enabled = false;
		score6.enabled = false;
		score7.enabled = false;
		score8.enabled = false;
		score9.enabled = false;
		score10.enabled = false;

		rank1.enabled = false;
		rank2.enabled = false;
		rank3.enabled = false;
		rank4.enabled = false;
		rank5.enabled = false;
		rank6.enabled = false;
		rank7.enabled = false;
		rank8.enabled = false;
		rank9.enabled = false;
		rank10.enabled = false;

		panelOn = false;
	}

	public void setVisible(){

		panel.enabled = true;

		playerText.enabled = true;
		scoreText.enabled = true;
		rankText.enabled = true;

		player1.enabled = true;
		player2.enabled = true;
		player3.enabled = true;
		player4.enabled = true;
		player5.enabled = true;
		player6.enabled = true;
		player7.enabled = true;
		player8.enabled = true;
		player9.enabled = true;
		player10.enabled = true;
		
		score1.enabled = true;
		score2.enabled = true;
		score3.enabled = true;
		score4.enabled = true;
		score5.enabled = true;
		score6.enabled = true;
		score7.enabled = true;
		score8.enabled = true;
		score9.enabled = true;
		score10.enabled = true;
		
		rank1.enabled = true;
		rank2.enabled = true;
		rank3.enabled = true;
		rank4.enabled = true;
		rank5.enabled = true;
		rank6.enabled = true;
		rank7.enabled = true;
		rank8.enabled = true;
		rank9.enabled = true;
		rank10.enabled = true;

		panelOn = true;

	}

	public void switchPanel(){
		if (panelOn) {
			disable ();
		} else {
			setVisible();
		}
	}

	public void display(){

		FileInfo theSourceFile = new FileInfo ("E://savegame//scores.txt");
		StreamReader reader = theSourceFile.OpenText();

		int i = 0,l=0;

	    while (reader.ReadLine()!=null) {
			l++;
		
		}
		reader.Close ();

		string text="",name="",score="";
		string[,] list=new string[l,2];
		string[] elements = new string[2];	

		StreamReader reader1 = theSourceFile.OpenText();

		for(int p=0;p<l;p++) {
			text = reader1.ReadLine();
			elements=text.Split(',');
			list[i,0]=elements[0];
			list[i,1]=elements[1];
			i++;
		}

		reader1.Close ();

		string[] temp=new string[2];

		for (int n=0; n<list.Length/2; n++) {
			for (int j=1; j<(list.Length/2-n); j++) {
				if(int.Parse(list[j-1,1])<int.Parse(list[j,1])){
					temp[0]=list[j,0];
					temp[1]=list[j,1];
					list[j,0]=list[j-1,0];
					list[j,1]=list[j-1,1];
					list[j-1,0]=temp[0];
					list[j-1,1]=temp[1];

				}
			}
		}

		player1.text=list[0,0];
		player2.text=list[1,0];
		player3.text=list[2,0];
		player4.text=list[3,0];
		player5.text=list[4,0];
		player6.text=list[5,0];
		player7.text=list[6,0];
		player8.text=list[7,0];
		player9.text=list[8,0];
		player10.text=list[9,0];

		score1.text=list[0,1];
		score2.text=list[1,1];
	    score3.text=list[2,1];
		score4.text=list[3,1];
		score5.text=list[4,1];
		score6.text=list[5,1];
		score7.text=list[6,1];
		score8.text=list[7,1];
		score9.text=list[8,1];
		score10.text=list[9,1];
		    
	}

	public void onHighScoresClicked(){
		if (panelOn) {
			disable ();
		} else {
			display();
			setVisible();
			StartCoroutine(changeColor());
			
		}
	}

	IEnumerator changeColor(){
		nextColor = Color.green;
		while (panelOn) {
			panel.CrossFadeColor (nextColor, 2f, true, true);
			if(nextColor==Color.blue){
				nextColor=Color.green;
			}else{
				nextColor=Color.blue;
			}
			yield return new WaitForSeconds(4f);
		
		}
	}

}
