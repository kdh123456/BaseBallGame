using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGamePanel : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    public Sprite[] sprites;

    private Canvas _canvas;

    private TeamSelect[] _teamSelect;

    void Awake()
    {
        _button.onClick.AddListener(StartGame);
		_canvas = this.transform.parent.GetComponent<Canvas>();
		_teamSelect = this.GetComponentsInChildren<TeamSelect>();
	}

    private void StartGame()
    {
        foreach(TeamSelect teamSelect in _teamSelect)
        {
            teamSelect.SetTeam();
		}
		GameManager.Instance.ChangeMode(Mode.PitchMode);
		_canvas.gameObject.SetActive(false);
	}
}
