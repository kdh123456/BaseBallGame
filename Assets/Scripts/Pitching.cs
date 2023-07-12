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

	private Vector3 vec;

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
    }

	public void Shoot()
	{
		StraightBall();
	}

    public void StraightBall()
    {
		GameObject obj = Instantiate(ballInstance).gameObject;
		obj.transform.position = ShootVec.transform.position;
		Ball ball = obj.GetComponent<Ball>();
		if (ball)
		{
			ball.Shoot(Vector3.forward, Vector3.left, 100f, 10f);
		}

		GameManager.Instance.ballObject = obj;
	}

    private void CurveBall()
    {
		GameObject obj = Instantiate(ballInstance).gameObject;
		obj.transform.position = ShootVec.transform.position;
		Ball ball = obj.GetComponent<Ball>();
		if (ball)
		{
			ball.Shoot(Vector3.forward, Vector3.right, 100f, 10f);
		}
	}

    private void SliderBall()
    {
		GameObject obj = Instantiate(ballInstance).gameObject;
		obj.transform.position = ShootVec.transform.position;
		Ball ball = obj.GetComponent<Ball>();
		if (ball)
		{
			ball.Shoot(Vector3.forward, Vector3.down + Vector3.right, 100f, 10f);
		}

		GameManager.Instance.ballObject = obj;
	}
}
