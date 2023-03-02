using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : SingletonBehaviour<PlayController>
{
    enum PLAYSTATE
    {
        READY,
        PLAY,
        FALLBLOCK,
        END
    }
    PLAYSTATE playState;

    void Start()
    {
        StartCoroutine(TEST());
    }

    void Update()
    {
        switch (playState)
        {
            case PLAYSTATE.READY:
                break;
            case PLAYSTATE.PLAY:
                break;
            case PLAYSTATE.FALLBLOCK:
                break;
            case PLAYSTATE.END:
                break;
            default:
                break;
        }

        // TEST CODE
        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < 4; i++)
            {
                TileController.Instance.TileList[7 + i * 9].NowBlock.gameObject.SetActive(false);
                TileController.Instance.TileList[7 + i * 9].NowBlock = null;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            for (int i = 0; i < 4; i++)
            {
                TileController.Instance.TileList[2 + i * 9].NowBlock.gameObject.SetActive(false);
                TileController.Instance.TileList[2 + i * 9].NowBlock = null;

                TileController.Instance.TileList[7 + i * 9].NowBlock.gameObject.SetActive(false);
                TileController.Instance.TileList[7 + i * 9].NowBlock = null;
            }
        }
    }

    IEnumerator TEST()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(0.05f);
            TileController.Instance.CheckTileList();
        }
    }
}
