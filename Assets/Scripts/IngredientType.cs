using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType
{
    // Value of enum is the percent alcohol of the drink
    // VALUES >100: Non-Alcoholic
    // VALUES <100: Alcoholic

    ////////////////////////////////////////
    // Liquor
    ////////////////////////////////////////
    Vodka = 39,
    Tequila = 40,
    TripleSec = 30,
    Gin = 41,
    Rum = 50,
    WhiteWine = 10,
    RedWhite = 12,
    Curacao = 42,
    Beer = 6,
    Whiskey = 35,

    ////////////////////////////////////////
    // Mixers
    ////////////////////////////////////////
    Cola = 101,
    OrangeJuice = 102,
    ClubSoda = 103,
    LemonJuice = 109,
    LimeJuice = 110,

    ////////////////////////////////////////
    // Garnishes/Other
    ////////////////////////////////////////
    Ice = 104,
    OrangeWedge = 105,
    LemonSlice = 106,
    LimeSlice = 107,
    Cherry = 108
}
