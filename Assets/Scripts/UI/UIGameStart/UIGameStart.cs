using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameStart : MonoBehaviour
{
    public GameObject loadScence;
    public Slider slider;
    public Button startBtn;

    private void Start()
    {
        startBtn.onClick.AddListener(LoadNextLevel);
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel());
        startBtn.gameObject.SetActive(false);
    }
    
    IEnumerator LoadLevel()
    {
        loadScence.SetActive(true);
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        
        operation.allowSceneActivation = true;
        
        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;

            if (operation.progress >= 0.9f) slider.value = 1; 
            
            yield return null;
        }
    }
}
