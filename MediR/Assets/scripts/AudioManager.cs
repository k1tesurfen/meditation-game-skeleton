using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    //TODO: 6 different audioclips to play before each condition.
    //      2 extra audioclips describe the task to do 

    public Sound[] sounds;

    public float waitingTime = 0f;

    public bool isAudioPlaying = false;

    private void Awake()
    {
        //populate audiomanager with audiosources.
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    private void Update()
    {
        //reduce time to wait 
        waitingTime -= Time.deltaTime;
        if(waitingTime < 0f)
        {
            waitingTime = 0f;
            isAudioPlaying = false;
        }
    }

    public float GetDuration(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.clip.length;
    }

    //plays sound with name name
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void AddToQueue(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        StartCoroutine(JanDelay(waitingTime, s));
        waitingTime += s.source.clip.length + 1f;
        isAudioPlaying = true;
    }

    public void Play(string[] name)
    {
        float length = 0f;
        foreach(string s in name)
        {
            Sound clip = Array.Find(sounds, sound => sound.name == s);
            StartCoroutine(JanDelay(length, clip));
            length += clip.source.clip.length + 1f;
        } 
    }

    IEnumerator JanDelay(float time, Sound s)
    {
        yield return new WaitForSeconds(time);
        s.source.Play();
    }
}

