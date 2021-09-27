using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;

/*
	Documentation: https://mirror-networking.com/docs/Guides/NetworkBehaviour.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class GameManager : NetworkBehaviour
{
    public SyncList<int> playersConnId = new SyncList<int>();
    public SyncList<PlayerManager> playerManagers = new SyncList<PlayerManager>();

    public SyncDictionary<int, string> nameDict = new SyncDictionary<int, string>();
    public SyncDictionary<int, PlayerManager> pmDict = new SyncDictionary<int, PlayerManager>();
    public SyncDictionary<int, RoomPlayer> rmDict = new SyncDictionary<int, RoomPlayer>();

    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    [SyncVar]
    public float gameTimer;
    public float timeToStart = -10, gameDuration = 60*5;
    

    public enum GameStates { room,start,inGame,end}

    [SyncVar]
    public GameStates gameState;


    //public GameObject player1;
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
    public override void OnStartClient() { }

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

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        
    }
    
    public void OnGameStart()
    {
        
    }

    [Command(requiresAuthority =false)]
    public void CMDChangeName(string name,int conn)
    {
        //print("About to change name");
        
        if (nameDict.ContainsKey(conn))
        {
            nameDict[conn] = name;
        }
        else
        {
          //  print("adding key");
            nameDict.Add(conn, name);
        }
    }

    public void shufflePlayers()
    {
        
        SyncList<PlayerManager> tempPms = playerManagers;
        for(int i = 0; i< playerManagers.Count; i++)
        {
            int num = Random.Range(0, playerManagers.Count);
            tempPms.Add(playerManagers[num]);
            playerManagers.RemoveAt(num);
        }
        playerManagers = tempPms;

    }

    public void AssignTeams()
    {
        
        foreach(PlayerManager player in playerManagers)
        {
            player.teamCode = (playerManagers.IndexOf(player) % 2 );
        }
    }

    public void UpdateSpawnPoints()
    {
        spawnPoints.Clear();
        foreach (SpawnPoint sp in GameObject.FindObjectsOfType<SpawnPoint>(false))
        {
            spawnPoints.Add(sp);
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdInitiateGame()
    {
        gameState = GameStates.start;
        gameTimer = timeToStart;

        UpdateSpawnPoints();

        foreach (int connId in playersConnId)
        {
            playerManagers.Add(NetworkServer.connections[connId].identity.gameObject.GetComponent<PlayerManager>());
        }
        shufflePlayers();
        AssignTeams();


        foreach(PlayerManager player in playerManagers)
        {
            //RespawnPlayer(player);
            foreach (SpawnPoint spawnPoint in spawnPoints)
            {
                if (spawnPoint.teamCode == player.teamCode)
                {
                    Vector2 randPos = Random.insideUnitCircle * spawnPoint.spawnRadius;
                    RPCMovePlayer(player.networkIdentity.connectionToClient, new Vector3(
                        spawnPoint.transform.position.x + randPos.x,
                        spawnPoint.transform.position.y,
                        spawnPoint.transform.position.z + randPos.y));
                    break;
                }
            }
        }
    }

    public void GameEnd(int teamCode)
    {
        gameState = GameStates.end;
    }

    
    public void RespawnPlayer(PlayerManager player)
    {
        player.health = player.maxHealth;
        foreach(SpawnPoint spawnPoint in spawnPoints)
        {
            if(spawnPoint.teamCode == player.teamCode)
            {
                //print("found1");
                StartCoroutine(DeathTimer(5f,player,spawnPoint));
                //player.mv.cam.transform.forward = spawnPoint.transform.forward;
                break;
            }
        }
    }
    [TargetRpc]
    public void RPCMovePlayer(NetworkConnection target,Vector3 position)
    {
        target.identity.gameObject.GetComponent<PlayerManager>().mv.gameObject.transform.position = position;
    }
    
    private void Update()
    {
        if (!isServer)
        {
            return;
        }
        gameTimer += Time.deltaTime;

        switch (gameState)
        {
            case GameStates.room:
                {
                    break;
                }
            case GameStates.start:
                {
                    if(gameTimer >= 0)
                    {
                        gameState = GameStates.inGame;
                        OnGameStart();
                    }
                    else
                    {
                        
                    }
                    break;
                }
            case GameStates.inGame:
                {
                    break;
                }
            case GameStates.end:
                {
                    break;
                }
        }
    }

    [ClientRpc]
    public void RPCToggleBody(PlayerManager player,bool state)
    {
        player.mv.gameObject.SetActive(state);
        //player.clientHolster.SetActive(state);
    }







    public IEnumerator DeathTimer(float time,PlayerManager player,SpawnPoint spawnPoint)
    {
        RPCToggleBody(player, false);
        Vector2 randPos = Random.insideUnitCircle * spawnPoint.spawnRadius;
        RPCMovePlayer(player.networkIdentity.connectionToClient, new Vector3(
            spawnPoint.transform.position.x + randPos.x,
            spawnPoint.transform.position.y,
            spawnPoint.transform.position.z + randPos.y));
        yield return new WaitForSeconds(time);
        RPCToggleBody(player, true);

    }
}