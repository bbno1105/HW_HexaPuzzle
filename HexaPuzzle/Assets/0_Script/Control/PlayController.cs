using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PLAYSTATE
{
    READY = 0,
    PLAY,
    MOVEBLOCK,
    FALLBLOCK,
    CHECKBLOCK,
    END
}

public class PlayController : SingletonBehaviour<PlayController>
{
    PLAYSTATE playState;
    public PLAYSTATE PlayState { get { return playState; } set { playState = value; UnityEngine.Debug.Log("PlayState : " + PlayState); } }

    // ���� ����
    [Header("���� �ӵ� ����")]
    [SerializeField] float gameSpeed;
    public float GameSpeed { get { return gameSpeed; } set { gameSpeed = value; } }

    float gameDelayTime = 0;
    public float GameDelayTime { get { return gameDelayTime; } set { gameDelayTime = value; } }

    // ��������
    [SerializeField] Block missionBlock;
    int missionCount;
    public int MissionCount
    {
        get { return missionCount; }
        set
        {
            if (value < 0) value = 0;
            missionCount = value;
            UIManager.Instance.SetMissionCount(missionCount);
        }
    }
    int playCount;
    public int PlayCount 
    { 
        get { return playCount; } 
        set 
        {
            if (value < 0) value = 0;
            playCount = value; 
            UIManager.Instance.SetPlayCount(playCount); 
        } 
    }

    private void Start()
    {
        PlayController.Instance.PlayState = PLAYSTATE.READY;

        if (StaticData.StageData.TryGetValue(21, out StageSheetData stageData))
        {
            for (int i = 0; i < stageData.Missiontype.Length; i++)
            {
                missionBlock.BlockType = (BLOCK_TYPE)stageData.Missiontype[i];
                missionBlock.Initialize();
            }

            for (int i = 0; i < stageData.Clearcount.Length ; i++)
            {
                MissionCount = stageData.Clearcount[i];
            }

            PlayCount = stageData.Movecount;
        }

        SoundManager.Instance.PlayBGM("BGM");
        SoundManager.Instance.SetBGMVolume(0.5f);
    }

    void Update()
    {
        GameDelayTime += GameSpeed * Time.deltaTime;
        if (2f < GameDelayTime)
        {
            switch (playState)
            {
                case PLAYSTATE.READY: // �÷��� �غ� ( ���� ��.. )
                    {
                        PlayState = PLAYSTATE.FALLBLOCK;
                    }
                    break;

                case PLAYSTATE.PLAY: // �÷��̾� ����
                    {
                        if(missionCount <= 0)
                        {
                            // Clear
                            UnityEngine.Debug.Log("Ŭ����");
                            UIManager.Instance.SetClearUI("Clear");
                            PlayState = PLAYSTATE.END;
                        }
                        else if(playCount <= 0)
                        {
                            // GameOver
                            UnityEngine.Debug.Log("Game Over");
                            UIManager.Instance.SetClearUI("Clear");
                            PlayState = PLAYSTATE.END;
                        }
                    }
                    break;

                case PLAYSTATE.MOVEBLOCK: // �� �̵�
                    {
                        if (ThreeMatch()) // ��ġ �Ǵ��Ͽ� ����
                        {
                            PlayCount--;
                            PlayState = PLAYSTATE.FALLBLOCK;
                        }
                        else // ��ġ���� �ʾ����� �ǵ�����
                        {
                            PlayState = PLAYSTATE.PLAY;
                            BlockController.Instance.RemoveBlock();
                        }
                        GameDelayTime = 0;
                    }
                    break;

                case PLAYSTATE.FALLBLOCK: // ��� �̵�
                    {
                        if(TileController.Instance.CheckTileList() == false)
                        {
                            PlayState = PLAYSTATE.CHECKBLOCK;
                        }
                        GameDelayTime = 0;
                    }
                    break;
                case PLAYSTATE.CHECKBLOCK: // ��� ��ġ
                    {
                        if (ThreeMatch()) // 3�̻� ���� ��ġ üũ
                        {
                            PlayState = PLAYSTATE.FALLBLOCK;
                        }
                        else
                        {
                            PlayState = PLAYSTATE.PLAY;
                        }
                        GameDelayTime = 0;
                    }
                    break;

                case PLAYSTATE.END: // ���� ����
                    if(Input.GetMouseButton(0))
                    {
                        SceneManager.LoadScene(0);
                    }
                    break;

                default:
                    break;
            }
        }
    }

    // ��ġ
    public bool ThreeMatch()
    {
        bool isMatched = false;

        for (int i = 0; i < TileController.Instance.TileList.Count; i++)
        {
            Tile targetTile = TileController.Instance.TileList[i];

            if (targetTile.NowBlock == false) continue; // ��� ���� üũ
            if ((int)targetTile.NowBlock.BlockType > 100) continue; // �Ϲ� ��� üũ

            List<int> check = MatchController.Instance.CheckStraightMatch(targetTile);
            
            // ��� ����
            for (int j = 0; j < check.Count; j++)
            {
                AttackMissionBlock(TileController.Instance.TileList[check[j]]);

                TileController.Instance.TileList[check[j]].NowBlock.DeActiveAnimation();
                TileController.Instance.TileList[check[j]].NowBlock = null;

                SoundManager.Instance.PlaySE("POP");

                isMatched = true;
            }

            // �̼Ǻ�� ������ó��
            for (int j = 0; j < BlockController.Instance.DamagedTile.Count; j++)
            {
                Tile targetBlock = BlockController.Instance.DamagedTile.Dequeue();
                targetBlock.Damaged();
            }
        }

        return isMatched;
    }

    // ������
    void AttackMissionBlock(Tile _tile)
    {
        for (int i = 0; i < _tile.TileAround.Length; i++)
        {
            if (_tile.TileAround[i] == -1) continue;

            Tile target = TileController.Instance.TileList[_tile.TileAround[i]];
            if (target.NowBlock == false) continue;

            if (BLOCK_TYPE.MISSION < target.NowBlock.BlockType && target.NowBlock.IsDamaged == false)
            {
                target.NowBlock.IsDamaged = true;
                BlockController.Instance.DamagedTile.Enqueue(target);
            }
        }
    }
}
