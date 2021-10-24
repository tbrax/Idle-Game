using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Card
{
    
    public string cardName;
    //public CardCost cost;
    public List<Effect> effects;
    public string cardDesc;
    public float cooldown;


    [System.NonSerialized]
    Deck deck = null;



    public Card(string name,Deck d)
    {
        setupCard(name, d);
    }



    void setupDesc()
    {  
        if (cardDesc == null || cardDesc == "todo")
        {
            string temp = "";
            foreach (Effect eff in effects)
            {
                temp += eff.getDesc();
            }

            cardDesc = temp;
        }
    }

    public void setupCard(string name, Deck d)
    {
        deck = d;
        cardName = name;
        if (effects == null)
        {
            effects = new List<Effect>();
            Effect e = new Effect(this);
            effects.Add(e);
        }
        setupEffects();
        setupDesc();
    }

    public void saveCard()
    {
        deck.getCharacter().saveCard(this);
    }

    void setupEffects()
    {
        foreach (Effect eff in effects)
        {
            eff.setupEffect(this);
        }
    }


    public float getCooldown()
    {
        return cooldown;
    }

    public string getName()
    {
        return cardName;
    }

    public string getDesc()
    {
        return cardDesc;
    }

    public float parseNum(Character tar,Character own,string num)
    {
        return deck.parseNum(tar,own,num);
    }

    // Start is called before the first frame update

    void loadData(string data)
    {

    }

    public void setCooldown(float c)
    {
        cooldown = c;
    }

    public void addEffect(Effect e)
    {
        
        effects.Add(e);
    }


    public void setDeck(Deck d)
    {
        deck = d;
        saveCard();
    }

    public Character getCharacter()
    {
        return deck.getCharacter();
    }




    public void use(Character target, Character source)
    {
        source.saveCard(this);
        foreach (Effect eff in effects)
        {
            eff.use(source, target);
        }
    }

    public Deck getDeck()
    {
        return deck;
    }

    /*public List<DictionaryOfStringAndString> getCost()
    {
        return cost.getCost();
    }*/

    public void takeCost(Character c, int choice)
    {
        //cost.takeCost(c, choice);
    }

    public bool canAfford(Character c, int choice)
    {
        return true;
        //return cost.canAfford(c,this, choice);
    }

    public Card deepCopy()
    {
        Card c = new Card(getName(),deck);

        //CardCost cos = cost.deepCopy();

        //c.setCost(cos);

        foreach (Effect eff in effects)
        {
            Effect effCopy = eff.deepCopy();
            c.addEffect(effCopy);
        }
        return c;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
