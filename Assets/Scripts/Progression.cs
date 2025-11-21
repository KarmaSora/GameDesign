using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Progression : MonoBehaviour
{
    // Start is called before the first frame update


    private float experincePoints;
    private float timer;

    public int levelup = 0;
    private TextMeshProUGUI experincePointText;
    private Slider slider;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer = Time.deltaTime;
        if(timer > 1)
        {
            GetXP();
            timer = 0;
        }
        
    }

    private void GetXP()
    {
        experincePoints += 10;
        slider.value = experincePoints;
        experincePointText.text = experincePoints + "/100";
        if (experincePoints >= 100)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        experincePoints = 0;
        slider.value = experincePoints;

    }

}
