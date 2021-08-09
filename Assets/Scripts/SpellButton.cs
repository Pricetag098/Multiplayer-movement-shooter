using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour
{
	public bool isEnabled;

	public GameObject player;

	SpellManager spellManager;


	public float cost;

	public Image background;
	
	//public SpellManager.Spells spell;
	/*
	private void Start()
	{
		spellManager = player.GetComponent<SpellManager>();
		background.color = Color.red;
	}
	
	public void passiveSpellAdd()
	{
		print("Spell Add "+ spell.ToString());

		if(!isEnabled && spellManager.passiveMana + cost <= spellManager.maxPassiveMana)
		{
			isEnabled = true;
			spellManager.addPassiveSpell(spell, cost);
			print("Test123");
			background.color = Color.green;
		}
		else if (isEnabled)
		{
			isEnabled = false;
			spellManager.delPassiveSpell(spell, cost);
			background.color = Color.red;
		}
		
	}
	*/
}
