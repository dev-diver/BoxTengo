using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BoxTengo/Game Config", fileName = "GameConfig.asset")]
public class GameConfig : ScriptableObject
{
    [Header("Time")]
    public float totalTime; //60
    public float secondTime; // 45
    public float retryTime; //30
    public float subRetryTime; //5
    public float minRetryTime; //0

    [Header("Point")]
    public float allClearPoint; //10000

    [Header("Combo")]
    public float comboTime; //2.25f;;
    public float baseMaxComboTime; //0.35
    public float plusComboTime; //0.07
    public float maxComboTime; //1.5
}