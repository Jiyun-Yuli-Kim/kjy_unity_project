using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    public enum EBGM
    {
        BGM_Title=0,
        BGM_Main
    }

    public enum ESFX
    {
        SFX_Walk,
        SFX_Run,
        SFX_Shake,
    }

    [SerializeField] private AudioClip[] bgms;
    [SerializeField] private AudioClip[] sfxs;
    
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource sfx;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayBGM(EBGM clip)
    {
        bgm.clip = bgms[(int)clip];
        bgm.Play();
    }

    public void PauseBGM()
    {
        if (bgm.isPlaying)
        {
            bgm.Pause();
        }

        else
        {
            return;
        }
    }

    public void StopBGM()
    {
        if (bgm.isPlaying)
        {
            bgm.Stop();
        }

        else
        {
            return;
        }
    }

    public void PlaySFX(ESFX clip)
    {		
        //동시에 플레이가 가능해야하는데 이방식으로 하면 끊겨버림
        // sfx.clip = clip;
        // sfx.Play();
		
        // 여러 음원 겹칠 수 있음
        sfx.PlayOneShot(sfxs[(int)clip]);
    }
}
