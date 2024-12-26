using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    private Collider _collider;
    private PlayerController _player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Kind" && _player.isInteracting)
        {
            // Kind 유형 주민의 대화를 로드한다
        }
    }
}
