using UnityEngine;
using Mirror;
using System.Collections.Generic;

/*
	Documentation: https://mirror-networking.com/docs/Guides/NetworkBehaviour.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class Movment : NetworkBehaviour
{
    #region Start & Stop Callbacks

    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer() { }

    /// <summary>
    /// Invoked on the server when the object is unspawned
    /// <para>Useful for saving object data in persistent storage</para>
    /// </summary>
    public override void OnStopServer() { }

    /// <summary>
    /// Called on every NetworkBehaviour when it is activated on a client.
    /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
    /// </summary>
    public override void OnStartClient() { }

    /// <summary>
    /// This is invoked on clients when the server has caused this object to be destroyed.
    /// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
    /// </summary>
    public override void OnStopClient() { }

    /// <summary>
    /// Called when the local player object has been set up.
    /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStartLocalPlayer() { }

    /// <summary>
    /// This is invoked on behaviours that have authority, based on context and <see cref="NetworkIdentity.hasAuthority">NetworkIdentity.hasAuthority</see>.
    /// <para>This is called after <see cref="OnStartServer">OnStartServer</see> and before <see cref="OnStartClient">OnStartClient.</see></para>
    /// <para>When <see cref="NetworkIdentity.AssignClientAuthority">AssignClientAuthority</see> is called on the server, this will be called on the client that owns the object. When an object is spawned with <see cref="NetworkServer.Spawn">NetworkServer.Spawn</see> with a NetworkConnection parameter included, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStartAuthority() { }

    /// <summary>
    /// This is invoked on behaviours when authority is removed.
    /// <para>When NetworkIdentity.RemoveClientAuthority is called on the server, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStopAuthority() { }

	#endregion
	public float jumpForce = 1, sensitivity = 1;
	Vector2 speed;
	public Vector3 dir;
	Rigidbody rb;
	public Transform cam;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		if (isLocalPlayer)
		{
			rb.isKinematic = false;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else
		{
			rb.isKinematic = true;
		}
	}
	private void FixedUpdate()
	{
		if (isLocalPlayer)
		{
			
		}
	}

	private void Update()
	{
		if (isLocalPlayer)
		{
			
			//print("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
			dir = new Vector3(
				(Input.GetAxis("Horizontal") + transform.forward.x) * Time.deltaTime,
				0,
				(Input.GetAxis("Vertical") + transform.forward.y) * Time.deltaTime
				).normalized * speed;

			Vector3 newVel = transform.forward * (dir.y * speed.y) + transform.right * (dir.x * speed.x);
			rb.velocity = new Vector3(
				newVel.x,
				rb.velocity.y,
				newVel.z
				);


			if (Input.GetKeyDown(KeyCode.Space))
			{
				rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
			}



			//transform.position = Vector3.MoveTowards(transform.position, body.position + offset, followSpeed);
			float angleY = -Input.GetAxisRaw("Mouse Y") * sensitivity;
			//print(angleY);
			if (cam.transform.rotation.eulerAngles.x + angleY > 90f && cam.transform.rotation.eulerAngles.x + angleY < 180)
			{
				angleY = 90f - cam.transform.rotation.eulerAngles.x;
			}
			//print(transform.rotation.eulerAngles.x);

			if (cam.transform.rotation.eulerAngles.x + angleY < 270 && cam.transform.rotation.eulerAngles.x + angleY > 180)
			{
				angleY = 270 - cam.transform.rotation.eulerAngles.x;
			}



			float angleX = Input.GetAxisRaw("Mouse X") * sensitivity;
			cam.transform.rotation = Quaternion.Euler(angleY + cam.transform.rotation.eulerAngles.x, angleX + cam.transform.rotation.eulerAngles.y, 0);
			transform.rotation = Quaternion.Euler(transform.rotation.x, cam.rotation.y, transform.rotation.z);
		}
		
	}
}
