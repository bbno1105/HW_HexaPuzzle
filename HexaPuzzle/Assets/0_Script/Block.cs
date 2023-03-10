using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BLOCK_TYPE
{
    EMPTY = 0,

    // 일반블록
    BLUE = 1,
    GREEN,
    ORANGE,
    PUPPLE,
    RED,
    YELLOW,

    // 미션블록
    MISSION = 100,
    TOP_DEFAULT,
    TOP_BROKEN
}

public enum BLOCK_STATE
{
    STOP = 0,
    MOVE
}

public class Block : MonoBehaviour
{
    BLOCK_TYPE blockType;
    public BLOCK_TYPE BlockType { get { return blockType; } set { blockType = value; } }

    BLOCK_STATE blockState;
    public BLOCK_STATE BlockState { get { return blockState; } set { blockState = value; } }

    //Queue<int> moveTile = new Queue<int>();
    //public Queue<int> MoveTile { get { return moveTile; } set { moveTile = value; } }

    Animator animator;

    [Header("일반블록")]
    [SerializeField] string blue;
    [SerializeField] string green;
    [SerializeField] string orange;
    [SerializeField] string pupple;
    [SerializeField] string red;
    [SerializeField] string yellow;

    [Header("미션블록")]
    [SerializeField] string topDefault;
    [SerializeField] string topBroken;

    // 블럭 이동
    Vector2 startPosition;
    Vector2 endPosition;

    float moveLerp = 0;

    // 공격받은 블럭
    bool isDamaged = false;
    public bool IsDamaged { get { return isDamaged; } set { isDamaged = value; } }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        BlockState = BLOCK_STATE.STOP;
        startPosition = transform.position;
        endPosition = transform.position;

        Initialize();
    }

    public void Initialize()
    {
        isDamaged = false;

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
            case BLOCK_TYPE.TOP_DEFAULT:
                resourcesName = topDefault;
                break;
            case BLOCK_TYPE.TOP_BROKEN:
                resourcesName = topBroken;
                break;
            default:
                break;
        }
        GetComponent<Image>().sprite = Resources.Load<Sprite>(resourcesName);
    }

    void Update()
    {
        switch (BlockState)
        {
            case BLOCK_STATE.STOP:
                {
                    transform.position = endPosition;
                }
                break;

            case BLOCK_STATE.MOVE:
                {
                    moveLerp = PlayController.Instance.GameDelayTime;
                    transform.position = Vector2.Lerp(startPosition, endPosition, moveLerp);
                    if (1 < moveLerp)
                    {
                        BlockState = BLOCK_STATE.STOP;
                    }
                }
                break;

            default:
                break;
        }
    }

    public void MoveSetting(Vector2 _startPosition, Vector3 _endPosition)
    {
        startPosition = _startPosition;
        endPosition = _endPosition;
        moveLerp = 0;
        PlayController.Instance.GameDelayTime = 0;
        BlockState = BLOCK_STATE.MOVE;
    }

    // 블록 터지는 연출 애니메이션
    public void DeActiveAnimation()
    {
        animator.SetTrigger(AnimString.IsPop);
    }

    public void DeActive() // Animation Event
    {
        gameObject.SetActive(false);
    }

    // 블록 상태 변화
    public bool Damaged()
    {
        // 데미지
        IsDamaged = false;


        return false;
    }
}
