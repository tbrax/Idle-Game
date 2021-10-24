using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public string name;

    public List<Character> characters = new List<Character>();

    public Team(string s)
    {
        name = s;
    }

    public int getIndex(Character c)
    {

        return characters.IndexOf(c);
    }

    public int numCharacters()
    {
        return characters.Count;
    }
    public void addCharacter(Character c)
    {
        characters.Add(c);
        c.addTeam(this);
    }

    public List<Character> getChas()
    {
        return characters;
    }


}
