using UnityEngine;
using System.Collections;

public class Helper {
    public static int getCategory(float value, float[] categories) {
        for (int i=0 ; i<categories.Length ; i++)
            if (value < categories[i])
                return i - 1;
        return categories.Length - 1;
    }
    public static HexType getTileType(int heightCategory, int moistureCategory) {
        switch (heightCategory) {
            case 0:
                switch (moistureCategory) {
                    case 0:
                        return HexType.SubtropicalDesert;
                    case 1:
                        return HexType.Grassland;
                    case 2:
                    case 3:
                        return HexType.TropicalForest1;
                    case 4:
                    case 5:
                    default:
                        return HexType.TropicalForest0;
                }
            case 1:
                switch (moistureCategory) {
                    case 0:
                        return HexType.TemperateDesert;
                    case 1:
                    case 2:
                        return HexType.Grassland;
                    case 3:
                    case 4:
                        return HexType.TropicalForest1;
                    case 5:
                    default:
                        return HexType.TropicalForest0;
                }
            case 2:
                switch (moistureCategory) {
                    case 0:
                    case 1:
                        return HexType.TemperateDesert;
                    case 2:
                    case 3:
                        return HexType.Shrubland;
                    case 4:
                    case 5:
                    default:
                        return HexType.Taiga;
                }
            case 3:
                switch (moistureCategory) {
                    case 0:
                        return HexType.Scorched;
                    case 1:
                        return HexType.Bare;
                    case 2:
                        return HexType.Tundra;
                    case 3:
                    case 4:
                    case 5:
                    default:
                        return HexType.Snow;
                }
            default:
                return HexType.ERROR;
        }
    }
};
