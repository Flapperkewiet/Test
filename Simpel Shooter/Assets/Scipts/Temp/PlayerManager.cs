using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    List<Participant> participants;
    public byte NumberOfParticipants;

    void Awake()
    {
        participants = new List<Participant>();

    }


}
