using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    private Rigidbody _rb;

    void Start()
    {
		_rb  = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
