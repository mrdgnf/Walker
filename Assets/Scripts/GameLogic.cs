using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    private int MovingPlayerIndex = 0;

    private int CountOfPlayers = 0;

    public Camera cameraMain;

    public Camera cameraPlayer;   

    public Text playerNameOnScreen;

    public PlayersLogic playersLogic;

    public List<Transform> standsTransform;
    void Start()
    {
        CountOfPlayers = playersLogic.players.Count;

        SetPlayerName();
    }
    private void SetPlayerName() => playerNameOnScreen.text = playersLogic.players[MovingPlayerIndex].name;

    private void IncrementIndex()
    {
        MovingPlayerIndex++;
        if (MovingPlayerIndex >= CountOfPlayers) MovingPlayerIndex = 0;
    }
    public IEnumerator MakeMove(int countOfSteps)
    {       
        yield return StartCoroutine(MovePlayerAndCamera(countOfSteps));

        EndMove();
    }

    public IEnumerator MovePlayerAndCamera(int countOfSteps)
    {
        cameraPlayer.enabled = true;

        MoveCamera();

        for (int i = 0; i < countOfSteps; i++)
        {           
            var player = playersLogic.players[MovingPlayerIndex];         

            Vector3 distanceOfMovement
                = standsTransform[player.currentStandIndex + 1].position
                - standsTransform[player.currentStandIndex].position;

            MoveCamera();

            yield return new WaitForSeconds(1f);            

            player.gameObject.transform.position += distanceOfMovement;

            player.currentStandIndex++;       
        }

        MoveCamera();

        yield return new WaitForSeconds(1.5f);

        cameraPlayer.enabled = false;
    }

    public void MoveCamera()
    {
        Vector3 cameraPosition = playersLogic.players[MovingPlayerIndex].gameObject.transform.position;

        cameraPosition.y += 1.2f;

        cameraPlayer.transform.position = cameraPosition;
    }

    public void EndMove()
    {
        IncrementIndex();

        SetPlayerName();

        Dice.playerMove = false;
    }
}
