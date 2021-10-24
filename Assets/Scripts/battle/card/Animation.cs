using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class Animation
{
    public float totalDuration = 1.0f;
    public bool repeat = false;
    public float repeatDuration = 0.0f;
    public float delay = 0.0f;
    public float scale = 1.0f;
    public float ending = 0.0f;
    public string name;
    int frameNum = 0;

    float timer = 0;

    Transform imagePlace;
    List<Sprite> imageFrames;
    Character target;

    [System.NonSerialized]
    List<float> tickTimes;

    [System.NonSerialized]
    int tickPlace;

    [System.NonSerialized]
    int animPlace;

    public Animation(string n)
    {
        name = n;
    }

    public void apply(Character target, Character source)
    {
        source.playAnimation(this,target);
    }

    public Animation deepCopy()
    {
        Animation a = new Animation(name);
        a.setNums(totalDuration, repeat, repeatDuration, frameNum, delay,ending, scale);   
        if (imageFrames != null)
        {
            List<Sprite> f = new List<Sprite>();
            foreach (Sprite s in imageFrames)
            {
                f.Add(s);
            }
            a.setFrames(f);
        }
        return a;
    }

    public void copyFrames(Animation a)
    {
        imageFrames = new List<Sprite>();
        if (a.getFrames() != null)
        {
            foreach (Sprite s in a.getFrames())
            {
                imageFrames.Add(s);
            }
            frameNum = imageFrames.Count;
        }
    }

    public List<Sprite> getFrames()
    {
        return imageFrames;
    }

    public string getName()
    {
        return name;
    }

    void setNums(float t, bool r, float rp, int i, float dl,float en, float sc)
    {
        totalDuration = t;
        repeat = r;
        repeatDuration = rp;
        frameNum = i;
        delay = dl;
        ending = en;
        scale = sc;

    }

    void setFrames(List<Sprite> f)
    {
        imageFrames = f;
    }

    void calcTickTimes()
    {
        tickTimes = new List<float>();
        int n = imageFrames.Count;
        
        if (n <=0)
        {
            n = 1;
        }
        float startTime = 0.0f + delay;

        if (!repeat)
        {
            float div = totalDuration / (n+1);
            tickTimes.Add(startTime);
            for (int i = 0; i < (n+1); i++)
            {
                startTime += div;
                if (i == n-1 && ending > 0)
                {
                    startTime += ending;
                }

                tickTimes.Add(startTime);                
            }

        }
        else
        {
            if (repeatDuration <= 0)
            {
                repeatDuration = 1.0f;
            }
            tickTimes.Add(startTime);

            float div = repeatDuration / (n);
            while (startTime < totalDuration)
            {
                startTime += div;
                tickTimes.Add(startTime);
            }
        }

        totalDuration += delay;
        totalDuration += ending;
    }

    public void play(Character c)
    {
        frameNum = 0;
        tickPlace = 0;
        animPlace = 0;

        timer = 0.0f;
        target = c;
        createDoll(c);
        calcTickTimes();
        checkAnimation();
    }

    public void die()
    {
        target.removeAnimation(this);

        if (imagePlace != null)
        {
            GameObject.Destroy(imagePlace.gameObject);
        }
        
    }


    void calcTickNext()
    {
        tickPlace += 1;
        animPlace += 1;
        if (animPlace > imageFrames.Count)
        {
            if (!repeat)
            {
                die();
            }

            animPlace = 0;
        }
    }

    void setAnimation()
    {
        if (animPlace < imageFrames.Count)
        {
            imagePlace.GetComponent<Image>().sprite = imageFrames[animPlace];
        }
    }

    void checkAnimation()
    {
        if (timer >= tickTimes[tickPlace])
        {
            setAnimation();
            calcTickNext();
        }

        if (timer >= totalDuration)
        {
            die();
        }
    }

    public void fight(float f)
    {
        timer += f;
        checkAnimation();
    }

    void createDoll(Character c)
    {
        GameObject build = Resources.Load<GameObject>("UI/animationImage") as GameObject;
        GameObject temp = GameObject.Instantiate(build);
        temp.transform.SetParent(c.getAnimationParent());
        temp.transform.localScale = new Vector2(scale, scale);
        Transform t = c.getDoll();
        temp.transform.position = t.position;

        imagePlace = temp.transform;

    }


    private static int SortByName(Sprite o1, Sprite o2)
    {
        return o1.name.CompareTo(o2.name);
    }

    public void loadImage(string n)
    {
        string folder = "Battle/Animations/" + n;

        UnityEngine.Object[] images;

        imageFrames = new List<Sprite>();
        images = Resources.LoadAll(folder, typeof(Sprite));

        if (images != null)
        {

            //icons = new Sprite[images.Length];

            for (int i = 0; i < images.Length; i++)
            {
                if (images[i] is Sprite)
                {

                    Sprite sp = (Sprite)images[i];
                    sp.texture.filterMode = FilterMode.Point;
                    //icons[i] = (Sprite)images[i];
                    imageFrames.Add(sp);
                }
            }
            imageFrames.Sort(SortByName);
            frameNum = imageFrames.Count;
        }
    }
}
