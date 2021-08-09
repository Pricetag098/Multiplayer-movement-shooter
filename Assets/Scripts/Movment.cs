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
	
	public GameObject bodyClient;
	public float jumpForce = 1, sensitivity = 1, airControl = .25f;
	public Vector3 moveSpeed;
	public Vector3 dir;
	public Rigidbody rb;
	public Transform cam;
	public PlayerManager playerManager;
	bool isLocalPlayer;
	public bool isSliding;
	public LayerMask whatIsGround;
	public float GCRadius;
	public Transform feet;

	public bool allowMovement;

	public enum MoveTypes { defult, flight, aircontrol}
	public MoveTypes moveType;
	
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
		if (isLocalPlayer && allowMovement)
		{
			switch (moveType)
			{
				case MoveTypes.defult:
					{
						rb.useGravity = true;
						transform.rotation = Quaternion.Euler(0, cam.rotation.eulerAngles.y, 0);
						
						dir = new Vector3(
							Input.GetAxis("Horizontal"),
							Input.GetAxis("Vertical"),
							0
							).normalized;
						newVel = transform.forward * (dir.y * moveSpeed.y) + transform.right * (dir.x * moveSpeed.x);

						transform.rotation = Quaternion.Euler(transform.rotation.x, cam.rotation.eulerAngles.y, transform.rotation.z);

						newVel = new Vector3(
							newVel.x,
							rb.velocity.y,
							newVel.z
						);
						if (isGrounded() && !isSliding)
						{
							

							if (Input.GetKeyDown(KeyCode.Space))
							{
								newVel = new Vector3(
									newVel.x,
									jumpForce,
									newVel.z
								);
								//rb.velocity = newVel;

							}
							
							rb.velocity = newVel;
						}

						else
						{
							if (isGrounded())
							{
								if (Input.GetKeyDown(KeyCode.Space))
								{
									newVel = new Vector3(
										newVel.x,
										jumpForce,
										newVel.z
									);
									//rb.velocity = newVel;

								}
							}
							
							if (dir != Vector3.zero)
							{
								rb.velocity = (airControl * newVel) + ((1 - airControl) * rb.velocity);
								
							}
							
						}
						if (Input.GetKey(KeyCode.LeftControl))
						{
							RaycastHit hit;
							if(Physics.Raycast(feet.position, Vector3.down, out hit,5f, whatIsGround))
							{
								if (hit.normal != Vector3.up)
								{
									isSliding = true;
								}
							}
							
							bodyClient.transform.localScale = new Vector3(1, .5f, 1);
							bodyClient.GetComponent<Collider>().material.dynamicFriction = 0;
						}
						else
						{
							isSliding = false;
							bodyClient.transform.localScale = Vector3.one;
							bodyClient.GetComponent<Collider>().material.dynamicFriction = 0.6f;

						}
						break;
						
					}
				case MoveTypes.flight:
					{
						rb.useGravity = false;
						transform.rotation = Quaternion.Euler(0, cam.rotation.eulerAngles.y, 0);
						// movement
						dir = new Vector3(
							Input.GetAxis("Horizontal"),
							Input.GetAxis("Vertical"),
							Input.GetAxis("UppyDowny")
							).normalized;

						newVel = cam.transform.forward * (dir.y * moveSpeed.y) + cam.transform.right * (dir.x * moveSpeed.x) + cam.transform.up * (dir.z * moveSpeed.z);

						transform.rotation = Quaternion.Euler(transform.rotation.x, cam.rotation.eulerAngles.y, transform.rotation.z);

						rb.velocity = newVel;
						break;
					}
			}
			
		}

	}
	bool isGrounded()
	{
		return Physics.OverlapSphere(feet.position, GCRadius, whatIsGround).Length > 0;
	}
	private void FixedUpdate()
	{
		if (isLocalPlayer && allowMovement)
		{

			//rb.velocity = newVel;
		}
	}
	public void OnDrawGizmos()
	{
		//if (isLocalPlayer)
		//{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(feet.position, GCRadius);
		//}
		
	}
}
