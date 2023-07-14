using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchingSelector : MonoBehaviour
{
	[SerializeField]
	private GameObject obj;

	private void Update()
	{
		Vector3 vec = obj.transform.localPosition;
		float xHori = Input.GetAxis("Horizontal") * Time.deltaTime;
		float yVerti = Input.GetAxis("Vertical") * Time.deltaTime;

		obj.transform.localPosition = new Vector3(Mathf.Clamp(vec.x + xHori, -0.5f, 0.5f), Mathf.Clamp(vec.y + yVerti, -0.5f, 0.5f), obj.transform.position.z);
	}
}
