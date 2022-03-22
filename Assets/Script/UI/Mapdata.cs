using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mapdata : MonoBehaviour
{
    [SerializeField] private Text ClearTimeText;
    [SerializeField] private Image[] Flowers;

    [SerializeField] private Sprite Flower;

    private void Awake()
    {
        float ClearTime = 0;
        ClearTime = PlayerPrefs.GetFloat("Stage1_ClearTime");
        ClearTimeText.text = ClearTime == 0 ? "" : (ClearTime/60) < 10 ? "0" + 
            (ClearTime/60).ToString("f0") + ":" + (ClearTime % 60).ToString("f0") : 
            (ClearTime/60).ToString("f0") + ":" + (ClearTime % 60).ToString("f0");

        int flowerCount = 0;
        flowerCount = PlayerPrefs.GetInt("Stage1_FlowerCount", flowerCount);

        for(int i = 0; i < flowerCount; i++)
        {
            Flowers[i].sprite = Flower;
        }
    }
}
