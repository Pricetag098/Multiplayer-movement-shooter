using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomUi : MonoBehaviour
{
    RoomManager rm;
    // Start is called before the first frame update
    void Start()
    {
        rm = GameObject.FindGameObjectWithTag("NetManager").GetComponent<RoomManager>();
    }

    
    public void changeIP(string Ip)
    {
        if (rm == null)
        {
            rm = GameObject.FindGameObjectWithTag("NetManager").GetComponent<RoomManager>();
        }
        rm.ChangeIp(Ip);
    }
    public void host()
    {
        if (rm == null)
        {
            rm = GameObject.FindGameObjectWithTag("NetManager").GetComponent<RoomManager>();
        }
        rm.StartHost();
    }
    public void Server()
    {
        if (rm == null)
        {
            rm = GameObject.FindGameObjectWithTag("NetManager").GetComponent<RoomManager>();
        }
        rm.StartServer();
    }
    public void client()
    {
        if (rm == null)
        {
            rm = GameObject.FindGameObjectWithTag("NetManager").GetComponent<RoomManager>();
        }
        rm.StartClient();
    }
}
