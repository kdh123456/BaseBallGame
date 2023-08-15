using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	private Vector3 _pitchingCameraPos;

	[SerializeField]
	private Vector3 _battingCameraPos;

	private void Start()
	{
		GameManager.Instance.onChangeGameMode += ChangeCameraPos;
	}

	private void ChangeCameraPos(Mode mode)
	{
		this.transform.position = mode == Mode.PitchMode ? _pitchingCameraPos : _battingCameraPos;
		this.transform.localRotation = mode == Mode.PitchMode ? Quaternion.Euler(6.65f, 0, 0) : Quaternion.Euler(6.65f, 180, 0);
	}
}
