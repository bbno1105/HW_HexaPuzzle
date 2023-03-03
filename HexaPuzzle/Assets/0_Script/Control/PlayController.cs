using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYSTATE
{
    READY,
    PLAY,
    FALLBLOCK,
    CHECKBLOCK,
    END
}

public class PlayController : SingletonBehaviour<PlayController>
{
    PLAYSTATE playState;
    public PLAYSTATE PlayState { get { return playState; } set { playState = value; UnityEngine.Debug.Log("PlayState : " + PlayState); } }

    [SerializeField] float gameSpeed;
    public float GameSpeed { get { return gameSpeed; } set { gameSpeed = value; } }

    float gameDelayTime = 0;
    public float GameDelayTime { get { return gameDelayTime; } set { gameDelayTime = value; } }

    private void Start()
    {
        PlayController.Instance.PlayState = PLAYSTATE.READY;
    }

    void Update()
    {
        switch (playState)
        {
            case PLAYSTATE.READY: // �÷��� �غ� ( ���� ��.. )
                {
                    PlayController.Instance.PlayState = PLAYSTATE.FALLBLOCK;
                }
                break;

            case PLAYSTATE.PLAY: // �÷��̾� ����
                {

                }
                break;

            case PLAYSTATE.FALLBLOCK: // ��� �̵�
                {
                    GameDelayTime += GameSpeed * Time.deltaTime;
                    if(2f < GameDelayTime)
                    {
                        if(TileController.Instance.CheckTileList() == false)
                        {
                            PlayState = PLAYSTATE.CHECKBLOCK;
                        }
                        GameDelayTime = 0;
                    }
                }
                break;
            case PLAYSTATE.CHECKBLOCK: // 3�̻� ��ġ üũ
                {
                    for (int i = 0; i < TileController.Instance.TileList.Count; i++)
                    {
                        if (TileController.Instance.TileList[i].NowBlock == false) continue; // ��� ���� üũ
                        // if ((int)TileController.Instance.TileList[i].NowBlock.BlockType > 100) continue; // �Ϲ� ��� üũ

                        List<int> check = MatchController.Instance.CheckStraightMatch(TileController.Instance.TileList[i]);
                        for (int j = 0; j < check.Count; j++)
                        {
                            TileController.Instance.TileList[check[j]].NowBlock.gameObject.SetActive(false);
                            TileController.Instance.TileList[check[j]].NowBlock = null;
                        }
                        PlayState = PLAYSTATE.FALLBLOCK;
                    }
                }
                break;

            case PLAYSTATE.END: // ���� ����
                break;

            default:
                break;
        }
    }
}
