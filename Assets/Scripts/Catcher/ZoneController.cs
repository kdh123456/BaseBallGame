using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
	[SerializeField]
	private GameObject[] zones;

	private void Start()
	{
		GameManager.Instance.onChangeGameMode += ZoneChange;
	}

	private void ZoneChange(Mode mode)
	{
		if (mode == Mode.PitchMode || mode == Mode.BatMode)
		{
			int index = mode == Mode.PitchMode ? 1 : 0;
			zones[index].SetActive(false);
			zones[(int)mode].SetActive(true);
		}
	}
}
