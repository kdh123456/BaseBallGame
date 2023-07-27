using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batter : MonoBehaviour
{
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private Bat bat;

	[SerializeField]
	private GameObject _batPos;

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
		
		bat.transform.position = _batPos.transform.position + (Vector3.right /2);
	}

	//private void OnAnimatorIK(int layerIndex)
	//{
	//	if (GameManager.Instance.ballObject)
	//	{
	//		//Debug.Log("¿÷");
	//		//_animator.SetLookAtPosition((GameManager.Instance.ballObject.transform.position - this.transform.position).normalized);
	//		//_animator.SetIKPosition(AvatarIKGoal.LeftHand, GameManager.Instance.ballObject.transform.position);
	//		//_animator.SetIKPosition(AvatarIKGoal.RightHand, GameManager.Instance.ballObject.transform.position);
	//		//_animator.SetIKPosition(AvatarIKGoal., GameManager.Instance.ballObject.transform.position);
	//	}
	//	//_animator.SetLookAtWeight(100);
	//}
}
