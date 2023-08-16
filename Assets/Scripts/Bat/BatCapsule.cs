using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCapsule : MonoBehaviour
{
	[SerializeField]
	private BatCapsuleFollower _batCapsuleFollowerPrefab;

	private Bat bat;

	private void SpawnBatCapsuleFollower()
	{
		var follower = Instantiate(_batCapsuleFollowerPrefab);
		follower.transform.position = transform.position;

		if (!bat.isVirtual)
			follower.SetFollowTarget(this);
		else
			follower.SetFollowTarget(this, true);

		bat.bodys.Add(follower.GetComponent<Rigidbody>());
	}

	private void Awake()
	{
		bat = gameObject.GetComponentInParent<Bat>();
	}

	private void Start()
	{
		SpawnBatCapsuleFollower();
	}
}
