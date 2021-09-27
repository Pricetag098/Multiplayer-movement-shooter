using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellBarrier : SpellManager.Spell
{
	
	string objectPath = "SpawnableProjectiles/Barrier";


	public override void init()
	{
		base.init();
		isAuto = false;

		
		
	}

	public override void Cast()
	{
		base.Cast();
		RaycastHit hit;

		
		if (rayCast(out hit, head.position))
		{
			sm.CMDSpawnObject(objectPath, hit.point);
		}
	}
	public override void UnCast()
	{
		base.UnCast();
	}

	public override void tick()
	{
		base.tick();
	}

	public SpellBarrier(PlayerManager player)
	{
		pm = player;
		init();
	}

	public bool rayCast(out RaycastHit Hit, Vector3 start)
	{


		RaycastHit hit;
		if (Physics.Raycast(start, head.transform.forward, out hit, Mathf.Infinity, 65))
		{
			if (hit.collider.GetComponent<HitBox>())
			{
				if (hit.collider.GetComponent<HitBox>().player != pm)
				{
					Hit = hit;
					return true;
				}
				else
				{
					if (rayCast(out hit, hit.point + head.transform.forward * 0.01f))
					{
						Hit = hit;
						return true;
					}
					else
					{
						Hit = hit;
						return false;
					}

				}


			}
			else
			{
				Hit = hit;
				return true;
			}


		}
		Hit = hit;
		return false;
	}
}
