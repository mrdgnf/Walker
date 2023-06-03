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
    public int availablePlace = 1;

    public int movingPlayerIndex = 0;

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
        gameLogic.playerNameOnScreen.text = players[movingPlayerIndex].name;

        gameLogic.playerNameOnScreen.color = players[movingPlayerIndex].color;

        gameLogic.textOnScreen.color = players[movingPlayerIndex].color;
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
                players[i].place = availablePlace;
            }
        }
    }

    private void IncrementIndex()
    {
        movingPlayerIndex++;
        if (movingPlayerIndex >= players.Count) movingPlayerIndex = 0;
    }

    private void IncrementIndexAfterFinish()
    {
        if (movingPlayerIndex > players.Count - 1)
            movingPlayerIndex--;
    }

    private void PlayerFinish()
    {
        AllocationOfPlayrsPlaces();

        availablePlace++;

        finalists.Add(players[movingPlayerIndex]);

        players.Remove(players[movingPlayerIndex]);

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
        players[movingPlayerIndex].countOfBonuses++;

        Dice.playerMove = false;

        gameLogic.cameraPlayer.enabled = false;
    }

    IEnumerator MovePlayerAndCamera(int countOfSteps, bool forward)
    {
        gameLogic.MoveCamera();

        yield return new WaitForSeconds(0.42f);

        for (int i = 0; i < countOfSteps; i++)
        {
            var player = players[movingPlayerIndex];

            var transform = player.gameObject.transform;

            Vector3 vectorOfNewPosition = GetVectorOfNewPosition(player, forward);

            yield return StartCoroutine(MovePlayer(transform, vectorOfNewPosition));

            gameLogic.MoveCamera();

            UpdatePlayerIndex(player, forward);

            if (player.currentStandIndex >= 27)
            {
                gameLogic.MoveCamera();
                yield break;
            }

        }
    }

    Vector3 GetVectorOfNewPosition(Player player, bool forward)
    {
        if (forward)
        {
            return gameLogic.standsTransform[player.currentStandIndex + 1].position
                - gameLogic.standsTransform[player.currentStandIndex].position;
        }
        else
        {
            return gameLogic.standsTransform[player.currentStandIndex - 1].position
                - gameLogic.standsTransform[player.currentStandIndex].position;
        }
    }

    IEnumerator MovePlayer(Transform transform, Vector3 vectorOfNewPosition)
    {
        Vector3 startMarker = transform.position;

        Vector3 endMarker = transform.position + vectorOfNewPosition;

        float distanceOfMovement = Vector3.Distance(startMarker, endMarker);

        float speed = 1f;
        float distanceCovered = 0f;
        float fractionOfJourney = 0f;

        float a = -1;
        float b = 1;

        float previousValueY = 0;

        while (fractionOfJourney < 1.0f)
        {
            distanceCovered += speed * Time.deltaTime;

            fractionOfJourney = distanceCovered / distanceOfMovement;

            float y = a * Mathf.Pow(fractionOfJourney, 2) + b * fractionOfJourney;

            transform.position -= new Vector3(0, previousValueY, 0);

            transform.position = Vector3.Lerp(startMarker, endMarker, Mathf.SmoothStep(0.0f, 1.0f, fractionOfJourney));

            transform.position += new Vector3(0, y, 0);

            previousValueY = y;

            gameLogic.MoveCamera();

            yield return new WaitForEndOfFrame();
        }

        transform.position -= new Vector3(0, previousValueY, 0);

        gameLogic.MoveCamera();
    }

    void UpdatePlayerIndex(Player player, bool forward)
    {
        if (forward)
        {
            player.currentStandIndex++;
        }
        else
        {
            player.currentStandIndex--;
        }
    }


    public IEnumerator MakeMove(int countOfSteps)
    {
        gameLogic.cameraPlayer.enabled = true;

        players[movingPlayerIndex].countOfMoves++;

        yield return StartCoroutine(MovePlayerAndCamera(countOfSteps, forward: true));

        var player = players[movingPlayerIndex];

        switch (player.currentStandIndex)
        {
            case 4:
            case 13:
            case 19:
            case 24:
                players[movingPlayerIndex].countOfPenalty++;
                yield return StartCoroutine(MovePlayerAndCamera(3,forward: false));
                break;

            case 7:
            case 14:
            case 22:
                yield return new WaitForSeconds(0.7f);
                players[movingPlayerIndex].countOfBonuses++;
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
