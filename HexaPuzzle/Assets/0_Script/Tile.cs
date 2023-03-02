using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] int id;
    public int ID { get { return id; } set { id = value; } }

}
