using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class SpellProjectile : SpellManager.Spell
{
	public float damage = 35f;
	public float fireRate = .3f;
	public GameObject bulletGo;
	float distanceFromFace = .1f, shootTime = .1f, spread = .001f, bulletSpeed = 20f;

	
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
            pm.handAnimator.SetInteger("HandPos", 1);
            if (Input.GetMouseButton(0))
			{
				if (shootTime >= fireRate)
				{
					Vector3 dir = head.transform.forward;
                    /*
					RaycastHit hit;
                    if(rayCast(out hit,head.position))
					//if (Physics.Raycast(head.position, head.transform.forward, out hit, Mathf.Infinity,65))
					{
						dir = ((hit.point + head.transform.forward *0.01f) - fingerTip.transform.position).normalized;
					}
                    */



					Vector3 rand = new Vector3(
						-Random.value + Random.value,
						-Random.value + Random.value,
						-Random.value + Random.value
						).normalized;

                    dir = (dir + (rand * spread)).normalized * bulletSpeed; //+ pm.mv.rb.velocity;

					//do on server
					sm.CMDProjectileShoot(bulletGo, dir, head.position, Quaternion.Euler(head.transform.forward), damage);
					//print(ammo);
					shootTime = 0;
				}
			}
			shootTime += Time.deltaTime;
		}
		
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


        }
        Hit = hit;
        return false;
    }
    //moved to spell Manager
    /*
	[Command]
	public void CMDProjectileShoot(GameObject bulletGo, Vector3 dir, Vector3 origin, Quaternion rotation, float damage)
	{
		bulletGo = Resources.Load("SpawnableProjectiles/MagicProjectile") as GameObject;
		GameObject bullet = NetworkManager.Instantiate(bulletGo, origin, rotation);
		bullet.GetComponent<Rigidbody>().velocity = dir;
		bullet.GetComponent<Projectile>().damage = damage;

		NetworkServer.Spawn(bullet);

	}
	*/


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
