using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanGun : MonoBehaviour
{

	public PlayerManager player;

	public float fireRate,damage,range;

	public Transform head;

	public GameObject bloodSplat;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (player.isLocalPlayer)
		{
			if (Input.GetMouseButtonDown(0))
			{
				RaycastHit hit;
				if (Physics.Raycast(head.position, transform.forward,out hit, range))
				{
					Debug.Log(hit.collider.transform.name);
					HitBox hitBox =  hit.collider.transform.gameObject.GetComponent<HitBox>();
					if(hitBox != null)
					{
						hitBox.OnHit(damage,hit.point,hit.normal);
					}
				}
			}
		}
    }
}
