using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
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
    public List<GameObject> playersGameObjects = new ();

    public List<Player> players = new ();

    public List<Material> materials = new ();
    void Start()
    {
        int count = 0;
        foreach (var playerSetting in StartSceneLogic.finallyPlayerSettings)
        {
            players.Add(new Player(playerSetting.Item1, playersGameObjects[count]));

            players[count].gameObject.GetComponent<MeshRenderer>().material = GetMaterial(playerSetting.Item2);

            players[count].gameObject.SetActive(true);

            count++;
        }
    }

    public Material GetMaterial(string name)
    {
        return name switch
        {
            "������" => materials[0],
            "�������" => materials[1],
            "���������" => materials[2],
            "�������" => materials[3],
            "�������" => materials[4],
            "����������" => materials[5],
            "Ƹ����" => materials[6],
            "����������" => materials[7],
            _ => null,
        };
    }

    void Update()
    {
        
    }
}
