﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class FightControl : MonoBehaviour
{
    
    bool FightWait = false;
    
    float FightWaitTime = 2f;
    float FightTimer;

    public Text Attack1Txt;
    public Text Attack2Txt;
    public Text Attack3Txt;
    public Text DefenseTxt;

    public GameObject EnemyHealthBar;
    public GameObject EnemyHealthText;

    public GameObject PlayerStats;
    public GameObject EnemyStats;
    public GameObject RunButton;


    public void Update()
    {

        if (PersistentManagerScript.Instance.FightScreen == true)
        {

            if (PersistentManagerScript.Instance.PlayerClass == 1)
            {

                Attack1Txt.text = "Cleaving Strike";
                Attack2Txt.text = "Crushing Blow";
                Attack3Txt.text = "Total Brawl";
                DefenseTxt.text = "Steel Up";
            }

            if (PersistentManagerScript.Instance.PlayerClass == 2)
            {

                Attack1Txt.text = "Sneaky Attack";
                Attack2Txt.text = "Poison Attack";
                Attack3Txt.text = "Swarm of Caltrops";
                DefenseTxt.text = "Cunning Counter";
            }

            if (PersistentManagerScript.Instance.PlayerClass == 3)
            {

                Attack1Txt.text = "Magic Missile";
                Attack2Txt.text = "Brainstorm";
                Attack3Txt.text = "Fireball";
                DefenseTxt.text = "Arcane Protection";
            }
            
        }

        if (PersistentManagerScript.Instance.HealthBarActive == true)
        {
            EnemyHealthBar.SetActive(true);
            EnemyHealthText.SetActive(true);

            PlayerStats.SetActive(true);
            EnemyStats.SetActive(true);
            RunButton.SetActive(true);
        }
        else
        {
            EnemyHealthBar.SetActive(false);
            EnemyHealthText.SetActive(false);

            PlayerStats.SetActive(false);
            EnemyStats.SetActive(false);
            RunButton.SetActive(false);
        }

}



    public void MakeBasicAttack()
    {
        if (FightWait == false && PersistentManagerScript.Instance.PlayerTurn == true)
        {
            PersistentManagerScript.Instance.PlayerTurn = true;
            PersistentManagerScript.Instance.BasicAttack = true;
            PersistentManagerScript.Instance.BasicAnimAttack = true;

            FightWait = true;

            StartCoroutine(FightWaitButtonPress());
        }

    }

    public void MakeSuperAttack() //NEW
    {
        if (FightWait == false && PersistentManagerScript.Instance.PlayerTurn == true)
        {
            if (PersistentManagerScript.Instance.PlayerClass == 1 && PersistentManagerScript.Instance.PlayerMana >= 15)
            {
                PersistentManagerScript.Instance.BasicAnimJumphit = true;
                PersistentManagerScript.Instance.PlayerTurn = true;
                PersistentManagerScript.Instance.SuperAttack = true;

                FightWait = true;

                StartCoroutine(FightWaitButtonPress());
            }

            if (PersistentManagerScript.Instance.PlayerClass == 2 && PersistentManagerScript.Instance.PlayerMana >= 10)
            {
                PersistentManagerScript.Instance.PlayerTurn = true;
                PersistentManagerScript.Instance.SuperAttack = true;

                FightWait = true;

                StartCoroutine(FightWaitButtonPress());
            }

            if (PersistentManagerScript.Instance.PlayerClass == 3 && PersistentManagerScript.Instance.PlayerMana >= 10)
            {
                PersistentManagerScript.Instance.PlayerTurn = true;
                PersistentManagerScript.Instance.SuperAttack = true;

                FightWait = true;

                StartCoroutine(FightWaitButtonPress());
            }
        }

    }

    public void MakeUltrattack() //NEW
    {
        if (FightWait == false && PersistentManagerScript.Instance.PlayerTurn == true)
        {
            if (PersistentManagerScript.Instance.PlayerClass == 1 && PersistentManagerScript.Instance.PlayerMana >= 20)
            {
                PersistentManagerScript.Instance.PlayerTurn = true;
                PersistentManagerScript.Instance.UltraAttack = true;
                PersistentManagerScript.Instance.BasicAnimTackle = true;

                FightWait = true;

                StartCoroutine(FightWaitButtonPress());
            }

            if (PersistentManagerScript.Instance.PlayerClass == 2 && PersistentManagerScript.Instance.PlayerMana >= 20)
            {
                PersistentManagerScript.Instance.PlayerTurn = true;
                PersistentManagerScript.Instance.UltraAttack = true;

                FightWait = true;

                StartCoroutine(FightWaitButtonPress());
            }

            if (PersistentManagerScript.Instance.PlayerClass == 3 && PersistentManagerScript.Instance.PlayerMana >= 20)
            {
                PersistentManagerScript.Instance.PlayerTurn = true;
                PersistentManagerScript.Instance.UltraAttack = true;

                FightWait = true;

                StartCoroutine(FightWaitButtonPress());
            }
        }

    }


    public void MakeBasicDefense()
    {
        /*
        if (PersistentManagerScript.Instance.BasicDefense == false)
        {
            PersistentManagerScript.Instance.PlayerTurn = false;
            PersistentManagerScript.Instance.BasicDefense = true;
        }
        */

        if (FightWait == false && PersistentManagerScript.Instance.PlayerTurn == true)
        {
            PersistentManagerScript.Instance.PlayerTurn = true;
            PersistentManagerScript.Instance.BasicDefense = true;
            PersistentManagerScript.Instance.DefenseActiveColor = true;

            FightWait = true;

            
            StartCoroutine(FightWaitButtonPress());
        }

    }
    
    IEnumerator FightWaitButtonPress()
    {
        FightTimer = FightWaitTime;
        yield return new WaitForSeconds(FightTimer);
        FightWait = false;
    }
    
}
