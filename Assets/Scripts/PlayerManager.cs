using UnityEngine;
using Mirror;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/*
	Documentation: https://mirror-networking.com/docs/Guides/NetworkBehaviour.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class PlayerManager : NetworkBehaviour
{
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
    public int connId;

    public GameManager gameManager;

    public nameDisplay nameDisp;

    public int test;
    public LayerMask layer;

	[SyncVar]
	public string uName = "Guest";

    [SyncVar]
    public int teamCode; 

	public NetworkManager netMan;
	public NetworkIdentity networkIdentity;
	public NetworkConnection networkConnection;

    public Animator handAnimator;

	[SyncVar]
	public bool trappable = true, canShoot = true;
	


	public GameObject serverBody, ClientBody, serverHolster, clientHolster, ui;
    public SkinnedMeshRenderer bodyRenderer;

	public SpellManager spells;

	public GameObject blood;
	public Movment mv;

	[Header("Stats")]

	

	[SyncVar]
	public float health = 100;
	public float maxHealth = 100;

	[SyncVar]
	public float shieldHealth = 0;
	float shieldMaxHealth = 0;




    
	public override void OnStartClient()
	{
        //networkIdentity.netId
        gameManager = GameObject.FindObjectOfType<GameManager>();
		netMan = GameObject.FindObjectOfType<NetworkManager>();
        serverBody.SetActive(true);
        ClientBody.SetActive(isLocalPlayer);
        serverHolster.SetActive(!isLocalPlayer);
        //if (isLocalPlayer)
        //{
            //serverBody.layer = 2;
        //}

        clientHolster.SetActive(isLocalPlayer);
		ui.SetActive(isLocalPlayer);
		
		if (isServer)
		{
			health = maxHealth;
            connId = connectionToClient.connectionId;
		}
        nameDisp.Begin();
	}
	

	[Command(requiresAuthority = false)]
	public void CMDOnTakeDmg(float dmg, Vector3 point, Vector3 dir)
	{
		health -= dmg;
		RpcOnTakeDamage(point, dir);

        if (health < 0f)
        {
            OnDeath();
        }
        
	}

    public void OnDeath()
    {
        gameManager.RespawnPlayer(this);
    }

	[ClientRpc]
	public void RpcOnTakeDamage(Vector3 point, Vector3 dir)
	{
		//Debug.Log(health);
		GameObject bloodGo = Instantiate(blood);
		bloodGo.transform.position = point;
		bloodGo.transform.forward = dir;
		Destroy(bloodGo, 5);
	}


	[Command(requiresAuthority = false)]
	public void CMDHeal(float amount)
	{
		health += amount;
		health = Mathf.Clamp(health, -1f, maxHealth);
		RpcOnHeal();
		
	}

	[Command(requiresAuthority = false)]
	public void CMDReplenishShield(float amount)
	{
		shieldHealth += amount;
		shieldHealth = Mathf.Clamp(shieldMaxHealth, -1f, shieldMaxHealth);
	}
	[Command(requiresAuthority =false)]
	public void CMDChangeTrappable(bool i)
	{
		trappable = i;
	}

	[ClientRpc]
	public void RpcOnHeal()
	{
		
	}
    [Client]
    public void Disconnect()
    {
        if(NetworkServer.active && NetworkClient.isConnected)
        {
            netMan.StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            netMan.StopClient();
        }
        SceneManager.LoadScene(0);
    }
    private void Update()
    {
        bodyRenderer.enabled = !isLocalPlayer;

        test = layer.value;

    }

    
}
