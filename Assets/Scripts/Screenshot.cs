using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Screenshot : MonoBehaviour
{
    [SerializeField] private int superSize;
    private int screenshotCount;
    
    // Start is called before the first frame update
    void Start()
    {
        Texture2D[] screenshots = Resources.LoadAll<Texture2D>("Screenshots");

        foreach (var screenshot in screenshots)
        {
            int currentNumber = int.Parse(Regex.Match(screenshot.name, @"\d+").Value);
            if (currentNumber >  screenshotCount)
                screenshotCount = currentNumber;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            screenshotCount++;
            ScreenCapture.CaptureScreenshot("Assets/Resources/Screenshots/" + string.Format("{0:000}",screenshotCount) + ".png", superSize);
            
        }
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}
