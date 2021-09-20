using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int points;

    public void Break()
    {
        Destroy(this.gameObject);
    }
}
