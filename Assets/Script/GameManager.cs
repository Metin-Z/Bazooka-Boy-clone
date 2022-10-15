using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<GameObject> enemies;
    public List<GameObject> bombs;
    public GameObject player;
    public virtual void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public void LevelCheck()
    {
        if (enemies.Count == 0)
        {
            CanvasManager.instance.nextLevelUI.SetActive(true);
        }
        if (player == null || PlayerController.instance.bombCount == 2 && enemies.Count > 0 && bombs.Count == 0)
        {
            CanvasManager.instance.failLevelUI.SetActive(true);
        }
    }

}
