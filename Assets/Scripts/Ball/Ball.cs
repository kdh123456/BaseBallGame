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

	private Vector3 cvec;
	private Vector3 rbvec;

	public Vector3 battingVec => rbvec;
	public bool IsShoot => _isShoot;

	void Awake()
	{
		_rb = GetComponent<Rigidbody>();
	}

	public void Shoot(Vector3 shootVec, Vector3 rotationVec, float speed, float rpmSpeed, bool isPitcher = false)
	{
		_rb.useGravity = true;
		shootVec.y = 0;
		_rb.AddForce(shootVec.normalized * speed);
		_rb.angularVelocity = rotationVec * speed;
		ShootVec = shootVec;
		_speed = speed;
		changeSpeed = rpmSpeed;

		if (isPitcher)
			GameManager.Instance.ChangeState(BattingState.Pitching);

		GameManager.Instance.SetBall(this.transform.gameObject);
		_isShoot = true;
	}

	private void Update()
	{
		rbvec = _rb.velocity;
		Vector3 vec = rbvec;
		//vec = vec * 100;
		vec.y = 1;
		Debug.DrawLine(this.transform.position, this.transform.position + vec);
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

	public void Muzzle(Transform trans)
	{
		_isShoot = false;
		_rb.useGravity = false;
		_rb.velocity = Vector3.zero;
		this.transform.SetParent(trans);
		this.transform.localPosition = Vector3.zero;
	}

	public void Hit()
	{
		_isShoot = false;

		cvec = this.transform.position;
		rbvec = _rb.velocity;
		rbvec.Normalize();
		Debug.Break();
	}

	private void OnCollisionEnter(Collision collision)
	{
		_isShoot = false;

		if(collision.gameObject.tag == "Ground" && GameManager.Instance.State == BattingState.Bat)
		{
			if (GameManager.Instance.CurrentStat.strikeCount < 2)
			{
				GameManager.Instance.AddStrike();
			}
			else
				return;
		}
	}
}
