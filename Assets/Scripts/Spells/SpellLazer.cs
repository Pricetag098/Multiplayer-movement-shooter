using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class SpellLazer : SpellManager.Spell
{
	public float damage = 60f;
	public float fireRate = 1f;
	public GameObject beamGo;
	float distanceFromFace = .1f, shootTime = .1f, spread = .001f, bulletSpeed = 20f;

	
	public override void init()
	{
		base.init();
		beamGo = Resources.Load("SpawnableProjectiles/Beam") as GameObject;
		Debug.Log(beamGo);
		isAuto = false;
	}

	public override void Cast()
	{
		base.Cast();
		
        pm.handAnimator.SetInteger("HandPos", 2);
            
		if (shootTime >= fireRate)
		{
            RaycastHit hit;
            if (rayCast(out hit, head.position))
            {
                if (hit.collider.GetComponent<HitBox>())
                {
                    hit.collider.GetComponent<HitBox>().OnHit(damage, hit.point, head.transform.forward);
                }

                //do on server later
                GameObject newBeam = GameObject.Instantiate(beamGo);
                LineRenderer lr =  newBeam.GetComponent<LineRenderer>();
                lr.SetPosition(0, palm.position);
                lr.SetPosition(1, hit.point);
                GameObject.Destroy(newBeam, 0.3f);
            }
            else
            {
                GameObject newBeam = GameObject.Instantiate(beamGo);
                LineRenderer lr = newBeam.GetComponent<LineRenderer>();
                lr.SetPosition(0, palm.position);
                lr.SetPosition(1, head.position + head.forward * 1000) ;
                GameObject.Destroy(newBeam, 0.15f);
            }
                   
                    

                    
                    


                    
            shootTime = 0;
		}
	}

	public override void tick()
	{
		base.tick();
		shootTime += Time.deltaTime;
	}





	public bool rayCast(out RaycastHit Hit,Vector3 start)
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
                    Debug.Log("Hit Player");
                }
                else
                {
                    if (rayCast(out hit,hit.point + head.transform.forward * 0.01f))
                    {
                        Debug.Log("B");
                        Hit = hit;
                        return true;
                    }
                    else 
                    {
                        Debug.Log("C");
                        Hit = hit;
                        return false; 
                    }
                    
                }


            }
            Debug.Log("A");
            Hit = hit;
            return true;
            
        }
        Debug.Log("D");
        Hit = hit;
        return false;
    }


	public override void UnCast()
	{
		base.UnCast();
	}

	public SpellLazer(PlayerManager player)
	{
		pm = player;
		init();
	}
	

	
}
