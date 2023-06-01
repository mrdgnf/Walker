using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    public int currentStandIndex = 0;

    public int place = 1;

    public int countOfBonuses = 0;

    public int countOfPenalty = 0;

    public string name;

    public GameObject gameObject;
    public Player(string name, GameObject gameObject)
    {
        this.name = name;   
        this.gameObject = gameObject;
    }
}
public class PlayersLogic : MonoBehaviour
{

    public List<GameObject> playersGameObjects;

    public List<Player> players;

    void Start()
    {
        int count = 0;
        foreach (var playerSetting in StartSceneLogic.finallyPlayerSettings)
        {
            players.Add(new Player(playerSetting.Item1, playersGameObjects[count]));

            players[count].gameObject.GetComponent<MeshRenderer>().material = playerSetting.Item2;

            players[count].gameObject.SetActive(true);

            count++;
        }
    }
    
}
