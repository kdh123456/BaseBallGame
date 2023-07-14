using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitching : MonoBehaviour
{
    [SerializeField]
    private GameObject ballInstance;

	[SerializeField]
	private GameObject ShootVec;

	[SerializeField]
	private Animator ballAnimator;

	[SerializeField]
	private GameObject _pitchingVec;

	private Vector3 vec;
	private Vector3 vecNormal;
	private Vector3 shootVec;
	private void Start()
	{
		vec = transform.position;
	}

	void Update()
    {
		if(Input.GetKeyDown(KeyCode.Space)) 
		{
			this.transform.position = vec;
			GameManager.Instance.ChangeState(BattingState.Pitch);
		}

		if(GameManager.Instance.State==BattingState.Pitch)
		{
			ballAnimator.SetBool("Pitching", true);
		}

		Debug.DrawLine(shootVec, vecNormal);
    }

	public void Shoot()
	{
		StraightBall();
	}

    public void StraightBall()
    {
		GameObject obj = Instantiate(ballInstance).gameObject;
		ShootVec.transform.position = new Vector3(ShootVec.transform.position.x, _pitchingVec.transform.position.y + 0.9f, ShootVec.transform.position.z);
		vecNormal = (_pitchingVec.transform.position - ShootVec.transform.position);
		obj.transform.position = ShootVec.transform.position;

		Ball ball = obj.GetComponent<Ball>();

		if (ball)
		{
			ball.Shoot(vecNormal, Vector3.left, 100f, 5f);
		}
		shootVec = obj.transform.position;

		GameManager.Instance.ballObject = obj;
	}

    private void CurveBall()
    {
		GameObject obj = Instantiate(ballInstance).gameObject;
		ShootVec.transform.position = new Vector3(ShootVec.transform.position.x, _pitchingVec.transform.position.y + 2f, ShootVec.transform.position.z);
		vecNormal = (_pitchingVec.transform.position - ShootVec.transform.position);
		obj.transform.position = ShootVec.transform.position;
		Ball ball = obj.GetComponent<Ball>();
		if (ball)
		{
			ball.Shoot(vecNormal, Vector3.right, 100f, 2f);
		}
	}

    private void SliderBall()
    {
		GameObject obj = Instantiate(ballInstance).gameObject;
		ShootVec.transform.position = new Vector3(ShootVec.transform.position.x, _pitchingVec.transform.position.y + 1.75f, ShootVec.transform.position.z);
		vecNormal = (_pitchingVec.transform.position - ShootVec.transform.position);
		obj.transform.position = ShootVec.transform.position + new Vector3(0.75f,0,0);
		Ball ball = obj.GetComponent<Ball>();
		if (ball)
		{
			ball.Shoot(vecNormal, Vector3.down, 100f, 5f);
		}

		GameManager.Instance.ballObject = obj;
	}
}
