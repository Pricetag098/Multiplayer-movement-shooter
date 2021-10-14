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
	
	public SpellManager.Spell spell;

  
    public enum SpellTypes { attack,ability}
    public SpellTypes spellType;

	public enum Spells
    {
        Lazer,
        Projectile,
		Barrier,
		WallHack,
		Dash
    }
    public Spells spellSelected;

    private void Start()
	{
		spellManager = player.GetComponent<SpellManager>();
		background.color = Color.red;

        switch (spellSelected)
        {
            case Spells.Lazer:
                {
                    spell = new SpellLazer(player.GetComponent<PlayerManager>());
                    break;
                }
            case Spells.Projectile:
                {
                    spell = new SpellProjectile(player.GetComponent<PlayerManager>());
                    break;
                }
			case Spells.Barrier:
				{
					spell = new SpellBarrier(player.GetComponent<PlayerManager>());
					break;
				}
			case Spells.WallHack:
				{
					spell = new SpellWallHack(player.GetComponent<PlayerManager>());
					break;
				}
			case Spells.Dash:
				{
					spell = new SpellDash(player.GetComponent<PlayerManager>());
					break;
				}

		}
        
	}

    public void onAddSpell()
    {
        if (isEnabled)
        {
            switch (spellType)
            {
                case SpellTypes.attack:
                    {
                        spellManager.primaryAttackSpell = null;
                        break;
                    }
				case SpellTypes.ability:
					{
						for (int i = 0; i < 3; i++)
						{
							if (spellManager.ablitys[i] == spell)
							{
								spellManager.ablitys[i].UnCast();
								spellManager.ablitys[i] = null;
								break;
							}
						}
						break;
					}
            }
        }
        else
        {
            switch (spellType)
            {
                case SpellTypes.attack:
                    {
                        spellManager.primaryAttackSpell = spell;
                        break;
                    }
				case SpellTypes.ability:
					{
						for(int i = 0; i < 3; i++)
						{
							if(spellManager.ablitys[i] == null)
							{
								spellManager.ablitys[i] = spell;
								break;
							}
						}
						break;
					}
			}
        }
        
    }
    private void Update()
    {
        switch (spellType)
        {
            case SpellTypes.attack:
                {
                    isEnabled = spellManager.primaryAttackSpell == spell;
                    break;
                }
			case SpellTypes.ability:
				{
					for (int i = 0; i < 3; i++)
					{
						if (spellManager.ablitys[i] == spell)
						{
							isEnabled = true;
							break;
						}
						else
						{
							isEnabled = false;
						}
					}
					break;
				}
		}
        if (isEnabled) { background.color = Color.green; }
        else { background.color = Color.red; }
    }
    /*
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
