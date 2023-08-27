using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
	public virtual void StateOn()
	{

	}

	public virtual bool IsStateOn()
	{
		return true;
	}
}
