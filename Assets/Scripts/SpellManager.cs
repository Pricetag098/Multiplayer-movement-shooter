using UnityEngine;
using Mirror;
using System.Collections.Generic;
using UnityEngine.Rendering;


/*
	Documentation: https://mirror-networking.com/docs/Guides/NetworkBehaviour.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class SpellManager : NetworkBehaviour
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
	public override void OnStartClient()
	{
		//pm = GetComponent<PlayerManager>();
	}

	/// <summary>
	/// This is invoked on clients when the server has caused this object to be destroyed.
	/// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
	/// </summary>
	public override void OnStopClient() { }

	
	

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


	public Spell primaryAttackSpell;
	//temp variables



	public Transform fingerTip;

	[SyncVar]
	public float mana = 100 ;
	[SyncVar]
	public float maxMana = 100;

	

	public PlayerManager pm;

	[SyncVar]
	public bool canFly = false;
	
	

	//public Material[] playerMats;
	//public Volume vol;
	//public Camera whCam;

	[Command]
	public void spawn(GameObject go)
	{
		NetworkServer.Spawn(go,gameObject);
	}
	
	public override void OnStartLocalPlayer()
	{
		
	}


	private void Start()
	{
		primaryAttackSpell = new SpellProjectile(pm);
		bulletGo = Resources.Load("SpawnableProjectiles/MagicProjectile") as GameObject;
	}

	
	
	#region Spells

	//[System.Serializable]
	
	public class Spell
	{

		
		public PlayerManager pm;
		public SpellManager sm;
		public Transform head;
		public Movment mv;
		public Cam cam;
		public NetworkManager networkManager;
		public NetworkIdentity identity;
		public bool isLocalPlayer;
		public Transform fingerTip;

		public virtual void init()
		{
			sm = pm.GetComponent<SpellManager>();
			mv = pm.mv;
			head = mv.cam;
			cam = pm.GetComponent<Cam>();
			networkManager = pm.netMan;
			identity = pm.networkIdentity;
			fingerTip = sm.fingerTip;
			
		}
		
		
		public virtual void Cast()
		{
			
		}
		public virtual void UnCast()
		{

		}

		
	}




	/*
	//healing and defence spells

	public class PassiveHealingSpell : Spell
	{
		float healR = 10f;


		public PassiveHealingSpell(PlayerManager playerManager)
		{
			pm = playerManager;
		}


		
		public override void Cast()
		{
			
			if (pm.health < pm.maxHealth)
			{
				Debug.Log(pm);
				pm.CMDHeal(healR * Time.deltaTime);
				
			}
		}
	}


	
	public class HighlightEnemySpell : Spell
	{
		public Material defaultMat, highlitedMat;
		public Volume volume;
		Camera whCam;
		public HighlightEnemySpell(Material[] mats,Volume vol,Camera cam)
		{
			volume = vol;
			whCam = cam;
			defaultMat = mats[0];
			highlitedMat = mats[1];
		}

		
		public override void Cast()
		{
			volume.enabled = true;
			whCam.enabled = true;
			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
			for(int i = 0; i< players.Length; i++)
			{
				players[i].GetComponent<PlayerManager>().serverBody.GetComponentsInChildren<SkinnedMeshRenderer>()[0].material = highlitedMat;
			}
			
			base.Cast();
		}
		

		public override void UnCast()
		{
			volume.enabled = false;
			whCam.enabled = false;
			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
			for (int i = 0; i < players.Length; i++)
			{
				players[i].GetComponent<PlayerManager>().serverBody.GetComponentsInChildren<SkinnedMeshRenderer>()[0].material = defaultMat;
			}
		}
	}
	*/
	public class SpellProjectile : SpellManager.Spell
	{
		public float damage = 35f;
		public float fireRate = .1f;
		public GameObject bulletGo;
		float distanceFromFace = .1f, shootTime = .1f, spread = .01f, bulletSpeed = 20f;


		public override void init()
		{
			base.init();
			bulletGo = Resources.Load("SpawnableProjectiles/MagicProjectile") as GameObject;
			Debug.Log(bulletGo);
		}

		public override void Cast()
		{
			base.Cast();
			if (sm.isLocalPlayer)
			{
				if (Input.GetMouseButtonDown(0))
				{
					if (shootTime >= fireRate)
					{
						Vector3 dir = head.transform.forward;
						RaycastHit hit;
						if (Physics.Raycast(head.position, head.transform.forward, out hit, Mathf.Infinity))
						{
							dir = (hit.point - fingerTip.transform.position).normalized;
						}




						Vector3 rand = new Vector3(
							-Random.value + Random.value,
							-Random.value + Random.value,
							-Random.value + Random.value
							).normalized;

						dir = (dir + (rand * spread)).normalized * bulletSpeed;
						CMDProjectileShoot(bulletGo, dir, fingerTip, Quaternion.Euler(head.transform.forward), damage);
						//print(ammo);
						shootTime = 0;
					}
				}
				shootTime += Time.deltaTime;
			}

		}

		[Command]
		public void CMDProjectileShoot(GameObject bulletGo, Vector3 dir, Transform origin, Quaternion rotation, float damage)
		{

			GameObject bullet = NetworkManager.Instantiate(bulletGo, origin.position, rotation);
			bullet.GetComponent<Rigidbody>().velocity = dir;
			bullet.GetComponent<Projectile>().damage = damage;

			NetworkServer.Spawn(bullet, pm.networkConnection);

		}



		public override void UnCast()
		{
			base.UnCast();
		}

		public SpellProjectile(PlayerManager player)
		{
			pm = player;
			init();
		}



	}
	public float damage = 35f;
	public float fireRate = .1f;
	public GameObject bulletGo;
	float distanceFromFace = .1f, shootTime = .1f, spread = .01f, bulletSpeed = 20f;

	

	[Command]
	public void CMDProjectileShoot(GameObject bulletGo, Vector3 dir, Vector3 origin, Quaternion rotation, float damage)
	{
		bulletGo = Resources.Load("SpawnableProjectiles/MagicProjectile") as GameObject;
		GameObject bullet = NetworkManager.Instantiate(bulletGo, origin, rotation);
		bullet.GetComponent<Rigidbody>().velocity = dir;
		bullet.GetComponent<Projectile>().damage = damage;

		NetworkServer.Spawn(bullet);

	}

	#endregion




	private void Update()
	{
		if (isLocalPlayer)
		{
			if (primaryAttackSpell != null)
			{
				//primaryAttackSpell.Cast();


				if (Input.GetMouseButtonDown(0))
				{
					if (shootTime >= fireRate)
					{
						Vector3 dir = primaryAttackSpell.head.transform.forward;
						RaycastHit hit;
						if (Physics.Raycast(primaryAttackSpell.head.position, primaryAttackSpell.head.transform.forward, out hit, Mathf.Infinity))
						{
							dir = (hit.point - fingerTip.transform.position).normalized;
						}




						Vector3 rand = new Vector3(
							-Random.value + Random.value,
							-Random.value + Random.value,
							-Random.value + Random.value
							).normalized;

						dir = (dir + (rand * spread)).normalized * bulletSpeed;
						CMDProjectileShoot(bulletGo, dir, fingerTip.position, Quaternion.Euler(primaryAttackSpell.head.transform.forward), damage);
						//print(ammo);
						shootTime = 0;
					}
				}
				shootTime += Time.deltaTime;
			}
		
			else
			{
				primaryAttackSpell = new SpellProjectile(pm);
			}
			

			
			
		}
	}
}
