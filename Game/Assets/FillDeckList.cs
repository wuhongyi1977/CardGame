﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;

public class FillDeckList : MonoBehaviour
{

    ICardDatabase database;

    void Start()
    {
        database = Repository.GetCardDatabaseInstance();
        FillList();
    }

    private void FillList()
    {
        string[] savedDecks = GetAllSavedDecks();
        foreach (string path in savedDecks)
        {
            if (path.EndsWith(".xml"))
            {
                GameObject go = (GameObject)Resources.Load("MainMenuResources/DeckItem");
                RectTransform prefab = Instantiate((RectTransform)go.transform);

                string[] deckNameDetails = path.Split('\\');
                string deckName = deckNameDetails[deckNameDetails.Length - 1];
                prefab.Find("DeckName").GetComponent<Text>().text = deckName;
                prefab.Find("DeckPath").GetComponent<Text>().text = path;

                prefab.GetComponent<Toggle>().group = transform.GetComponent<ToggleGroup>();

                prefab.SetParent(transform);
                prefab.localScale = new Vector3(1, 1, 1); //neznam zasto sam mjenja pa moram ja vratiti na default
            }
        }

        transform.GetChild(0).GetComponent<Toggle>().isOn = true;
    }

    private string[] GetAllSavedDecks()
    {
        return Directory.GetFiles(Application.dataPath + "/Decks");
    }
}