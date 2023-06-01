using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private readonly System.Random random = new();

    static public bool playerMove = false;

    private int sideNumber = 1;

    public GameObject dice;

    public GameLogic gameLogic;

    public List<Vector3> diceSidesRotation;
    void Start()
    {
        
    }

    private void OnMouseDown()
    {
        if (playerMove) return;

        StartCoroutine(RollDice());     
    }

    private IEnumerator RollDice()
    {
        playerMove = true;

        for (int i = 0; i < 15; i++)
        {
            sideNumber = GetPseudoRandomSideIndex(sideNumber);

            dice.transform.eulerAngles = diceSidesRotation[sideNumber-1];

            yield return new WaitForSeconds(0.05f);        
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(gameLogic.MakeMove(sideNumber));
    }

    private int GetPseudoRandomSideIndex(int currentSideIndex)
    {        
        int newSideIndex;

        do
        {
            newSideIndex = random.Next(1,7);

        } while (currentSideIndex == newSideIndex);

        return newSideIndex;
    }
}
