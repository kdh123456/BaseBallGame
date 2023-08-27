using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
	public bool isVirtual;

	public List<Rigidbody> bodys = new List<Rigidbody>();

	[NonSerialized]
	public List<Rigidbody> vBodys = new List<Rigidbody>();

	private bool isBattingOn = false;

	[SerializeField]
	private float maxTimer;
	private float timer;

	private MeshRenderer renderer;

	private void Awake()
	{
		renderer = GetComponent<MeshRenderer>();
	}

	public void OnEnable()
	{
		if(!isVirtual)
		{
			isBattingOn= true;
			timer = 0;
			renderer.enabled = false;
			foreach (Rigidbody rb in bodys)
			{
				rb.gameObject.SetActive(true);
			}
		}

		if (isVirtual)
		{
			foreach (Rigidbody rb in bodys)
			{
				rb.gameObject.SetActive(true);
			}
		}
	}
	public void OnDisable()
	{
		if (isVirtual)
		{
			foreach (Rigidbody rb in bodys)
			{
				if(rb!= null)
				rb.gameObject.SetActive(false);
			}
		}
	}

	private void Update()
	{
		if (isBattingOn)
		{
			if (maxTimer > timer)
			{
				timer += Time.deltaTime;
				for (int i = 0; i < vBodys.Count; i++)
				{
					bodys[i].velocity = vBodys[i].velocity;
				}
			}
			else
			{
				foreach (Rigidbody rb in bodys)
				{
					rb.gameObject.SetActive(false);
				}
				this.gameObject.SetActive(false);
			}
		}
	}
}
