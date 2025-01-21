using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlayBGM(SoundManager.EBGM.BGM_Title);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            GameManager.Instance.SceneChange.Load(1);
            SoundManager.Instance.PlayBGM(SoundManager.EBGM.BGM_Main);
        }
    }
}
