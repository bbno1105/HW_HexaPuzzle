using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // 게임 설정
    [Header("게임 속도 설정")]
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
        GameDelayTime += GameSpeed * Time.deltaTime;
        if (2f < GameDelayTime)
        {
            switch (playState)
            {
                case PLAYSTATE.READY: // 플레이 준비 ( 도움말 등.. )
                    {
                        PlayState = PLAYSTATE.FALLBLOCK;
                    }
                    break;

                case PLAYSTATE.PLAY: // 플레이어 조작
                    {

                    }
                    break;

                case PLAYSTATE.MOVEBLOCK: // 블럭 이동
                    {
                        if (ThreeMatch()) // 매치 판단하여 진행
                        {
                            PlayState = PLAYSTATE.FALLBLOCK;
                        }
                        else // 매치되지 않았으면 되돌리기
                        {
                            PlayState = PLAYSTATE.PLAY;
                            BlockController.Instance.RemoveBlock();
                        }
                        GameDelayTime = 0;
                    }
                    break;

                case PLAYSTATE.FALLBLOCK: // 블록 이동
                    {
                        if(TileController.Instance.CheckTileList() == false)
                        {
                            PlayState = PLAYSTATE.CHECKBLOCK;
                        }
                        GameDelayTime = 0;
                    }
                    break;
                case PLAYSTATE.CHECKBLOCK: // 블록 매치
                    {
                        if (ThreeMatch()) // 3이상 직선 매치 체크
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

                case PLAYSTATE.END: // 게임 종료
                    break;

                default:
                    break;
            }
        }
    }

    // 매치
    public bool ThreeMatch()
    {
        bool isMatched = false;

        for (int i = 0; i < TileController.Instance.TileList.Count; i++)
        {
            Tile targetTile = TileController.Instance.TileList[i];

            if (targetTile.NowBlock == false) continue; // 블록 유무 체크
            if ((int)targetTile.NowBlock.BlockType > 100) continue; // 일반 블록 체크

            List<int> check = MatchController.Instance.CheckStraightMatch(targetTile);
            
            for (int j = 0; j < check.Count; j++)
            {
                AttackMissionBlock(TileController.Instance.TileList[check[j]]);

                TileController.Instance.TileList[check[j]].NowBlock.DeActiveAnimation();
                TileController.Instance.TileList[check[j]].NowBlock = null;

                isMatched = true;
            }

            // 미션블록 데미지처리
            for (int j = 0; j < BlockController.Instance.DamagedTile.Count; j++)
            {
                Tile targetBlock = BlockController.Instance.DamagedTile.Dequeue();
                targetBlock.Damaged();
            }
        }

        return isMatched;
    }

    // 데미지
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
