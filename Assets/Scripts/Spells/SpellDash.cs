using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpellDash : SpellManager.Spell
{
	public float charge = 5f, maxCharge = 5f;
	public float dashRange = 10f;
	public override void init()
	{
		base.init();
	}

	public override void Cast()
	{
		base.Cast();
		if(charge >= maxCharge)
		{
			RaycastHit hit;
			if(rayCast(out hit, head.position))
			{
				mv.transform.position = hit.point;
			}
			else
			{
				mv.transform.position = mv.transform.position + head.forward * dashRange;
			}

			charge = 0;
		}
	}
	public override void UnCast()
	{
		base.UnCast();
	}
	public override void tick()
	{
		base.tick();
		charge = Mathf.Clamp(charge + Time.deltaTime, 0, maxCharge);
	}

	public bool rayCast(out RaycastHit Hit, Vector3 start)
	{


		RaycastHit hit;
		if (Physics.Raycast(start, head.transform.forward, out hit, dashRange, 65))
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

	public SpellDash(PlayerManager player)
	{
		pm = player;
		init();
	}
	public override float getCharge()
	{
		return charge / maxCharge;
	}
}
