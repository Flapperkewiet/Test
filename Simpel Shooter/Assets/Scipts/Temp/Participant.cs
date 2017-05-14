using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Participant : Health
{
    private string _name;
    private byte _id;
    private byte _team;
    public string Name
    {
        get { return _name; }
        protected set { _name = value; }
    }
    public byte ID
    {
        get { return _id; }
        set { _id = value; }
    }
    public byte Team
    {
        get { return _team; }
        protected set { _team = value; }
    }

    public Participant(string name, byte id, byte team)
    {
        Name = name;
        ID = id;
        Team = team;
    }

}
