using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class UiManager : MonoBehaviour
{
	bool showMenu = false;
	
	public int menuScreen;
	public GameObject player;
	SpellManager spellManager;
	PlayerManager playerManager;

	NetworkManagerHUD networkManagerHUD; //hacky fix unitl i make a custom net manager

	[Header("Ui Elements")]
	public GameObject menuPanel;
	//public GameObject manaPanel;
	public GameObject crossHair;
	//public Image passiveBar;
	//public Image activeBar;
	public GameObject[] menuPanels;



	private void Awake()
	{
		spellManager = player.GetComponent<SpellManager>();
		playerManager = player.GetComponent<PlayerManager>();


		networkManagerHUD = GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetworkManagerHUD>();
	}

	public void SetMenuScreen(int screenNo)
	{
		menuScreen = screenNo;
	}


	

	void ManageMenuPanels()
	{
		menuPanels[0].SetActive(menuScreen == 0);
		menuPanels[1].SetActive(menuScreen == 1);
		menuPanels[2].SetActive(menuScreen == 2);
		menuPanels[3].SetActive(menuScreen == 3);
		menuPanels[4].SetActive(menuScreen == 4);
		networkManagerHUD.enabled = menuScreen == 4;
	}

	void ManageUi()
	{
		//passiveBar.fillAmount = (spellManager.passiveMana / spellManager.maxPassiveMana);
		//activeBar.fillAmount = (spellManager.mana / spellManager.maxMana);

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			showMenu = !showMenu;

		}
		if (showMenu)
		{
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
		Cursor.visible = showMenu;
		//playerManager.alowShooting = !showMenu;
		playerManager.mv.allowMovement = !showMenu;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		ManageUi();
		menuPanel.SetActive(showMenu);
		//manaPanel.SetActive(showMenu);
		crossHair.SetActive(!showMenu);
		
		if (showMenu)
		{
			ManageMenuPanels();
		}
		else
		{
			for(int i = 0; i < menuPanels.Length; i++)
			{
				menuPanels[i].SetActive(false);
			}
			networkManagerHUD.enabled = false;
		}
    }
	public void Exit()
	{
		Application.Quit();
	}
}
