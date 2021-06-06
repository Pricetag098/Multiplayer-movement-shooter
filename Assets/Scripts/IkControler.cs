using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IkControler : MonoBehaviour
{
	public PlayerManager player;

	public Transform leftHand, rightHand, head;

	Animator animator;
    // Start is called before the first frame update
    void Start()
    {
		animator = GetComponent<Animator>();
	}

	private void OnAnimatorIK(int layerIndex)
	{
		if (player.isServer)
		{
			animator.SetLookAtWeight(1);
			animator.SetLookAtPosition(head.position + head.forward);


			
			if (rightHand != null)
			{
				animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
				animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
				animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
				animator.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
			}

			if (leftHand != null)
			{
				animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
				animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
				animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
				animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
			}
		}
			
	}
}
