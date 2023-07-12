using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchingSelector : MonoBehaviour
{
	[SerializeField]
	private GameObject obj;

	private void Update()
	{
		float xHori = Input.GetAxis("Horizontal");
		float yVerti = Input.GetAxis("Vertical");

		float x = obj.transform.position.x + xHori;
		float y = obj.transform.position.y + yVerti;

		obj.transform.localPosition = new Vector3(Mathf.Clamp(x, -0.5f, 0.5f), Mathf.Clamp(y, -0.5f, 0.5f), obj.transform.position.z);
	}


}
