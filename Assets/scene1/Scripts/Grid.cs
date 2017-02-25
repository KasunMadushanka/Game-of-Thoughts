using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	public static GameObject[,] cells = new GameObject[40, 30];
	public static bool[,] states=new bool[40,30];
	public static bool[,] tempStates=new bool[40,30];
	public static int[,] color = new int[40, 30];
	public static int[] kingCell = new int[2];
	public static int[] otherKingCell = new int[2];
	public static bool livingCellSelected;
	public static string gameState="initial";
	
}
