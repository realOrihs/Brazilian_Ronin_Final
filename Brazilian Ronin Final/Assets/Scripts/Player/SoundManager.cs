using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource mainSource { get; private set; }
    public AudioClip battleMusic;
    public static SoundManager singleton;
    public AudioSource soundHitRight { get; private set; }
    public AudioSource soundHitLeft { get; private set; }
    public AudioSource soundDamage { get; private set; }
    public AudioSource soundAxe { get; private set; }
    public AudioSource soundRun { get; private set; }
    public AudioSource soundRoll { get; private set; }
    public static AudioClip currentClip => singleton.mainSource.clip;
    private float targetVolume;
    public bool smoothOff = false;
    public bool smoothOn = false;


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
            return;
        }
        
        CampScript.OnClearCamp += OffBattleMusic;
        Player.isDead += OffBattleMusic;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            singleton.mainSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
            singleton.soundHitRight = GameObject.Find("RightHandAttack").GetComponent<AudioSource>();
            singleton.soundHitLeft = GameObject.Find("LeftHandAttack").GetComponent<AudioSource>();
            singleton.soundDamage = GameObject.Find("mixamorig:Neck").GetComponent<AudioSource>();
            singleton.soundRun = GameObject.Find("Armature.001").GetComponent<AudioSource>();
            singleton.soundRoll = GameObject.Find("mixamorig:Spine").GetComponent<AudioSource>();
        }
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
