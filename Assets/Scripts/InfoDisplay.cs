using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoDisplay : MonoBehaviour
{
	public PlayerManager pm;
	public SpellManager sm;

	public Image health;
	public Image[] chargeDisp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		health.fillAmount = pm.health / pm.maxHealth;

		for(int i = 0; i < 3; i++)
		{
			if(sm.ablitys[i] != null)
			{
				chargeDisp[i].fillAmount = sm.ablitys[i].getCharge();
			}
			else
			{
				chargeDisp[i].fillAmount = 0;
			}
			
		}
    }
}
