using UnityEngine;
using Mirror;
using System.Collections.Generic;
using UnityEngine.UI;

/*
	Documentation: https://mirror-networking.com/docs/Guides/NetworkBehaviour.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class RoomPlayer : NetworkBehaviour
{
	public string locName = "Guest";
	public GameObject server, client;
	public Image img,svrImg;

    [SyncVar]
    public int connId;
    public GameManager gameManager;

	[SyncVar]
	public string uName;

	

	[SyncVar]
	public bool svrIsReady;
    #region Start & Stop Callbacks

    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer() { }

    /// <summary>
    /// Invoked on the server when the object is unspawned
    /// <para>Useful for saving object data in persistent storage</para>
    /// </summary>
    public override void OnStopServer() { }

    /// <summary>
    /// Called on every NetworkBehaviour when it is activated on a client.
    /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
    /// </summary>
    public override void OnStartClient()
	{
		transform.parent = GameObject.FindGameObjectWithTag("RoomParent").transform;
		transform.localScale = Vector3.one;
		server.SetActive(!isLocalPlayer);
		client.SetActive(isLocalPlayer);
        gameManager = FindObjectOfType<GameManager>();
        if (isServer)
        {
            connId = connectionToClient.connectionId;
        }

    }
    


    /// <summary>
    /// This is invoked on clients when the server has caused this object to be destroyed.
    /// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
    /// </summary>
    public override void OnStopClient() { }

    /// <summary>
    /// Called when the local player object has been set up.
    /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStartLocalPlayer() { }

    /// <summary>
    /// This is invoked on behaviours that have authority, based on context and <see cref="NetworkIdentity.hasAuthority">NetworkIdentity.hasAuthority</see>.
    /// <para>This is called after <see cref="OnStartServer">OnStartServer</see> and before <see cref="OnStartClient">OnStartClient.</see></para>
    /// <para>When <see cref="NetworkIdentity.AssignClientAuthority">AssignClientAuthority</see> is called on the server, this will be called on the client that owns the object. When an object is spawned with <see cref="NetworkServer.Spawn">NetworkServer.Spawn</see> with a NetworkConnection parameter included, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStartAuthority() { }

    /// <summary>
    /// This is invoked on behaviours when authority is removed.
    /// <para>When NetworkIdentity.RemoveClientAuthority is called on the server, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStopAuthority() { }

	#endregion
	public bool isReady = false;
	
    public void changeName(string name)
    {
		locName = name;
		CMDChangeName(name);
		//FindObjectOfType<DataStorage>().uName = name;
        //print("RoomPlayer changename");
        //gameManager.CMDChangeName(name, connId);
    }
	
	[Command]
	public void CMDChangeName(string name)
	{
		uName = name;
	}

    private void Start()
    {
      gameManager = FindObjectOfType<GameManager>();
    }

	

    public void Ready()
	{
		isReady = !isReady;
		CmdReady(isReady);
        img.color = isReady ? Color.green : Color.red; 

		GetComponent<NetworkRoomPlayer>().CmdChangeReadyState(isReady);
       
	}

	[Command]
	void CmdReady(bool IsReady)
	{
		svrIsReady = IsReady;
	}
    private void Update()
    {
        if (!isLocalPlayer)
        {
           svrImg.color = svrIsReady ? Color.green : Color.red;
        }
        else
        {
            img.color = isReady ? Color.green : Color.red;

			//CMDChangeName(locName);
        }
    }
}
