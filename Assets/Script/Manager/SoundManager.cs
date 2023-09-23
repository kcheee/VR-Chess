using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region
    static public SoundManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // soundManager

    // title 0 : In A World Of My Own
    // main 1 : All in the Golden Afternoon
    //
    public AudioClip[] BGMList;
    public AudioSource audioSource;

    void Start()
    {
        DontDestroyOnLoad(this);

        audioSource = GetComponent<AudioSource>();

        audioSource.Play();
        audioSource.DOFade(1, 3);

        // Title 
        // In A World Of My Own ½ÇÇà;
    }

    // Update is called once per frame
    
}
