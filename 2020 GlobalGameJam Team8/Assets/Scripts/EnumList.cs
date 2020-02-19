using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EModuleDirection
{
    UP = 0,
    DOWN,
    LEFT,
    RIGHT,
    UP_TO_RIGHT,
    UP_TO_LEFT,
    UP_TO_DOWN,
    RIGHT_TO_UP,
    RIGHT_TO_DOWN,
    RIGHT_TO_LEFT,
    DOWN_TO_RIGHT,
    DOWN_TO_LEFT,
    DOWN_TO_UP,
    LEFT_TO_DOWN,
    LEFT_TO_UP,
    LEFT_TO_RIGHT,
    CROSS,
    NO_UP,
    NO_LEFT,
    NO_DOWN,
    NO_RIGHT,
}

public enum EModuleType
{
    NONE = 0,
    MODULE_CREATER,
    MODULE_CLEANER,
    MODULE_CONVEYOR_LINE,
    MODULE_CONVEYOR_L_TYPE,
    PLAYER_SPAWN_POINT,
    BOSS,
    WALL,
    TUBE_START,
    TUBE_END,
    TUBE_LINE,
    TUBE_L_TYPE,
    TUBE_T_TYPE,
    TUBE_CROSS,
    HAMMER,
}