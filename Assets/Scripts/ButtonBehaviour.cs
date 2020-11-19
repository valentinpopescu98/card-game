using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    public void OnButtonPress()
    {
        GameController c = GameObject.Find("GameController").GetComponent<GameController>();
        c.reset();
    }
}
