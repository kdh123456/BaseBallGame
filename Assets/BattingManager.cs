using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattingManager : MonoSingleton<BattingManager>
{
	public Batter _batter;

	public GameObject _batCameraPos;

	private void Start()
	{
		GameManager.Instance.onStateChange += BattingCamera;
		GameManager.Instance.onStateChange += NextBatter;
	}

	public void BattingReset()
	{
		_batter.gameObject.SetActive(true);
		CameraController.Instance.CameraReset();
	}

	private void BattingCamera(BattingState state)
	{
		if(state == BattingState.Batting)
		{
			CameraController.Instance.SetCameraPosition(_batCameraPos.transform.position);
			CameraController.Instance.ChaseCamera(GameManager.Instance.BallObject);
			CameraController.Instance.ZoomInOutCamera(10);
		}
	}

	private void NextBatter(BattingState state)
	{
		if (state == BattingState.Batting)
		{
			GameManager.Instance.ChangeTeamBatter();
		}
	}

}
