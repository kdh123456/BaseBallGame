using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
	[SerializeField]
	private Button _button;

	private Canvas _canvas;

	private void Awake()
	{
		_button.onClick.AddListener(ReStartGame);
		_canvas = this.transform.parent.GetComponent<Canvas>();
		GameManager.Instance.onChangeGameMode += StartGame;
	}

	private void ReStartGame()
	{
		GameManager.Instance.ChangeMode(Mode.StartMode);
	}

	private void StartGame(Mode mode)
	{
		if(mode == Mode.StartMode)
		_canvas.gameObject.SetActive(false);
	}
}
