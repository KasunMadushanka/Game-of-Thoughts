using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	private const string typeName = "UniqueGameName";
	private const string gameName = "Game of Thoughts";
	private bool isRefreshingHostList = false;
	private HostData[] hostList;
	public Image cellimage;
	public static string NetworkState;
	Spawning script;


    void Awake(){

		//MasterServer.ipAddress="127.0.0.1";
		//MasterServer.port = 23466;
		//Network.natFacilitatorIP = "127.0.0.1";
		//Network.natFacilitatorPort = 50005;
	}

	void Start(){
		script = GetComponent<Spawning> ();
	}

	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if(NetworkState.Equals("server")){
				if (GUI.Button(new Rect(150, 100, 250, 100), "Start Server"))
					StartServer();
			}else if(NetworkState.Equals("client")){
				if (GUI.Button(new Rect(150, 250, 250, 100), "Refresh Hosts"))
					RefreshHostList();
				if (hostList != null)
				{
					for (int i = 0; i < hostList.Length; i++)
					{
						if (GUI.Button(new Rect(430, 100 + (110 * i), 300, 100), hostList[i].gameName))
							JoinServer(hostList[i]);
					}
				}
			}

		}
	}

	private void StartServer()
	{
		Network.InitializeServer(2, 25000,false);
		MasterServer.RegisterHost(typeName, gameName);
	}
	
	void OnServerInitialized()
	{
		script.SpawnPlayer ();
	}
	
	
	void Update()
	{
		if (isRefreshingHostList && MasterServer.PollHostList().Length > 0)
		{
			isRefreshingHostList = false;
			hostList = MasterServer.PollHostList();
		}
	}
	
	private void RefreshHostList()
	{
		if (!isRefreshingHostList)
		{
			isRefreshingHostList = true;
			MasterServer.RequestHostList(typeName);
		}
	}
	
	
	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}
	
	void OnConnectedToServer()
	{
		script.SpawnPlayer ();
	}
	
}
