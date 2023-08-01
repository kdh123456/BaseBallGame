using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBallImage : MonoBehaviour
{
	[SerializeField]
	private GameObject _obj;

	public void AddBall(Transform trans)
	{
		GameObject obj = Instantiate(_obj);
		obj.transform.parent = this.transform;
		obj.transform.position = Camera.main.WorldToScreenPoint(trans.position); 
	}
}
