using UnityEngine;
using System.Collections;

public class MapGen : MonoBehaviour {
    // Mapgen dynamics variables
    public int nTilesX = 10;
    public int nTilesZ = 10;
    public float tileRadius = 1f;
    public float heightFactor = 10f;
    public float heightOffset = 0.5f;
    public float perlinFactor = 0.05f;
    public float distanceFromWaterFactor = 0.1f;
    public int maxDistanceFromWater = 30;
    public int nFoodSources = 5;
    public int nNests = 1;

    // Temp mapgen dynamics variables
    public HexTile baseTile;
    public FoodSource foodSourceTemplate;
    public Nest nestTemplate;

    // Internal variables
    private HexTile[,] tiles;
    private float[,] heightMap;
    private float[,] waterMap;

    // Internal functions
    private void initTiles() {
        tiles = new HexTile[nTilesX, nTilesZ];
        int szX = nTilesX * 2 - 1;
        int szY = nTilesZ * 2;
        constructHeightMap(szX, szY);
        constructWaterMap(szX, szY);
        constructTiles();
        linkTiles();
        assignFoodSourcesAndNests();
    }
    private void constructHeightMap(int szX, int szY) {
        heightMap = new float[szX, szY];
        float xOffset = Random.value;
        float yOffset = Random.value;
        for (int x = 0 ; x < szX ; x++) {
            for (int y = 0 ; y < szY ; y++) {
                heightMap[x, y] = Mathf.Pow(Mathf.PerlinNoise(perlinFactor * x + xOffset,
                                                              perlinFactor * y + yOffset), 2) + heightOffset;
            }
        }
    }
    private void constructWaterMap(int szX, int szY) {
        waterMap = new float[szX, szY];
        // Generate distanceFromWater values
        int[,] distanceFromWater = new int[szX, szY];
        for (int x = 0 ; x < szX ; x++)
            for (int y = 0 ; y < szY ; y++)
                distanceFromWater[x, y] = (heightMap[x, y] < 0 ? 0 : 30);
        bool newValue = true;
        while (newValue) {
            newValue = false;
            for (int x = 0 ; x < szX ; x++) {
                for (int y = 0 ; y < szY ; y++) {
                    if (x > 0 && distanceFromWater[x - 1, y] + 1 < distanceFromWater[x, y]) {
                        distanceFromWater[x, y] = distanceFromWater[x - 1, y] + 1;
                        newValue = true;
                    }
                    if (x < szX - 1 && distanceFromWater[x + 1, y] + 1 < distanceFromWater[x, y]) {
                        distanceFromWater[x, y] = distanceFromWater[x + 1, y] + 1;
                        newValue = true;
                    }
                    if (y > 0 && distanceFromWater[x, y - 1] + 1 < distanceFromWater[x, y]) {
                        distanceFromWater[x, y] = distanceFromWater[x, y - 1] + 1;
                        newValue = true;
                    }
                    if (y < szY - 1 && distanceFromWater[x, y + 1] + 1 < distanceFromWater[x, y]) {
                        distanceFromWater[x, y] = distanceFromWater[x, y + 1] + 1;
                        newValue = true;
                    }
                    distanceFromWater[x, y] = Mathf.Min(distanceFromWater[x, y], 30);
                }
            }
        }
        // Generate complete water map
        float xOffset = Random.value;
        float yOffset = Random.value;
        for (int x = 0 ; x < heightMap.GetLength(0) ; x++) {
            for (int y = 0 ; y < heightMap.GetLength(1) ; y++) {
                float actualWater = distanceFromWaterFactor * (maxDistanceFromWater - distanceFromWater[x, y]) / maxDistanceFromWater;
                float randomWater = 0.1f * Mathf.PerlinNoise(perlinFactor * x + xOffset, perlinFactor * y + yOffset);
                waterMap[x, y] = actualWater + randomWater;
            }
        }
    }
    private void constructTiles() {
        GameObject tileParent = GameObject.Find("_Tiles");
        float xOffset = (3f / 2f) * tileRadius;
        float zOffset = Mathf.Sqrt(3f / 4f) * tileRadius;
        for (int x = 0 ; x < nTilesX ; x++) {
            for (int z = 0 ; z < nTilesZ ; z++) {
                float height = heightMap[2 * x, 2 * z + (x % 2)];
                Vector3 newLocation = new Vector3((float)x * xOffset,
                                                  height * heightFactor,
                                                  ((2f * z) + (x % 2)) * zOffset);
                tiles[x, z] = Instantiate(baseTile, newLocation, Quaternion.identity) as HexTile;
                tiles[x, z].transform.parent = tileParent.transform;
                tiles[x, z].gameObject.name = "Tile " + x + "," + z;
                tiles[x, z].height = height;
            }
        }
        // Recenter Tiles_ so we're constructing around the origin
        Vector3 offset = tiles[tiles.GetLength(0) - 1, tiles.GetLength(1) - 1].transform.position - tiles[0, 0].transform.position;
        offset.y = 0;  
        GameObject.Find("_Tiles").transform.position -= (offset / 2);
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
                float moisture = waterMap[2 * x, 2 * z + (x % 2)];
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
                float moisture = waterMap[2 * x, 2 * z + (x % 2)];
                tiles[x, z].setType(neighbours, true, moisture);
            }
        }
    }
    void addFoodSource(HexTile tile) {
        FoodSource foodSource = Instantiate(foodSourceTemplate, tile.transform.position, Quaternion.identity) as FoodSource;
        foodSource.transform.parent = tile.gameObject.transform;
        tile.findFoodSource();
    }
    void addNest(HexTile tile) {
        Nest nest = Instantiate(nestTemplate, tile.transform.position, Quaternion.identity) as Nest;
        nest.transform.parent = tile.gameObject.transform;
        nest.setLocation(tile);
        tile.findNest();
    }
    void assignFoodSourcesAndNests() {
        int currentFoodSources = 0;
        while (currentFoodSources < nFoodSources) {
            HexTile tile = tiles[Random.Range(0, nTilesX - 1), Random.Range(0, nTilesZ - 1)];
            if (tile != null && !tile.isFoodSource() && !tile.isNest()) {
                addFoodSource(tile);
                currentFoodSources++;
            }
        }
        int currentNests = 0;
        while (currentNests < nNests) {
            int x = Random.Range(0, nTilesX - 1);
            int z = Random.Range(0, nTilesZ - 1);
            HexTile tile = tiles[x, z];
            if (tile != null && !tile.isFoodSource() && !tile.isNest()) {
                addNest(tile);
                currentNests++;
            }
        }
    }

    // Unity logic functions
    void Start() {
        initTiles();
    }
}
