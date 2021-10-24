using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    Character character;
    List<Card> cards = new List<Card>();
    List<int> cardState = new List<int>();
    List<Card> passives = new List<Card>();
    List<int> passivesState = new List<int>();   
    List<Card> loadedCards = new List<Card>();

    List<Animation> loadedAnimations = new List<Animation>();
    List<CardSound> loadedSounds = new List<CardSound>();
    int maxDepthMisc = 5;
    int depthMisc = 5;

    //List<Card> hand = new List<Card>();
    //List<Card> grave = new List<Card>();
    // List<Card> dec = new List<Card>();
    //List<Card> discard = new List<Card>();

    //bool validDeck = true;
    // Start is called before the first frame update

    /*public object this[int i]
    {
        get { return cards[i]; }
        set { cards[i] = (Card)value; }
    }*/

    public object this[int i] => this.cards[i];

    public Deck(Character c)
    {
        character = c;
    }
    /*
    public void setupDeck()
    {
        foreach (Card car in cards)
        {
            Card cn = car.deepCopy();
            dec.Add(cn);
        }
    }
    */

    public List<Card> getCards()
    {
        return cards;
    }

    public Card getCardMisc(string name)
    {
        foreach(Card c in loadedCards)
        {
            if (c.getName() == name)
            {
                return c;
            }
        }

        Debug.Log("Cant find " + name);
        return null;
    }

    public void activatePassives()
    {
        
        foreach(Card p in passives)
        {
            character.useCard(p, character.getBattleLogic().getAdjacent(character));
        }

    }

    public void addAnimationFrames(Animation a)
    {
        bool needLoad = true;
        foreach(Animation al in loadedAnimations)
        {
            if (al.getName() == a.getName())
            {
                needLoad = false;
            }
        }

        if (needLoad)
        {
            loadAnimation(a.getName());
        }

        foreach (Animation al in loadedAnimations)
        {
            if (al.getName() == a.getName())
            {
                a.copyFrames(al);
            }
        }
    }


    public void loadAnimation(string name)
    {
        Animation a = new Animation(name);
        a.loadImage(name);
        loadedAnimations.Add(a);
    }

    public void loadSound(string name)
    {
        CardSound c = new CardSound(name);
        c.loadSound(name);
        loadedSounds.Add(c);

    }


    public CardSound findSound(string name)
    {
        bool wantLoad = true;
        foreach (CardSound c in loadedSounds)
        {
            if (c.getName() == name)
            {
                wantLoad = false;
            }
        }

        if (wantLoad)
        {
            loadSound(name);
        }
        

        foreach (CardSound c in loadedSounds)
        {
            if (c.getName() == name)
            {
                return c.deepCopy();
            }
        }


        return null;
    }



    public void loadMiscCard(string name)
    {
        bool wantLoad = true;

        foreach(Card c in loadedCards)
        {
            if (c.getName() == name)
            {
                wantLoad = false;
            }
        }
        Card tmp = character.loadCard(name);

        foreach (Card c in loadedCards)
        {
            if (tmp.getName() == c.getName())
            {
                wantLoad = false;
            }
        }

        if (depthMisc > 0)
        {
            if (wantLoad)
            {
                depthMisc--;
                loadedCards.Add(tmp);
                tmp.setupCard(name, this);
                
            }
            else
            {
            }
        }
        else
        {
            Debug.Log("Depth exceeded");
        }
    }

    public List<Card> getHand()
    {

        return cards;
        //return hand;
    }



    int getMaxHandSize()
    {
        return (int)parseNum(character, character, character.getBaseStat("handsize"));
    }
    /*
    public List<Card> getDeck(int num = -1,int ordered = -1)
    {
        return dec;
    }
    


    void deckOut()
    {
        validDeck = false;
    }
    
    void checkEmpty()
    {
        if (dec.Count <= 0)
        {
            discardToDraw();
        }

        if (dec.Count <= 0)
        {
            deckOut();
        }

    }
    

    void drawOne()
    {
        checkEmpty();

        if (validDeck)
        {
            Card c = dec[0];
        }
        
    }

    public int getCurrentHandSize()
    {
        return hand.Count;
    }

    public void shuffle()
    {
        for (int i = 0; i < dec.Count; i++)
        {
            Card temp = dec[i];
            int randomIndex = Random.Range(i, dec.Count);
            dec[i] = dec[randomIndex];
            dec[randomIndex] = temp;
        }

    }
    public void draw(int amt)
    {
        int min = Mathf.Min(amt, getMaxHandSize() - getCurrentHandSize());
        for (var i=0;i<min;i++)
        {
            drawOne();
        }
    }
    */

    public int length()
    {
        return cards.Count;
    }

    public void addCard(Card c, int state)
    {
        depthMisc = maxDepthMisc;
        c.setupCard(c.getName(), this);
        cards.Add(c);
        cardState.Add(state);

    }

    public void addPassive(Card c, int state)
    {
        depthMisc = maxDepthMisc;
        c.setupCard(c.getName(), this);
        passives.Add(c);
        passivesState.Add(state);

    }

    public float parseNum(Character tar,Character own, string num)
    {
        return character.parseNum(tar,own,num);
    }

    public Character getCharacter()
    {
        return character;
    }

    public void setCharacter(Character c)
    {
        character = c;
    }

    public Deck(Deck prev)
    {

    }

    public Deck deepCopy()
    {
        Deck td = new Deck(character);

        //foreach (Card car in cards)
        //{

        for (int i = 0; i< cards.Count; i++)
        {

            Card carCopy = cards[i].deepCopy();
            td.addCard(carCopy, cardState[i]);
        }

        for (int i = 0; i < passives.Count; i++)
        {

            Card carCopy = passives[i].deepCopy();
            td.addPassive(carCopy, passivesState[i]);
        }

        return td;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
