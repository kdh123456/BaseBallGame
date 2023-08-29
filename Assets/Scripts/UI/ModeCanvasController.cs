using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeCanvasController : MonoBehaviour
{
	[SerializeField]
	private Canvas[] _objects;

	private void Start()
	{
		_objects = GetComponentsInChildren<Canvas>();
		GameManager.Instance.onChangeGameMode += ActiveCanavas;
		GameManager.Instance.onChangeGameMode += ActiveStartOrEndCanvas;
		GameManager.Instance.onChangeGameMode += ActiveBasicCanavas;
	}

	private void ActiveCanavas(Mode mode)
	{
		_objects[(int)mode].gameObject.SetActive(true);
		_objects[(int)mode == 0 ? 1 : 0].gameObject.SetActive(false);
	}

	private void ActiveStartOrEndCanvas(Mode mode)
	{
		if (mode == Mode.StartMode || mode == Mode.EndMode)
		{
			_objects[(int)mode].gameObject.SetActive(true);
			_objects[(int)Mode.PitchMode].gameObject.SetActive(false);
			_objects[(int)Mode.BatMode].gameObject.SetActive(false);
			_objects[4].gameObject.SetActive(false);
		}
	}

	private void ActiveBasicCanavas(Mode mode)
	{
		if(mode == Mode.PitchMode || mode == Mode.BatMode)
		_objects[4].gameObject.SetActive(true);
	}
}
