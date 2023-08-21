using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoSingleton<CameraController>
{
	[SerializeField]
	private Vector3 _pitchingCameraPos;

	[SerializeField]
	private Vector3 _battingCameraPos;

	private GameObject _chaseObject = null;

	[SerializeField]
	private CinemachineVirtualCamera _virtualCamera;

	private void Start()
	{
		GameManager.Instance.onChangeGameMode += ChangeCameraPos;
		_virtualCamera = GetComponent<CinemachineVirtualCamera>();
	}

	public void ChangeCameraPos(Mode mode)
	{
		this.transform.position = mode == Mode.PitchMode ? _pitchingCameraPos : _battingCameraPos;
		this.transform.localRotation = mode == Mode.PitchMode ? Quaternion.Euler(6.65f, 0, 0) : Quaternion.Euler(6.65f, 180, 0);
	}

	public void ChaseCamera(GameObject obj)
	{
		_chaseObject = obj;
		if (_chaseObject == null)
		{
			_virtualCamera.LookAt = null;
			return;
		}
		//_virtualCamera.Follow = _chaseObject.transform;
		_virtualCamera.LookAt = _chaseObject.transform;
	}

	public void ZoomInOutCamera(float fov)
	{
		_virtualCamera.m_Lens.FieldOfView = fov;
	}

	public void SetCameraPosition(Vector3 vec)
	{
		this.gameObject.transform.position = vec;
	}

	public void CameraReset()
	{
		ChaseCamera(null);
		ZoomInOutCamera(60);
	}
}
