using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batter : MonoBehaviour
{
	[SerializeField]
	private Animator _animator;

	Vector3 vec;
	Quaternion rotation;

	private void Start()
	{
		vec = transform.position;
		rotation = transform.rotation;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			this.transform.position = vec;
			this.transform.rotation = rotation;
			_animator.SetBool("Batting", true);

		}
	}

	private void OnAnimatorIK(int layerIndex)
	{
		if (GameManager.Instance.ballObject)
		{
			//Debug.Log("¿÷");
			//_animator.SetLookAtPosition((GameManager.Instance.ballObject.transform.position - this.transform.position).normalized);
			_animator.SetIKPosition(AvatarIKGoal.LeftHand, GameManager.Instance.ballObject.transform.position);
			_animator.SetIKPosition(AvatarIKGoal.RightHand, GameManager.Instance.ballObject.transform.position);
			//_animator.SetIKPosition(AvatarIKGoal., GameManager.Instance.ballObject.transform.position);
		}
		//_animator.SetLookAtWeight(100);
	}
}
