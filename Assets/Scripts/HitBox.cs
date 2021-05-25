using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{

	public enum HitBoxTypes { head, body, arm, leg }
	public HitBoxTypes type;


	public PlayerManager player;


	

		

	public void OnHit(float dmg, Vector3 point, Vector3 dir)
	{
		switch (type)
		{
			case HitBoxTypes.head:
				{
					player.CMDOnTakeDmg(dmg * 2, point, dir);
					break;
				}
			case HitBoxTypes.body:
				{
					player.CMDOnTakeDmg(dmg * 1, point, dir);
					break;
				}
			case HitBoxTypes.arm:
				{
					player.CMDOnTakeDmg(dmg * .75f, point, dir);
					break;
				}
			case HitBoxTypes.leg:
				{
					player.CMDOnTakeDmg(dmg * .5f, point, dir);
					break;
				}
		}
	}
}
