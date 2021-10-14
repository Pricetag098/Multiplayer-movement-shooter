using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class SpellWallHack : SpellManager.Spell
{
	public Material defaultMat, highlitedMat;
	public Volume volume;
	Camera whCam;

	bool running;

	float timeElapsed = 0, runTime = 3f;

	public float charge = 5f, maxCharge = 5f;

	public override void init()
	{
		base.init();
		volume = cam.GetComponentInChildren<Volume>();
		defaultMat = Resources.Load("Mats/Body") as Material;
		highlitedMat = Resources.Load("Mats/Highlighted") as Material;
		whCam = volume.transform.parent.GetChild(1).GetComponent<Camera>();
	}

	public override void Cast()
	{
		base.Cast();
		
		if (!running && charge >= maxCharge)
		{
			pm.handAnimator.SetTrigger("LeftPoint");
			running = true;
			volume.enabled = true;
			whCam.enabled = true;
			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
			for (int i = 0; i < players.Length; i++)
			{
				players[i].GetComponent<PlayerManager>().serverBody.GetComponentsInChildren<SkinnedMeshRenderer>()[0].material = highlitedMat;
			}
			timeElapsed = 0;
		}
		
	}

	public override void tick()
	{
		base.tick();
		if (running)
		{
			timeElapsed = Mathf.Clamp(timeElapsed + Time.deltaTime, 0, runTime);
			if(timeElapsed == runTime)
			{
				UnCast();
			}
		}
		else
		{
			charge = Mathf.Clamp(charge + Time.deltaTime, 0, maxCharge);
		}
	}

	public override void UnCast()
	{
		base.UnCast();
		volume.enabled = false;
		whCam.enabled = false;
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < players.Length; i++)
		{
			players[i].GetComponent<PlayerManager>().serverBody.GetComponentsInChildren<SkinnedMeshRenderer>()[0].material = defaultMat;
		}
		running = false;
		charge = 0;
	}

	public SpellWallHack(PlayerManager player)
	{
		pm = player;
		init();
	}
	public override float getCharge()
	{
		if (!running)
		{
			return charge / maxCharge;
		}
		else
		{
			return (runTime - timeElapsed) / runTime;
		}
	}
}
