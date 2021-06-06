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

	//temp variables
	

	[SyncVar]
	public float passiveMana = 0;
	[SyncVar]
	public float maxPassiveMana = 100;

	

	[SyncVar]
	public float mana = 100 ;
	[SyncVar]
	public float maxMana = 100;

	

	public PlayerManager pm;

	[SyncVar]
	public bool canFly = false;
	
	
	public List<Spell> passiveSpells = new List<Spell>();


	
	public List<Spells> pSpellList = new List<Spells>(); //for hacky solution should realy fix

	public Material[] playerMats;
	public Volume vol;
	

	public enum Spells
	{
		nonPassive,
		passiveHealing, //done
		pasiveShielding,
		activeShielding,
		flight, //done
		speedBoost, //done
		unTrapable, //done
		fastFiring, //done
		aimAssist, 
		lightRay,
		highlightPlayers
	}

	public override void OnStartLocalPlayer()
	{
		
	}


	private void Start()
	{
			
	}


	//[Command(requiresAuthority = false)]
	public void addPassiveSpell(Spells spell, float cost)
	{
		pSpellList.Add(spell);
		print("Do THe thing");
		switch (spell)
		{
			default:
				{
					print("found the error");
					passiveSpells.Add(new Spell());
					break;
				}

			case Spells.passiveHealing:
				{
					PassiveHealingSpell passiveHealingSpell = new PassiveHealingSpell(pm);
					passiveSpells.Add(passiveHealingSpell);
					break;
				}
			case Spells.highlightPlayers:
				{
					passiveSpells.Add(new HighlightEnemySpell(playerMats,vol));
					print("DID THing");
					break;
				}
			case Spells.flight:
				{
					passiveSpells.Add(new FlightSpell(pm));
					break;
				}
			case Spells.speedBoost:
				{
					passiveSpells.Add(new AccelerateSpell(pm));
					break;
				}
			case Spells.unTrapable:
				{
					passiveSpells.Add(new IgnoreTrapsSpell(pm));
					break;
				}
			case Spells.fastFiring:
				{
					passiveSpells.Add(new FireRateBoostSpell(pm));
					break;
				}
		}
		passiveMana += cost;
	}

	[Command(requiresAuthority = false)]
	public void delPassiveSpell(Spells spell, float cost)
	{
		int i =	pSpellList.IndexOf(spell);
		passiveSpells[i].UnCast();
		pSpellList.RemoveAt(i);
		passiveSpells.RemoveAt(i);
		passiveMana -= cost;
	}



	#region Spells


	// spell classes
	public class Spell
	{
		
		
		public PlayerManager pm;
		

		
		public virtual void Cast()
		{
			print("CASTING COUCH");
		}
		public virtual void UnCast()
		{

		}
		
	}
	

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

		public HighlightEnemySpell(Material[] mats,Volume vol)
		{
			volume = vol;
			defaultMat = mats[0];
			highlitedMat = mats[1];
		}

		
		public override void Cast()
		{
			volume.enabled = true;
			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
			for(int i = 0; i< players.Length; i++)
			{
				players[i].GetComponent<PlayerManager>().serverBody.GetComponentsInChildren<SkinnedMeshRenderer>()[0].material = highlitedMat;
			}
			print("CASTING hl");
			base.Cast();
		}
		

		public override void UnCast()
		{
			volume.enabled = false;
			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
			for (int i = 0; i < players.Length; i++)
			{
				players[i].GetComponent<PlayerManager>().serverBody.GetComponentsInChildren<SkinnedMeshRenderer>()[0].material = defaultMat;
			}
		}
	}

	public class FlightSpell : Spell
	{
		public FlightSpell(PlayerManager player)
		{
			pm = player;
		}

		public override void Cast()
		{
			pm.mv.moveType = Movment.MoveTypes.flight;
		}
		public override void UnCast()
		{
			pm.mv.moveType = Movment.MoveTypes.defult;
		}
	}

	public class AccelerateSpell : Spell
	{
		Vector3 regSpeed;
		Vector3 boostedSpeed;

		public AccelerateSpell(PlayerManager player)
		{
			pm = player;
			regSpeed = pm.mv.moveSpeed;
			boostedSpeed = regSpeed * 1.5f;
		}
		public override void Cast()
		{
			pm.mv.moveSpeed = boostedSpeed;
		}
		public override void UnCast()
		{
			pm.mv.moveSpeed = regSpeed;
		}
	}

	public class IgnoreTrapsSpell : Spell
	{
		public IgnoreTrapsSpell(PlayerManager player)
		{
			pm = player;
		}

		public override void Cast()
		{
			pm.CMDChangeTrappable(false);
		}
		public override void UnCast()
		{
			pm.CMDChangeTrappable(true);
		}

	}
	public class FireRateBoostSpell : Spell
	{

		public FireRateBoostSpell(PlayerManager player)
		{
			pm = player;
		}

		public override void Cast()
		{
			pm.fireRateMod = 1.3f;
		}
		public override void UnCast()
		{
			pm.fireRateMod = 1f;
		}
	}
	


	#endregion




	private void Update()
	{
		if (isLocalPlayer)
		{
			
			

			for(int i = 0 ; i < passiveSpells.Count; i++)
			{
				print(i);
				passiveSpells[i].Cast();
			}
			
		}
	}
}
