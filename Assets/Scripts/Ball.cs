using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	private Rigidbody _rb;

	[SerializeField]
	private Vector3 ShootVec;

	[SerializeField]
	private Vector3 vec = Vector3.forward;

	[SerializeField]
	private float _speed;

	[SerializeField]
	private float changeSpeed;

	private bool _isShoot;

	void Awake()
	{
		_rb = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		GameManager.Instance.ballObject = this.gameObject;
	}

	public void Shoot(Vector3 shootVec, Vector3 rotationVec, float speed, float rpmSpeed)
	{
		_rb.AddForce(shootVec.normalized * speed);
		_rb.angularVelocity = rotationVec * speed;

		GameManager.Instance.ChangeState(BattingState.Pitching);
		_isShoot = true;
	}

	private void FixedUpdate()
	{
		if (!_isShoot)
			return;

		Vector3 anglevec = _rb.angularVelocity;
		Vector3 velocity = _rb.velocity;

		anglevec.Normalize();
		velocity.Normalize();

		Vector3 cross = Vector3.Cross(anglevec, velocity);
		_rb.AddForce(cross * changeSpeed);
		_rb.AddForce(ShootVec.normalized * _speed);
	}

	public void Hit()
	{
		_isShoot = false;
		GameManager.Instance.ChangeState(BattingState.Batting);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.CompareTag("Ground") && GameManager.Instance.State != BattingState.Batting)
		{
			GameManager.Instance.AddBall();
		}
	}
}
