using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCapsuleFollower : MonoBehaviour
{
	private BatCapsule _batFollower;
	private Rigidbody _rigidbody;
	private Vector3 _velocity;

	[SerializeField]
	private float _sensitivity = 100f;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		Vector3 destination = _batFollower.transform.position;
		_rigidbody.transform.rotation = transform.rotation;

		_velocity = (destination - _rigidbody.transform.position) * _sensitivity;
		_rigidbody.velocity = _velocity;
		transform.rotation = _batFollower.transform.rotation;
		transform.position = _batFollower.transform.position;
	}

	public void SetFollowTarget(BatCapsule batFollower)
	{
		_batFollower = batFollower;
	}

	public void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Ball")
		{
			collision.gameObject.GetComponent<Ball>().Hit();

			Vector3 conVel = _rigidbody.velocity;
			Vector3 conAnguarval = _rigidbody.angularVelocity;

			Vector3 ballVel = collision.rigidbody.velocity;
			Vector3 ballAnguarval = collision.rigidbody.angularVelocity;

			collision.rigidbody.velocity = conVel + -ballVel;
			collision.rigidbody.angularVelocity = conAnguarval + -ballAnguarval;
		}
	}
}
