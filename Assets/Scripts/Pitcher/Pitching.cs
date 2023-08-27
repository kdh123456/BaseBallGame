using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitching : MonoBehaviour
{
    [SerializeField]
    private GameObject ballInstance;

	[SerializeField]
	private GameObject _shootVec;

	[SerializeField]
	private Animator ballAnimator;

	[SerializeField]
	private GameObject _pitchingVec;

	[SerializeField]
	private PitchSelector _pitchSelector;

	private PitchType _type;
	private Vector3 vec;
	private Vector3 vecNormal;

	private Vector3 maxXPos = Vector3.right / 3;
	private Vector3 minXPos = Vector3.left / 3;

	private Vector3 maxYPos = Vector3.up / 2;
	private Vector3 minYPos = Vector3.down / 2;

	[SerializeField]
	private Transform pos;

	private void Start()
	{
		vec = transform.position;
		maxYPos.y += pos.transform.position.y;
		minYPos.y += pos.transform.position.y;
	}

	void Update()
    {
		if(Input.GetKeyDown(KeyCode.Space)) 
		{
			Shoot();
		}
    }

	public void Shoot()
	{
		_type = _pitchSelector.Type;
		this.transform.position = vec;
		GameManager.Instance.ChangeState(BattingState.Pitch);
		ballAnimator.SetBool("Pitching", true);
	}

	public void ShootBall()
	{
		if(GameManager.Instance.gameMode != Mode.PitchMode)
		{
			float rand = UnityEngine.Random.Range(0f, 100f);

			float x = UnityEngine.Random.Range(minXPos.x, maxXPos.x);
			float y = UnityEngine.Random.Range(minYPos.y, maxYPos.y);
			_pitchingVec.transform.position = new Vector3(x, y, _pitchingVec.transform.position.z);

			switch (rand)
			{
				case float i when i <= 70 && i>0: // data가 int 타입이고 10보다 큰 경우 
					StraightBall();
					break;
				case float i when i <= 80 && i > 70: // data가 int 타입이고 10 이하인 경우 
					SliderBall();
					break;
				case float i when i <= 100 && i > 80:
					CurveBall();
					break;
			}

			return;
		}

		if (_type == PitchType.FourSeamFastBall)
			StraightBall();
		else if (_type == PitchType.SliderBall)
			SliderBall();
		else
			CurveBall();
	}

	private void StraightBall()
    {
		GameObject obj = Instantiate(ballInstance).gameObject;
		_shootVec.transform.position = new Vector3(_shootVec.transform.position.x, _pitchingVec.transform.position.y + 0.9f, _shootVec.transform.position.z);
		vecNormal = (_pitchingVec.transform.position - _shootVec.transform.position);
		obj.transform.position = _shootVec.transform.position;

		Ball ball = obj.GetComponent<Ball>();

		if (ball)
		{
			ball.Shoot(vecNormal, Vector3.left, 100f, 5f, true);
		}

	}

    private void CurveBall()
    {
		GameObject obj = Instantiate(ballInstance).gameObject;
		_shootVec.transform.position = new Vector3(_shootVec.transform.position.x, _pitchingVec.transform.position.y + 2f, _shootVec.transform.position.z);
		vecNormal = (_pitchingVec.transform.position - _shootVec.transform.position);
		obj.transform.position = _shootVec.transform.position;
		Ball ball = obj.GetComponent<Ball>();
		if (ball)
		{
			ball.Shoot(vecNormal, Vector3.right, 100f, 2f, true);
		}
	}

    private void SliderBall()
    {
		GameObject obj = Instantiate(ballInstance).gameObject;
		_shootVec.transform.position = new Vector3(_shootVec.transform.position.x, _pitchingVec.transform.position.y + 1.75f, _shootVec.transform.position.z);
		vecNormal = (_pitchingVec.transform.position - _shootVec.transform.position);
		obj.transform.position = _shootVec.transform.position + new Vector3(0.75f,0,0);
		Ball ball = obj.GetComponent<Ball>();
		if (ball)
		{
			ball.Shoot(vecNormal, Vector3.down, 100f, 5f , true);
		}
	}
}
