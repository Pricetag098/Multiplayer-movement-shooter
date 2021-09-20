using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class nameDisplay : MonoBehaviour
{
    public PlayerManager pm, localPm;

    GameManager gameManager;

    public string name;

    TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        gameManager = pm.gameManager;
        localPm = Camera.main.GetComponentInParent<PlayerManager>();
        name = gameManager.nameDict[pm.connectionToClient.connectionId];
        if (pm.isLocalPlayer) { Destroy(gameObject); print("A"); }
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.LookAt(Camera.main.transform);
    }
}
