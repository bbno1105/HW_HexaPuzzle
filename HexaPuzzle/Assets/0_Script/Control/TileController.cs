using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : SingletonBehaviour<TileController>
{
    [SerializeField] Transform TileMapParent;
    [SerializeField] GameObject tilePrefab;

    [SerializeField] int mapSizeY = 21;
    [SerializeField] int mapSizeX = 9;
    [SerializeField] Vector2 startPosition;

    int tileCount = 0;

    float tileSize;
    float distanceX;
    float distanceY;

    void Awake()
    {
        // tilePrefab.GetComponent<RectTransform>().sizeDelta = new Vector2();
        tileSize = tilePrefab.GetComponent<RectTransform>().sizeDelta.x;

        distanceX = tileSize * 1.5f;
        distanceY = tileSize / 4 * Mathf.Sqrt(3); // 1 : ¡î3 = tileSize/4 : distanceY
        UnityEngine.Debug.Log("distanceY : " + distanceY);

        startPosition = Vector2.zero;
    }

    void Start()
    {
        MakeTile();
    }

    void MakeTile()
    {
        for (int y = 0; y < mapSizeY; y++)
        {
            if (y % 2 == 0)
            {
                for (int x = 0; x < mapSizeX/2; x++)
                {
                    Vector2 tilePosition = new Vector2(startPosition.x + distanceX / 2 + distanceX * x, distanceY * y);
                    CreateTile(tilePosition);
                }
            }
            else
            {
                for (int x = 0; x < mapSizeX/2 + 1; x++)
                {
                    Vector2 tilePosition = new Vector2(startPosition.x + distanceX * x, distanceY * y);
                    CreateTile(tilePosition);
                }
            }
        }
    }

    void CreateTile(Vector2 _tilePosition)
    {
        GameObject newTile = Instantiate(tilePrefab, _tilePosition, Quaternion.identity, TileMapParent);
        newTile.GetComponent<Tile>().ID = tileCount++;
    }
}
