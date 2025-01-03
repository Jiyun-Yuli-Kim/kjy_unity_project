using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            GameManager.Instance.SceneChange.Load(1);
        }
    }
}
