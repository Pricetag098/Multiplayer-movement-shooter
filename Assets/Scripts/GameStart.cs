using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameStart : NetworkBehaviour
{
    private void Update()
    {
        if (NetworkServer.active)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CmdInitiateGame();
            Destroy(gameObject);
        }
        
    }
    
    
}
