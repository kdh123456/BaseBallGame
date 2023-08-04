using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Referee : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();

        GameManager.Instance.onChangeCount += StrikeOrBall;
	}

    private void StrikeOrBall(CountEnum count)
    {
        if(count == CountEnum.Strike)
        {
            _animator.SetTrigger("Strike");
		}
    }
}
