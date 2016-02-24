﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

    private RectTransform infoPanel;
    private RectTransform notificationPanel;
    private RectTransform messages;
    private Dictionary<char, string> playerCharName;    //char = a/b; string = ime a/b

	// Use this for initialization
    void Awake()
    {
        transform.GetComponent<TurnsManager>().OnEndTurn += GUIManager_OnEndTurn;
        transform.GetComponent<TurnsManager>().OnPlayerLoseHealth += GUIManager_OnPlayerLoseHealth;
        transform.GetComponent<TurnsManager>().OnNotification += GUIManager_OnNotification;
        infoPanel = GameObject.Find("Canvas/InfoPanel").GetComponent<RectTransform>();
        notificationPanel = GameObject.Find("Canvas/NotificationPanel").GetComponent<RectTransform>();
        messages = notificationPanel.Find("MessagePanel/Messages").GetComponent<RectTransform>();

        string aName = GameObject.Find("Canvas/A_PlayerSide").GetComponent<BasePlayer>().playerName;
        string bName = GameObject.Find("Canvas/B_PlayerSide").GetComponent<BasePlayer>().playerName;

        notificationPanel.Find("A_PlayerName/NameText").GetComponentInChildren<Text>().text = aName;
        notificationPanel.Find("B_PlayerName/NameText").GetComponentInChildren<Text>().text = bName;

        playerCharName = new Dictionary<char, string>(2);

        playerCharName.Add('a', aName);
        playerCharName.Add('b', bName);

	}

    void GUIManager_OnNotification(char player, string message)
    {
        string msgToWrite = playerCharName[player] + ": " + message + "\n\r";

        GameObject newGO = (GameObject)Resources.Load("Notification");
        RectTransform notification = (RectTransform)GameObject.Instantiate(newGO.transform);
        notification.SetParent(messages);

        Text messageText = notification.GetComponentInChildren<Text>();
        messageText.text = msgToWrite;

        switch (player)
        {
            case 'a':
                messageText.color = Color.blue;
                break;
            case 'b':
                messageText.color = Color.red;
                break;
            default:
                break;
        }
        messageText.alignment = TextAnchor.UpperCenter;
        notification.transform.localScale = new Vector3(1, 1, 1);
    }

    void GUIManager_OnPlayerLoseHealth(PlayerLoseHealthEventArgs args)
    {
        switch (args.PlayerPosition)
        {
            case 'a':
                infoPanel.Find("A_PlayerHealth").GetComponentInChildren<Text>().text = args.CurrentHealth.ToString();
                break;
            case 'b':
                infoPanel.Find("B_PlayerHealth").GetComponentInChildren<Text>().text = args.CurrentHealth.ToString();
                break;
        }
    }

    void GUIManager_OnEndTurn(EndTurnEventArgs args)
    {
       infoPanel.Find("CurrentTurnImage/CurrentTurnText").GetComponentInChildren<Text>().text = args.TurnNumber.ToString();
       infoPanel.Find("PlayerPlayingText").GetComponentInChildren<Text>().text = ">" + args.NextPlayer;
    }
}