using UnityEngine;
using System.Collections;

public class MapGen : MonoBehaviour {
    // Mapgen dynamics variables
    public int nTilesX = 10;
    public int nTilesZ = 10;
    public float tileRadius = 1f;


    // Temp mapgen dynamics variables
    public HexTile baseTile;

    // Internal variables
    HexTile[,] tiles;
    float[,] heightMap;
    float[,] waterMap;

    // Internal functions
    private void initTiles() {
        tiles = new HexTile[nTilesX, nTilesZ];
        constructHeightMap();
        constructWaterMap();
        constructTiles();
        linkTiles();
    }
    private void constructHeightMap() {
        // TODO
    }
    private void constructWaterMap() {
        // TODO
    }
    private void constructTiles() {
        float xOffset = (3f / 2f) * tileRadius;
        float zOffset = Mathf.Sqrt(3f / 4f) * tileRadius;
        for (int x = 0 ; x < nTilesX ; x++) {
            for (int z = 0 ; z < nTilesZ ; z++) {
                float height = 1f; // TODO
                Vector3 newLocation = new Vector3((float)x * xOffset,
                                                  height,
                                                  ((2f * z) + (x % 2)) * zOffset);
                tiles[x, z] = Instantiate(baseTile, newLocation, Quaternion.identity) as HexTile;
            }
        }
    }
    private void linkTiles() {
        for (int z = 0 ; z < nTilesZ ; z++) {
            bool top = (z == nTilesZ - 1);
            bool bottom = (z == 0);
            bool left;
            bool right;
            for (int x = 0 ; x < nTilesX ; x += 2) {
                right = (x == nTilesX - 1);
                left = (x == 0);
                HexTile[] neighbours = {(top ? null : tiles[x, z+1]),
                                        (right? null : tiles[x+1, z]),
                                        ((right || bottom) ? null : tiles[x+1, z-1]),
                                        (bottom ? null : tiles[x, z-1]),
                                        ((left || bottom) ? null : tiles[x-1, z-1]),
                                        (left ? null : tiles[x-1, z])};
                float moisture = 10f; // TODO
                tiles[x, z].setType(neighbours, true, moisture);
            }
            for (int x = 1 ; x < nTilesX ; x += 2) {
                right = (x == nTilesX - 1);
                left = false;
                HexTile[] neighbours = {(top ? null : tiles[x, z+1]),
                                        ((top || right) ? null : tiles[x+1, z+1]),
                                        (right ? null : tiles[x+1, z]),
                                        (bottom ? null : tiles[x, z-1]),
                                        (left ? null : tiles[x-1, z]),
                                        ((top || left) ? null : tiles[x-1, z+1])};
                float moisture = 10f; // TODO
                tiles[x, z].setType(neighbours, true, moisture);
            }
        }
    }

    // Unity logic functions
    void Start() {
        Debug.Log("In MapGen::start()");
        initTiles();
    }
}
