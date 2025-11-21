using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int level;
    public TextMeshProUGUI levelText;
    void Start()
    {
        levelText.text = "Level " + level;


    }

    public void IncreaseLevel()
    {
        level++;
        levelText.text = "Level " + level;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
