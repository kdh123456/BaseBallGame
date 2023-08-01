using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    [SerializeField]
    private Transform rotation;

	public Transform left;
	public Transform right;

    private Rigidbody _rb;

    void Start()
    {
		_rb  = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		//this.transform.rotation = rotation.rotation;
	}
}
