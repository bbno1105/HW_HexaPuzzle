using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BLOCK_TYPE
{
    BLUE = 0,
    GREEN,
    ORANGE,
    PUPPLE,
    RED,
    YELLOW
}

public class Block : MonoBehaviour
{
    BLOCK_TYPE blockType;
    public BLOCK_TYPE BlockType { get { return blockType; } set { blockType = value; } }

    Queue<int> moveTile = new Queue<int>();
    public Queue<int> MoveTile { get { return moveTile; } set { moveTile = value; } }

    bool canMove;
    public bool CanMove { get { return canMove; } set { canMove = value; } }

    [SerializeField] string blue;
    [SerializeField] string green;
    [SerializeField] string orange;
    [SerializeField] string pupple;
    [SerializeField] string red;
    [SerializeField] string yellow;

    Vector2 startPosition;
    Vector2 endPosition;
    float lerpValue = 0;

    void OnEnable()
    {
        canMove = true;
        Initialize();
    }

    void Update()
    {
        if(canMove == false)
        {
            transform.position = Vector2.Lerp(startPosition, endPosition, lerpValue);
            lerpValue += 20 * Time.deltaTime;

            if (1 < lerpValue)
            {
                transform.position = endPosition;
                lerpValue = 0;
                CanMove = true;
            }
        }
    }

    void Initialize()
    {
        string resourcesName = string.Empty;
        switch (blockType)
        {
            case BLOCK_TYPE.BLUE:
                resourcesName = blue;
                break;
            case BLOCK_TYPE.GREEN:
                resourcesName = green;
                break;
            case BLOCK_TYPE.ORANGE:
                resourcesName = orange;
                break;
            case BLOCK_TYPE.PUPPLE:
                resourcesName = pupple;
                break;
            case BLOCK_TYPE.RED:
                resourcesName = red;
                break;
            case BLOCK_TYPE.YELLOW:
                resourcesName = yellow;
                break;
            default:
                break;
        }
        GetComponent<Image>().sprite = Resources.Load<Sprite>(resourcesName);
    }

    public void MoveSetting(Vector2 _startPosition, Vector3 _endPosition)
    {
        startPosition = _startPosition;
        endPosition = _endPosition;
        canMove = false;
    }
}
