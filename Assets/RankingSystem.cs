using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingSystem : MonoBehaviour
{
    public LevelUp playerLevel;

    [Header("Ranks")]
    [SerializeField] private int ranks;
    [SerializeField] private int qRank;
    [SerializeField] private int wRank;
    [SerializeField] private int eRank;
    [SerializeField] private int rRank;

    [Header("Ability Rank Up Text")]
    public TMP_Text qRankUpText;
    public TMP_Text wRankUpText;
    public TMP_Text eRankUpText;
    public TMP_Text rRankUpText;

    public void RankUp()
    {
        for (int currentRank = 0; currentRank <= 18; currentRank += 0)
        {
            if (playerLevel.currentLevel > currentRank)
            {
                ranks = playerLevel.currentLevel -= currentRank;
                currentRank = playerLevel.currentLevel;
            }
        }
    }

    public void RankUpQ()
    {
        if (ranks > 0 && qRank > Mathf.Ceil(playerLevel.currentLevel % 2))
        {
            ranks--;
            qRank++;
            qRankUpText.text = qRank.ToString();
        }
    }

    public void RankUpW()
    {
        if (ranks > 0 && wRank > Mathf.Ceil(playerLevel.currentLevel % 2))
        {
            ranks--;
            wRank++;
            wRankUpText.text = wRank.ToString();
        }
    }

    public void RankUpE()
    {
        if (ranks > 0 && eRank > Mathf.Ceil(playerLevel.currentLevel % 2))
        {
            ranks--;
            eRank++;
            eRankUpText.text = eRank.ToString();
        }
    }

    public void RankUpR()
    {
        if (ranks > 0 && rRank > Mathf.Floor(playerLevel.currentLevel % 6))
        {
            ranks--;
            rRank++;
            rRankUpText.text = rRank.ToString();
        }
    }
}
