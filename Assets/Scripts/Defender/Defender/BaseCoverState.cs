using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseCoverState : State
{
	public bool baseCoverOn = false;
	private Base _coverBase = null;

	private NavMeshAgent _agent;

	private void Start()
	{
		_agent = GetComponent<NavMeshAgent>();
	}
	public override void StateOn()
	{
		if (baseCoverOn)
		{
			_coverBase.BaseCovering();
			_agent.SetDestination(_coverBase.transform.position);
		}

		if (BaseControll.Instance.BaseIsEmpty())
		{
			List<Base> bases = BaseControll.Instance.EmptyBases();

			float minDistacne = 999f;
			foreach (Base Bases in bases)
			{
				float distacne = Vector3.Distance(this.transform.position, Bases.transform.position);
				if (minDistacne > distacne)
				{
					minDistacne = distacne;
				}

				_coverBase = Bases;
			}
			_coverBase.BaseCoverSetting(minDistacne, this.transform.GetComponent<Defend>());
		}
	}

	public override bool IsStateOn()
	{
		if (BaseControll.Instance.BaseIsEmpty())
		{
			List<Base> bases = BaseControll.Instance.EmptyBases();

			foreach (Base Bases in bases)
			{
				float distacne = Vector3.Distance(this.transform.position, Bases.transform.position);
				if (distacne < 10)
					return true;
			}

			return false;
		}

		if (baseCoverOn)
			return true;

		return false;
	}
}
