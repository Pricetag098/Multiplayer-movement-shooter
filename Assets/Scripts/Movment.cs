using UnityEngine;
using Mirror;
using System.Collections.Generic;

/*
	Documentation: https://mirror-networking.com/docs/Guides/NetworkBehaviour.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class Movment : MonoBehaviour
{
   
	public float jumpForce = 1, sensitivity = 1;
	public Vector2 moveSpeed;
	public Vector2 dir;
	Rigidbody rb;
	public Transform cam;
	public PlayerManager playerManager;
	bool isLocalPlayer;


	Vector3 newVel;
	private void Start()
	{
		isLocalPlayer = playerManager.isLocalPlayer;
		rb = GetComponent<Rigidbody>();
		if (isLocalPlayer)
		{
			rb.isKinematic = false;
		}
		else
		{
			rb.isKinematic = true;
		}
	}

	private void Update()
	{
		if (isLocalPlayer)
		{

			transform.rotation = Quaternion.Euler(0, cam.rotation.eulerAngles.y, 0);
			// movement
			dir = new Vector2(
				Input.GetAxis("Horizontal"),
				Input.GetAxis("Vertical")
				).normalized;
			newVel = transform.forward * (dir.y * moveSpeed.y) + transform.right * (dir.x * moveSpeed.x);

			transform.rotation = Quaternion.Euler(transform.rotation.x, cam.rotation.eulerAngles.y, transform.rotation.z);
			if (Input.GetKeyDown(KeyCode.Space))
			{
				rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
			}
		}

	}
	private void FixedUpdate()
	{
		if (isLocalPlayer)
		{
			
			rb.velocity = new Vector3(
				newVel.x,
				rb.velocity.y,
				newVel.z
				);
		}
	}
}
