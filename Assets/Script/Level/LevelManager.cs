using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public Level[] Levels;
    private GameObject lastLevelPrefab;
    private GameObject lastLevelController;
    public GameObject nextLevelUI;
    [HideInInspector] public List<GameObject> levelObjectList;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            NextLevel();
        }
    }
    public void Start()
    {
        InitializeLevel();
     
    }
    public void InitializeLevel()
    {
        Level currentLevel = GetCurrentLevel();

        levelObjectList.ForEach(x => Destroy(x));
        levelObjectList.Clear();

        if (currentLevel == null)
        {
            Debug.LogError("Level is null.");
            return;
        }

        if (lastLevelPrefab != null)
        {
            Destroy(lastLevelController);
        }
        lastLevelPrefab = Instantiate(currentLevel.Prefab);
        levelObjectList.Add(lastLevelPrefab);


    }
    public void NextLevel()
    {
        nextLevelUI.SetActive(false);           
        Level currentLevel = GetCurrentLevel();
        PlayerPrefs.SetInt(CommonTypes.LEVEL_DATA_KEY, currentLevel.Id + 1);
        InitializeLevel();
        PlayerPrefs.SetInt(CommonTypes.LEVEL_FAKE_DATA_KEY, PlayerPrefs.GetInt(CommonTypes.LEVEL_FAKE_DATA_KEY) + 1);      
    }
    public void RestartLevel()
    {
        CanvasManager.instance.failLevelUI.SetActive(false);
        InitializeLevel();     
    }
    public Level GetCurrentLevel()
    {
        int currentLevelId = PlayerPrefs.GetInt(CommonTypes.LEVEL_DATA_KEY);
        int totalLevelCount = Levels.Length;
        return Levels.SingleOrDefault(x => x.Id == currentLevelId % totalLevelCount);
    }
}