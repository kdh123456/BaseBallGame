using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

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

	private bool _isShoot = false;
	private bool _isUpdate = false;

	private Vector3 cvec;
	private Vector3 rbvec;

	public Vector3 battingVec => rbvec;
	public bool IsShoot => _isShoot;  

	private Vector3 _gravity;
	private Vector3 _firstRb;
	private Vector3 _futurePath;
	public Vector3 FuturePath => _futurePath;

	private bool _flying = false;
	public bool Flying => _flying;

	private bool _isMuzzle = false;
	public bool IsMuzzle => _isMuzzle;

	private bool _isThrowing = false;
	public bool IsThrowing => _isThrowing;

	private float _timer = 0f;
	public float Timer => _timer;
	void Awake()
	{
		_rb = GetComponent<Rigidbody>();
		_gravity = Physics.gravity;
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
		_isUpdate = true;
	}

	public void Throw(Vector3 shootVec, Vector3 player, Vector3 target, float initialAngle)
	{
		shootVec.y = 0;
		ShootVec = shootVec;
		GameManager.Instance.ChangeState(BattingState.Pitching);
		GameManager.Instance.SetBall(this.transform.gameObject);
		_rb.velocity = GetVelocity(player, target, initialAngle);
		_isShoot = true;
	}

	private void FixedUpdate()
	{
		if (!_isUpdate)
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
		_isThrowing = false;
		_isShoot = false;
		_isUpdate = false;
		_rb.useGravity = false;
		_rb.velocity = Vector3.zero;
		_isMuzzle = true;
		this.transform.SetParent(trans);  
		this.transform.localPosition = Vector3.zero;
	}

	public void Hit()
	{
		_isShoot = false;
		_isUpdate = false;

		cvec = this.transform.position;

		_firstRb = _rb.velocity;

		_flying = true;

		DefendController.Instance.onBating?.Invoke();

		FuturePathSet();
	}

	private void FuturePathSet()
	{
		float minDistance = 999f;
		Defend coverDefend = null;

		float time = Time.deltaTime;
		Vector3 velocity = _firstRb + _gravity * time;
		Vector3 position = this.transform.position;
		position = position + velocity * time;
		float timer = 0;
		while (position.y > 0)
		{
			velocity = velocity + _gravity * time;
			position = position + velocity * time;
			timer = time;
		}

		_timer = timer;
		_futurePath = position;

		foreach (Defend defender in DefendController.Instance._defends)
		{
			float distance = Vector3.Distance(_futurePath, defender.transform.position);
			if (minDistance > distance)
			{
				minDistance = distance;
				coverDefend = defender;
			}
		}
		coverDefend.GetComponent<FuturePathState>().FutureSet(_futurePath);

		NavMeshAgent navAgent = coverDefend.GetComponent<NavMeshAgent>();
		Vector3 vel = navAgent.desiredVelocity;

		Vector3 pos = Vector3.zero;

		float tim = Time.deltaTime;
		float timr = 0f;
		while (pos == _futurePath)
		{
			pos = vel * tim;
			float x = Mathf.Clamp(pos.x, 0, _futurePath.x);
			float y = Mathf.Clamp(pos.y, 0, _futurePath.y);
			float z = Mathf.Clamp(pos.z, 0, _futurePath.z);
			pos = new Vector3(x, y, z);
			timr += tim;
		}

		if (_timer > timr)
		{
			RunnerManager.Instance.fly = true;
		}

		DefendController.Instance.FuturePathCover();
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Bat")
		{
			_isShoot = false;
			_isUpdate = false;
		}

		if(collision.gameObject.tag == "Ground")
		{
			_flying = false;
		}
	}

	public void DefendThrow(Vector3 vec)
	{
		_isThrowing = true;
		_isMuzzle = false;
		_rb.useGravity = true;
		_rb.velocity = GetVelocity(this.transform.position, vec, 30f);
	}

	public Vector3 GetVelocity(Vector3 player, Vector3 target, float initialAngle)
	{
		float gravity = Physics.gravity.magnitude;
		float angle = initialAngle * Mathf.Deg2Rad;

		Vector3 planarTarget = new Vector3(target.x, 0, target.z);
		Vector3 planarPosition = new Vector3(player.x, 0, player.z);

		float distance = Vector3.Distance(planarTarget, planarPosition);
		float yOffset = player.y - target.y;

		float initialVelocity
			= (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

		Vector3 velocity
			= new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

		float angleBetweenObjects
			= Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (target.x > player.x ? 1 : -1);
		Vector3 finalVelocity
			= Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

		return finalVelocity;
	}

	public Vector3 AngleGetVelocity(Vector3 player, Vector3 target, float initialAngle, Vector3 rotateVec)
	{
		float gravity = Physics.gravity.magnitude;
		float angle = initialAngle * Mathf.Deg2Rad;

		Vector3 planarTarget = new Vector3(target.x, 0, target.z);
		Vector3 planarPosition = new Vector3(player.x, 0, player.z);

		float distance = Vector3.Distance(planarTarget, planarPosition);
		float yOffset = player.y - target.y;

		float initialVelocity
			= (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

		Vector3 velocity
			= new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

		// 왼쪽으로 회전하도록 수정
		float angleBetweenObjects
			= Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (target.x < player.x ? -1 : 1);
		Vector3 finalVelocity
			= Quaternion.AngleAxis(angleBetweenObjects, rotateVec) * velocity;

		return finalVelocity;
	}
}
