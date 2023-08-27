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
		GameManager.Instance.onStateChange += BattingReset;
	}

	public void BattingReset(BattingState state)
	{
		if(state == BattingState.Idle)
		_batter.gameObject.SetActive(true);
	}

	private void BattingCamera(BattingState state)
	{
		if(state == BattingState.Batting)
		{
			CameraController.Instance.SetCameraPosition(_batCameraPos.transform.position);
			CameraController.Instance.ChaseCamera(GameManager.Instance.BallObject);
			CameraController.Instance.ZoomInOutCamera(30);
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
