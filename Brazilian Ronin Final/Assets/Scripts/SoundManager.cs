﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource mainSource { get; private set; }
    public AudioClip battleMusic;
    public static SoundManager singleton;
    public AudioSource soundHitRight { get; private set; }
    public AudioSource soundHitLeft { get; private set; }
    public AudioSource soundDamage { get; private set; }
    public AudioSource soundAxe { get; private set; }
    public static AudioClip currentClip => singleton.mainSource.clip;
    private float targetVolume;
    private bool smoothOff = false;
    private bool smoothOn = false;


    private void Awake()
    {
        if (!singleton)
        {
            singleton = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        CampScript.OnClearCamp -= OffBattleMusic;
        CampScript.OnClearCamp += OffBattleMusic;
        
        singleton.mainSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
        singleton.soundHitRight = GameObject.Find("RightHandAttack").GetComponent<AudioSource>();
        singleton.soundHitLeft = GameObject.Find("LeftHandAttack").GetComponent<AudioSource>();
        singleton.soundDamage = GameObject.Find("mixamorig:Neck").GetComponent<AudioSource>();
        //singleton.soundAxe = GameObject.Find("attackZone").GetComponent<AudioSource>();
    }

   public static void PlayMusic(AudioClip clip, float volume, bool loop)
    {
        if (clip == currentClip || clip == null) return;
        singleton.mainSource.loop = loop;
        singleton.mainSource.clip = clip;
        singleton.targetVolume = volume;
        singleton.smoothOn = true;
        singleton.mainSource.Play();
    }

    private void OffBattleMusic()
    {

        if(currentClip == battleMusic)
        {
            smoothOff = true;
        }
    }

    public void Update()
    {
        if (smoothOff)
        {
            singleton.mainSource.volume -= 0.04f * Time.deltaTime;
            if (singleton.mainSource.volume == 0)
            {
                singleton.mainSource.Stop();
                smoothOff = false;
            }
        }

        if (smoothOn)
        {
            singleton.mainSource.volume += 0.06f * Time.deltaTime;
            if (singleton.mainSource.volume > singleton.targetVolume)
            {
                smoothOn = false;
            }
        }
    }
}
