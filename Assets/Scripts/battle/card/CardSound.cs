using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSound
{
    string name;
    AudioClip aud;



    float life;
    float timer;
    float delay = 0;
    float volume = 1.0f;
    bool activ = true;
    Character target;

    Transform soundPlace;

    public CardSound(string n)
    {
        name = n;
        delay = 0;
        volume = 1.0f;
    }

    void createDoll(Character c)
    {
        GameObject build = Resources.Load<GameObject>("UI/soundObject") as GameObject;
        GameObject temp = GameObject.Instantiate(build);

        temp.transform.SetParent(c.getAnimationParent());
        AudioSource au = temp.transform.GetComponent<AudioSource>();
        au.clip = aud;
        au.volume = getSoundVolume();
        //au.Play(0);
        soundPlace = temp.transform;
    }

    public float getSoundVolume()
    {
        return 1.0f;
    }

    public void setAudio(AudioClip c)
    {
        aud = c;
    }


    public void setDelay(float f)
    {
        delay = f;
    }

    public void setVolume(float f)
    {
        volume = f;
    }

    public void play(Character c)
    {
        if (aud != null)
        {
            activ = true;
            target = c;
            timer = 0;
            life = delay + aud.length;
            c.addCurrentSound(this);
            createDoll(c);
            

            fight(0);
        }
    }

    public void fight(float time)
    {
        if (activ && timer >= delay)
        {
            activ = false;
            soundPlace.GetComponent<AudioSource>().Play(0);
        }

        timer += time;

        if (timer > life)
        {
            die();
        }
    }

    public string getName()
    {
        return name;
    }

    public CardSound deepCopy()
    {
        CardSound cd = new CardSound(name);

        cd.setAudio(aud);

        return cd;
    }


    public void die()
    {
        target.removeSound(this);


        if (soundPlace != null)
        {
            GameObject.Destroy(soundPlace.gameObject);
        }

    }

    public void loadSound(string n)
    {
        string place = "Battle/Sounds/" + n;


        aud = Resources.Load<AudioClip>(place);

    }

}
