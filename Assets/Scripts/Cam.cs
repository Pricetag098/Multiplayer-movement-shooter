using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
	public float followSpeed = 1, sensitivity = 1;
	public PlayerManager playerManager;
	public Vector3 offset;
	public Transform body;
	Movment mv;
	public GameObject cam;
	// Start is called before the first frame update
	void Start()
	{
		if (playerManager.isLocalPlayer)
		{
			mv = body.gameObject.GetComponent<Movment>();
		}
		else
		{
			cam.SetActive(false);
			//cam.gameObject.GetComponent<AudioListener>().enabled = false;
		}
		
	}

	public void ChangeSense(float newSense)
	{
		sensitivity = newSense;
	}

	// Update is called once per frame
	void Update()
	{
		if(playerManager.isLocalPlayer)
		{
			transform.position = Vector3.Lerp(transform.position, body.position + offset, followSpeed *Time.deltaTime);
			
			if (mv.allowMovement)
			{
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
	private void FixedUpdate()
	{
		if (playerManager.isLocalPlayer)
		{
			//transform.position =  body.position + offset;
		}
	}
}
