using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{    
    public int place = 1;

    public int countOfMoves = 0;

    public int countOfBonuses = 0;

    public int countOfPenalty = 0;

    public int currentStandIndex = 0;

    public string name;

    public Color color;

    public GameObject gameObject;   
    public Player(string name, GameObject gameObject)
    {
        this.name = name;   
        this.gameObject = gameObject;
    }
}
public class PlayersLogic : MonoBehaviour
{
    public int maxPlace = 1;

    public int MovingPlayerIndex = 0;

    public GameLogic gameLogic;

    public List<GameObject> playersGameObjects;

    public List<Player> players;

    public List<Player> finalists;


    void Start()
    {
        int count = 0;
        foreach (var playerSetting in StartSceneLogic.finallyPlayerSettings)
        {
            players.Add(new Player(playerSetting.Item1.Replace(" ", "_"), playersGameObjects[count]));

            players[count].gameObject.GetComponent<MeshRenderer>().material = playerSetting.Item2;

            players[count].color = playerSetting.Item2.color;

            players[count].gameObject.SetActive(true);

            count++;
        }
    }

    public void SetPlayerName() 
    {       
        gameLogic.playerNameOnScreen.text = players[MovingPlayerIndex].name;

        gameLogic.playerNameOnScreen.color = players[MovingPlayerIndex].color;

        gameLogic.textOnScreen.color = players[MovingPlayerIndex].color;
    }

    private void AllocationOfPlayrsPlaces()
    {
        var sortPlayers = players.OrderBy(p => p.currentStandIndex).ToList();

        for (int i = 0; i < players.Count; i++)
        {
            if (i > 0 && players[i].currentStandIndex == players[i - 1].currentStandIndex)
            {
                players[i].place = players[i - 1].place;
            }
            else
            {
                players[i].place = i + maxPlace;
            }
        }
    }

    private void IncrementIndex()
    {
        MovingPlayerIndex++;
        if (MovingPlayerIndex >= players.Count) MovingPlayerIndex = 0;
    }

    private void IncrementIndexAfterFinish()
    {
        if (MovingPlayerIndex > players.Count - 1)
            MovingPlayerIndex--;
    }

    private void PlayerFinish()
    {
        AllocationOfPlayrsPlaces();

        maxPlace++;

        finalists.Add(players[MovingPlayerIndex]);

        players.Remove(players[MovingPlayerIndex]);

        if (players.Count <= 0)
        {

            gameLogic.GameFinish();

            return;
        }

        IncrementIndexAfterFinish();

        SetPlayerName();

        Dice.playerMove = false;

        gameLogic.cameraPlayer.enabled = false;
    }

    private void EndMove()
    {
        AllocationOfPlayrsPlaces();

        IncrementIndex();

        SetPlayerName();

        Dice.playerMove = false;

        gameLogic.cameraPlayer.enabled = false;
    }

    private void BonusMove()
    {
        players[MovingPlayerIndex].countOfBonuses++;

        Dice.playerMove = false;

        gameLogic.cameraPlayer.enabled = false;
    }

    private IEnumerator MoveForwardPlayerAndCamera(int countOfSteps)
    {
        gameLogic.MoveCamera();

        for (int i = 0; i < countOfSteps; i++)
        {
            var player = players[MovingPlayerIndex];

            var distanceOfMovement
                = gameLogic.standsTransform[player.currentStandIndex + 1].position
                - gameLogic.standsTransform[player.currentStandIndex].position;

            gameLogic.MoveCamera();

            yield return new WaitForSeconds(0.5f);

            player.gameObject.transform.position += distanceOfMovement;

            player.currentStandIndex++;

            if (player.currentStandIndex >= 27)
            {
                gameLogic.MoveCamera();
                yield break;
            }
        }

        gameLogic.MoveCamera();
    }

    private IEnumerator MoveBackPlayerAndCamera(int countOfSteps)
    {
        gameLogic.MoveCamera();

        for (int i = 0; i < countOfSteps; i++)
        {
            var player = players[MovingPlayerIndex];

            Vector3 distanceOfMovement
                = gameLogic.standsTransform[player.currentStandIndex - 1].position
                - gameLogic.standsTransform[player.currentStandIndex].position;

            gameLogic.MoveCamera();

            yield return new WaitForSeconds(0.5f);

            player.gameObject.transform.position += distanceOfMovement;

            player.currentStandIndex--;
        }

        gameLogic.MoveCamera();
    }

    public IEnumerator MakeMove(int countOfSteps)
    {
        gameLogic.cameraPlayer.enabled = true;

        yield return StartCoroutine(MoveForwardPlayerAndCamera(countOfSteps));

        var player = players[MovingPlayerIndex];

        switch (player.currentStandIndex)
        {
            case 4:
            case 13:
            case 19:
            case 24:
                yield return StartCoroutine(MoveBackPlayerAndCamera(3));
                break;

            case 7:
            case 14:
            case 22:
                yield return new WaitForSeconds(0.7f);
                BonusMove();
                yield break;

            case 27:
                yield return new WaitForSeconds(0.7f);
                PlayerFinish();
                yield break;

            default:
                break;
        }

        yield return new WaitForSeconds(0.7f);

        EndMove();
    }

}
