using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneLogic : MonoBehaviour
{
    [System.Serializable]
    public class PlayerSetting
    {
        public InputField name;
        public Dropdown color;
    }

    public Color activeStartButtonColor;  
    public Color noactiveStartButtonColor;

    public Color blockColor;
    private ColorBlock blockColorBlock;
    private ColorBlock normalColorBlock;

    public Button startButton;
    public Text startButtonText;

    public List<PlayerSetting> playerSettings;

    public static List<(string,string)> finallyPlayerSettings = new ();
    void Start() 
    {      
        normalColorBlock = playerSettings[0].name.colors;

        blockColorBlock = normalColorBlock;

        blockColorBlock.normalColor = blockColorBlock.selectedColor = blockColorBlock.pressedColor = blockColorBlock.highlightedColor =
            blockColor;
    }

    public void NamesUpdate()
    {

        int count = 0;

        var playersWithDuplicates = new HashSet<PlayerSetting>();

        RecolorToNormal();

        //Count the number of players with a name and add duplicate names to the list
        for (int i = 0; i < playerSettings.Count; i++)
        {        
            if (playerSettings[i].name.text != "")
            {
                count++;

                for (int j = 0; j < playerSettings.Count; j++)
                {
                    if (i != j && playerSettings[i].name.text == playerSettings[j].name.text)
                    {
                        playersWithDuplicates.Add(playerSettings[i]);
                        playersWithDuplicates.Add(playerSettings[j]);
                    }
                }
            }
        }     

        if (playersWithDuplicates.Count > 0 || count < 2)
        {
            RecolorToBlock(playersWithDuplicates);
            DeactivateStartButton();
        }             
        else
            ActivateStartButton();

    }

    private void RecolorToNormal()
    {
        foreach (var player in playerSettings)       
            player.name.colors = normalColorBlock;      
    }
    private void RecolorToBlock(HashSet<PlayerSetting> playersWithDuplicates)
    {
        foreach (var player in playersWithDuplicates)        
            player.name.colors = blockColorBlock;
    }
    private void ActivateStartButton()
    {
        startButton.interactable = true;
        startButtonText.color = activeStartButtonColor;
    }
    private void DeactivateStartButton()
    {
        startButton.interactable = false;
        startButtonText.color = noactiveStartButtonColor;
    }
    public void GameStart()
    {
        foreach (var player in playerSettings)
        {
            if (player.name.text == "")
                continue;
            finallyPlayerSettings.Add((player.name.text,player.color.captionText.text));
        }
        SceneManager.LoadScene(1);
    }
}
