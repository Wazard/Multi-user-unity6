using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhotonEvents
{
    public const byte OBJECT_ASSEMBLED_EVENT = 1;
    public const byte OBJECT_DISASSEMBLED_EVENT = 2;
    public const byte OBJECT_TAKEN_IN_HAND = 3;
    public const byte OBJECT_DESTRUCTION = 4;
    public const byte ASSEMBLABLE_OBJECT_DESTRUCTION = 5;
    public const byte ASSEMBLABLE_OBJECT_CREATION = 6;
    public const byte BELT_TOGGLE_EVENT = 7;
}
