using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCard",menuName ="Card")]
public class CardTutObject : ScriptableObject
{
    public string text;
    public bool used = false;
    public int lastForHowLong;

}
