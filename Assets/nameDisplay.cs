using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class nameDisplay : MonoBehaviour
{
    public PlayerManager pm, localPm;

    GameManager gameManager;

    //public string userName;

    TextMeshPro text;
    // Start is called before the first frame update
   public void Begin()
    {
        text = GetComponent<TextMeshPro>();
        gameManager = pm.gameManager;
        localPm = Camera.main.GetComponentInParent<PlayerManager>();

        

        if (pm.isLocalPlayer) 
        { 
            Destroy(gameObject);
            //print("A");
        }
        text.text = pm.uName;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.LookAt(Camera.main.transform);
    }
}
