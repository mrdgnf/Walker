using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public GameObject GameResults;

    public GameObject ScoreBoard;

    public GameObject Cell;

    public GameObject dice;

    public Camera cameraPlayer;

    public Text textOnScreen;

    public Text playerNameOnScreen;

    public PlayersLogic playersLogic;

    public List<Transform> standsTransform;

    void Start() => playersLogic.SetPlayerName();        

    public void MoveCamera()
    {
        Vector3 cameraPosition = playersLogic.players[playersLogic.movingPlayerIndex].gameObject.transform.position;

        cameraPosition.y += 1.2f;

        cameraPlayer.transform.position = cameraPosition;
    }     

    public void GameFinish()
    {
        cameraPlayer.enabled = false;

        playerNameOnScreen.enabled = false;

        textOnScreen.enabled = false;

        dice.SetActive(false);

        GameResults.SetActive(true);

        foreach (var player in playersLogic.finalists)
        {
            GameObject ñell = Instantiate(this.Cell);

            ñell.transform.SetParent(ScoreBoard.transform);

            ñell.transform.localScale = Vector3.one;

            TableCell tableCell = ñell.GetComponent<TableCell>();

            tableCell.place.text = player.place.ToString();
            tableCell.playerName.text = player.name;
            tableCell.moveCount.text = player.countOfMoves.ToString();
            tableCell.bonusCount.text = player.countOfBonuses.ToString();
            tableCell.penaltyCount.text = player.countOfPenalty.ToString();            
        }

        Dice.playerMove = false;
    }

    public void RestartGame() => SceneManager.LoadScene(0);

}
