using UnityEngine;
using Mirror;
using System.Collections.Generic;

/*
	Documentation: https://mirror-networking.com/docs/Guides/NetworkBehaviour.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class Cam : NetworkBehaviour
{
	public float followSpeed = 1, sensitivity = 1;
	public Vector3 offset;
	public Transform body;


	void Start()
	{
		if (isLocalPlayer)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (isLocalPlayer)
		{


			//transform.position = Vector3.MoveTowards(transform.position, body.position + offset, followSpeed);
			float angleY = -Input.GetAxisRaw("Mouse Y") * sensitivity;
			//print(angleY);
			if (transform.rotation.eulerAngles.x + angleY > 90f && transform.rotation.eulerAngles.x + angleY < 180)
			{
				angleY = 90f - transform.rotation.eulerAngles.x;
			}
			//print(transform.rotation.eulerAngles.x);

			if (transform.rotation.eulerAngles.x + angleY < 270 && transform.rotation.eulerAngles.x + angleY > 180)
			{
				angleY = 270 - transform.rotation.eulerAngles.x;
			}



			float angleX = Input.GetAxisRaw("Mouse X") * sensitivity;
			transform.rotation = Quaternion.Euler(angleY + transform.rotation.eulerAngles.x, angleX + transform.rotation.eulerAngles.y, 0);
		}
	}
}
