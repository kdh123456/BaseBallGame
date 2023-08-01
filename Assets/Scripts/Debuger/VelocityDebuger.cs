using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityDebuger : MonoBehaviour
{
    [SerializeField]
    private float maxVelocity = 20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Renderer>().material.color = ColorForVelocity();
	}

    private Color ColorForVelocity()
    {
        float velocity = GetComponent<Rigidbody>().velocity.magnitude;

        return Color.Lerp(Color.green, Color.red, velocity / maxVelocity);
    }
}
