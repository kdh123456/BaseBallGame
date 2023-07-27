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
	}

	private void ActiveCanavas(Mode mode)
	{
		_objects[(int)mode].gameObject.SetActive(true);
		_objects[(int)mode == 0 ? 1 : 0].gameObject.SetActive(false);
	}
}
