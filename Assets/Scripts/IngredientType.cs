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
    Curacao = 42,

    ////////////////////////////////////////
    // Mixers
    ////////////////////////////////////////
    Cola = 101,
    OrangeJuice = 102,
    ClubSoda = 103,

    ////////////////////////////////////////
    // Garnishes/Other
    ////////////////////////////////////////
    Ice = 104,
    OrangeWedge = 105
}
