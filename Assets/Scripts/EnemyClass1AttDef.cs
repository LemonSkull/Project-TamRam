﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class EnemyClass1AttDef : MonoBehaviour
{
    public int EnClass = 1;
    public int EnLVL = 1;
    public int EnHealth = 100;
    

    // DmgCalc = Damage that is done to player or enemy. DmgCalcWaitTime = Time that stat is shown
    int DmgCalc = 0;
    float DmgCalcTime = 0.5f; // chance values if needed

    public bool PlayerDefUP = false;

    public int EnStr;
    public int EnAgi;
    public int EnDex;
    public int EnInt;
    public int EnCon;

    public bool EnemyTurn; // Who is doing attack first -> false = player, true = enemy


    //Player's stats
    public int PlayerClass;
    public int PlayerHealth;

    private int STR, CON, DEX, AGI, INT, LUCK, CHA, WIS;

    int KeepValue; // Holds value for math calculations
    int KeepValue1; // Holds Critical hit values!!!
    int KeepValue2;
    int KeepValue3;

    int KeepEnStr; // Holds enemy stats for status effects
    int KeepEnDex;
    int KeepEnInt;
    int KeepEnAgi;

    int WeakEnStr; // Holds enemy stats in Weakness status effect
    int WeakEnDex;
    int WeakEnInt;
    int WeakEnAgi;
    int WeakEnCon;

    int SlowEnAgi;
    //bool SlowEnAgiTrue = false;

    // Hold basic value when status effects active
    //int NoStatusEffect;
    int NoConfusionEffect;
    int ConfusionEffectCon;

    int STRBuff, STRNerf, DEXBuff, DEXNerf, INTBuff, INTNerf; // Stats buff/nerf when attacking different Class foe

    int EnStrBuff, EnStrNerf, EnDexBuff, EnDexNerf, EnIntBuff, EnIntNerf; // Stats buff/nerf when attacking different Class foe



    // shows player and enemy damages calculated
    public Text DmgDoneTxt;
    public Text DmgTakenTxt;

    public bool DoubleDmg = false;

    //private bool CollisionCheck = false; // testausta

    public GameObject myParentObject;

    public Text EnHealthTxt;
    public Text EnStrTxt;
    public Text EnDexTxt;
    public Text EnIntTxt;
    public Text EnConTxt;
    public Text EnAgiTxt;
    public Text EnLVLTxt;

    // Timing stuff here:
    float TurnStartTime = 0.2f;
    float TurnEndTime = 1f;

    //float EnemyDeathTime = 1f; // +0.5f

    public float SpawnPosX = 0;
    public float SpawnPosY = 0;

    // -> When enemy dies timing -> IEnumerator EnemyDeath()

    //When enemy gets damage -> wait until start next attack
    float DamageToAttack = 1.5f;


    // PLAYER STATUS EFFECTS HERE

    public int StunEffect = 0;
    public int PoisonEffect = 0;
    public int ConfusionEffect = 0;
    public int WeakenEffect = 0;
    public int SlowEffect = 0;
    public int BurnEffect = 0;

    bool Class2AttBuff = false;
    int Class2AttBuffValue;

    void Start()
    {
        PersistentManagerScript.Instance.EnemyHealth = 1;
        PersistentManagerScript.Instance.EnemyMaxHealth = 1;

        GetComponent<SpriteRenderer>().enabled = false; //Makes object invisible
        GetComponent<Animator>().enabled = false;

        GetPlayerStats();


        PersistentManagerScript.Instance.XPScreen = 0;

        ClassBuffNerf();
    }

    void ClassBuffNerf()
    {
        STRBuff = STR + (STR / 4); // Player Strength get 25% buff
        STRNerf = STR - (STR / 4); // Player Strength get 25% nerf

        DEXBuff = DEX + (DEX / 4); // Player Strength get 25% buff
        DEXNerf = DEX - (DEX / 4); // Player Strength get 25% nerf

        INTBuff = INT + (INT / 4); // Player Strength get 25% buff
        INTNerf = INT - (INT / 4); // Player Strength get 25% nerf

        EnStrBuff = EnStr + (EnStr / 4);
        EnStrNerf = EnStr - (EnStr / 4);

        EnDexBuff = EnDex + (EnDex / 4);
        EnDexNerf = EnDex - (EnDex / 4);

        EnIntBuff = EnInt + (EnInt / 4);
        EnIntNerf = EnInt - (EnInt / 4);

    }

    void GetPlayerStats()
    {
        PlayerHealth = PersistentManagerScript.Instance.PlayerHealth;

        PlayerClass = PersistentManagerScript.Instance.PlayerClass;
        STR = PersistentManagerScript.Instance.Str;
        CON = PersistentManagerScript.Instance.Con;
        DEX = PersistentManagerScript.Instance.Dex;
        AGI = PersistentManagerScript.Instance.Agi;
        INT = PersistentManagerScript.Instance.Int;
        LUCK = PersistentManagerScript.Instance.Luck;
        CHA = PersistentManagerScript.Instance.Cha;
        WIS = PersistentManagerScript.Instance.Wis;

        EnLVL = PersistentManagerScript.Instance.Lvl;
        PersistentManagerScript.Instance.EnLvl = EnLVL;
        /*
        KeepValue = 0; 
        KeepValue1 = 0; 
        KeepValue2 = 0;
        KeepValue3 = 0;
        
        KeepEnStr = 0; 
        KeepEnDex = 0;
        KeepEnInt = 0;
        */
    }

    void RestoreStatusEffect()
    {
        StunEffect = 0;
        PoisonEffect = 0;
        ConfusionEffect = 0;
        WeakenEffect = 0;
        SlowEffect = 0;
        BurnEffect = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        

        RestoreStatusEffect();
        GetComponent<SpriteRenderer>().enabled = true; //Makes object visible
        GetComponent<Animator>().enabled = true;

        GetPlayerStats();
        //PersistentManagerScript.Instance.XPScreen = 0;
        transform.SetParent(myParentObject.transform);
        transform.position = new Vector2(SpawnPosX, SpawnPosY);
        //PersistentManagerScript.Instance.FightScreen = true;
        PersistentManagerScript.Instance.FightTransition = true; // START SCREENCHANGE SCRIPT
        DrawEnStatsOnce();
        StartCoroutine(FightStart());
    }

    void DrawEnStatsOnce()
    {
        EnLVL = PersistentManagerScript.Instance.EnLVL;

        EnLVLTxt.text = EnLVL.ToString();
        EnLVLTxt.text = ("Level ") + EnLVLTxt.text + (" (Warrior)");

    }
    void GetPersistentStats()
    {
        EnStr = PersistentManagerScript.Instance.EnStr;
        EnCon = PersistentManagerScript.Instance.EnCon;
        EnDex = PersistentManagerScript.Instance.EnDex;
        EnAgi = PersistentManagerScript.Instance.EnAgi;
        EnInt = PersistentManagerScript.Instance.EnInt;

    }



    void DrawEnStatsUpdate()
    {
        EnLVL = PersistentManagerScript.Instance.EnLVL;
        PersistentManagerScript.Instance.EnClass = EnClass;

        EnLVLTxt.text = EnLVL.ToString();
        EnLVLTxt.text = ("Level ") + EnLVLTxt.text + (" (Warrior)");


        //EnStr = PersistentManagerScript.Instance.EnStr;
        EnStrTxt.text = EnStr.ToString();
        //EnStrTxt.text = "Str      " + EnStrTxt.text;

        //EnCon = PersistentManagerScript.Instance.EnCon;
        EnConTxt.text = EnCon.ToString();
        //EnConTxt.text = "Con     " + EnConTxt.text;

        //EnDex = PersistentManagerScript.Instance.EnDex;
        EnDexTxt.text = EnDex.ToString();
        //EnDexTxt.text = "Dex     " + EnDexTxt.text;

        //EnInt = PersistentManagerScript.Instance.EnInt;
        EnIntTxt.text = EnInt.ToString();
        // EnIntTxt.text = "Int       " + EnIntTxt.text;

        //EnAgi = PersistentManagerScript.Instance.EnAgi;
        EnAgiTxt.text = EnAgi.ToString();
        //EnAgiTxt.text = "Agi      " + EnAgiTxt.text;

        //EnHealth = PersistentManagerScript.Instance.EnemyHealth; // without +1, enemy dies instantly (HP 0)
        EnHealthTxt.text = EnHealth.ToString();
        EnHealthTxt.text = "HP     " + EnHealthTxt.text;

        //EnLVL = PersistentManagerScript.Instance.Lvl;

        //PersistentManagerScript.Instance.EnemyHealth = EnHealth;
        PersistentManagerScript.Instance.EnemyHealth = EnHealth;
    }

    void PlayerDamageDone()
    {
        if (DoubleDmg == true)
        {
            DmgCalc -= EnHealth;
            //var DoubleDmg1 = DmgCalc + DmgCalc;

            DmgDoneTxt.text = DmgCalc.ToString();
            DmgDoneTxt.text = "2x";

            //DmgDoneTxt.text = DmgCalc.ToString();
            //DmgDoneTxt.text = "  -" + DmgDoneTxt.text;

        }
        else
        {
            DmgCalc -= EnHealth;

            DmgDoneTxt.text = DmgCalc.ToString();
            DmgDoneTxt.text = "-" + DmgDoneTxt.text;

        }
    }

    void PlayerDamageTaken()
    {
        DmgCalc -= PlayerHealth;

        DmgTakenTxt.text = DmgCalc.ToString();
        DmgTakenTxt.text = "-" + DmgTakenTxt.text;
    }

/// <summary>
/// -------------------------------------------------------------------------------------------------------------------
/// </summary>

    void Update()
    {


        DrawEnStatsUpdate();

        //BASIC ATTACK
       if (PersistentManagerScript.Instance.PlayerTurn == true && PersistentManagerScript.Instance.BasicAttack == true)
            {

            if (PersistentManagerScript.Instance.BasicAttack == true)
                {
                PersistentManagerScript.Instance.BasicAttack = false;
                //GetPlayerStats();
                //StartCoroutine(PlayerBasicAttack());
                GetPlayerStats();
                if (AGI / 2 >= EnAgi)
                {
                    
                    
                    DoubleDmg = true;
                    PlayerDamageDone();
                    StartCoroutine(PlayerBasicAttack());

                    StartCoroutine(PlayerBasicAttack());

                    DoubleDmg = false;
                }
                else //if (AGI / 2 <= EnAgi)
                {
                    
                    StartCoroutine(PlayerBasicAttack());
                }

                PersistentManagerScript.Instance.PlayerTurn = false;
            }
       }

       //SUPER ATTACK
       if (PersistentManagerScript.Instance.PlayerTurn == true && PersistentManagerScript.Instance.SuperAttack == true)
        {
            if (PersistentManagerScript.Instance.SuperAttack == true)
            {
                PersistentManagerScript.Instance.SuperAttack = false;
                GetPlayerStats();
                StartCoroutine(PlayerSuperAttack());

                PersistentManagerScript.Instance.PlayerTurn = false;
            }

        }

        //ULTRA ATTACK
        if (PersistentManagerScript.Instance.PlayerTurn == true && PersistentManagerScript.Instance.UltraAttack == true)
        {
            if (PersistentManagerScript.Instance.UltraAttack == true)
            {
                PersistentManagerScript.Instance.UltraAttack = false;
                GetPlayerStats();
                StartCoroutine(PlayerUltraAttack());

                PersistentManagerScript.Instance.PlayerTurn = false;
            }

        }

        // DEFENSE
        if (PersistentManagerScript.Instance.PlayerTurn == true && PersistentManagerScript.Instance.BasicDefense == true)
        {

            if (PersistentManagerScript.Instance.BasicDefense == true)
            {
                PersistentManagerScript.Instance.BasicDefense = false;
                GetPlayerStats();
                StartCoroutine(PlayerBasicDefense());

                PersistentManagerScript.Instance.PlayerTurn = false;
            }
        }


        if (PersistentManagerScript.Instance.EnemyTurn == true && PersistentManagerScript.Instance.PlayerTurn == false) // Enemy Automatic attack
        {
            
            StartCoroutine(EnemyBasicAttack());
            PersistentManagerScript.Instance.EnemyTurn = false;
        }



        if (EnHealth <= 0)
        {
            
            StartCoroutine(EnemyDeath());
        }


    }

    IEnumerator FightStart()
    {
        PersistentManagerScript.Instance.HealthBarActive = false;
        
        yield return new WaitForSeconds(5f); //!!
        GetPersistentStats();

        PersistentManagerScript.Instance.HealthBarActive = true;
        EnHealth = PersistentManagerScript.Instance.EnemyMaxHealth;

        if (PersistentManagerScript.Instance.Agi >= EnAgi)
        {
            PersistentManagerScript.Instance.EnemyTurn = false;
            PersistentManagerScript.Instance.PlayerTurn = true;
        }
        if (PersistentManagerScript.Instance.Agi <= EnAgi)
        {
            PersistentManagerScript.Instance.EnemyTurn = true;
            PersistentManagerScript.Instance.PlayerTurn = false;


        }



    }


    IEnumerator EnemyDeath() // When Enemy dies -> Fightscreen ends -> go to world screen
    {

        yield return new WaitForSeconds(DmgCalcTime);
        DmgTakenTxt.text = " ";
        PersistentManagerScript.Instance.XPScreen = 1;

        yield return new WaitForSeconds(0.5f);

        PersistentManagerScript.Instance.EnDies = 1;

        /*
        if (PersistentManagerScript.Instance.EnDies == 0)
            PersistentManagerScript.Instance.EnDies = 1;
            
        else
            PersistentManagerScript.Instance.EnDies = 2;
            */
            /*
        yield return new WaitForSeconds(EnemyDeathTime);

        
        PersistentManagerScript.Instance.XPScreen = 0;
        PersistentManagerScript.Instance.PlayerTurn = false;
        PersistentManagerScript.Instance.FightScreen = false;

        Destroy(gameObject);
        */
    }


    public void CriticalHitClac()
    {
        KeepValue1 = STR;
        KeepValue2 = DEX;
        KeepValue3 = INT;

        if (PersistentManagerScript.Instance.IsCritical == true)
        {
            STR = STR * 2;
            DEX = DEX * 2;
            INT = INT * 2;

        }
        else
        {
            STR = KeepValue1;
            DEX = KeepValue2;
            INT = KeepValue3;
        }

    }
    IEnumerator PlayerBasicDefense()
    {
         // If Defense button is used -> Skip attack buff defense
        
            // Code added in EnemyBasicAttack()
      yield return new WaitForSeconds(TurnEndTime);
        {
            PlayerDefUP = true;

            PersistentManagerScript.Instance.EnemyTurn = true;

        }

    }

    IEnumerator PlayerUltraAttack()
    {
        if (EnHealth >= 0 && PersistentManagerScript.Instance.EnemyTurn == false)
        {
            PersistentManagerScript.Instance.StartRandomCrit = true;


            yield return new WaitForSeconds(TurnStartTime);

            var DEXKeep = DEX;

            if (PlayerClass == 1) // Ultra Attack from Class1 -> ///Cost-20 MP
            {

                CriticalHitClac();
                ClassBuffNerf();
                //ConfusionEffect += 1;
                if (WeakenEffect == 0)
                {
                    WeakenEffect += 5;
                }

                //NoConfusionEffect = STR;
                //STR = 20; // ATTACK   
                PersistentManagerScript.Instance.PlayerMana -= 20;

                if (STR >= EnCon)
                {
                    DmgCalc = EnHealth;
                    EnHealth -= STR - EnCon;

                    PlayerDamageDone();
                    yield return new WaitForSeconds(DmgCalcTime);
                    DmgDoneTxt.text = " ";
                }
                else
                {

                    
                }


                //STR = NoConfusionEffect; // ATTACK RESET VALUE                          

            }

            if (PlayerClass == 2) // 
            {
                if (SlowEffect == 0)
                {
                    SlowEffect += 4;
                    EnAgi -= 5;
                    
                    SlowEnAgi = EnAgi;
                }    
                PersistentManagerScript.Instance.PlayerMana -= 20;
                
                if (Class2AttBuff == true)
                {
                    DEX += 5;
                    Class2AttBuff = false;
                }

                CriticalHitClac();
                ClassBuffNerf();

                

                if (DEXNerf >= EnCon)
                {
                    var DexUltra = 5;


                    DmgCalc = EnHealth;
                    //EnHealth -= DEXNerf - EnCon;
                    EnHealth -= (DexUltra + DEXNerf) - EnCon;
                    PlayerDamageDone();
                    yield return new WaitForSeconds(DmgCalcTime);
                    DmgDoneTxt.text = " ";

                }
                else
                {
                    
                }


            }

            if (PlayerClass == 3) // 
            {
                CriticalHitClac();
                ClassBuffNerf();

                PersistentManagerScript.Instance.PlayerMana -= 10;
                if (BurnEffect == 0)
                {
                    BurnEffect += 4;
                }


                if (INTBuff >= EnCon)
                {
                    DmgCalc = EnHealth;
                    EnHealth -= INTBuff - EnCon;
                    PlayerDamageDone();
                    yield return new WaitForSeconds(DmgCalcTime);
                    DmgDoneTxt.text = " ";

                }
                else
                {
                    
                }


            }

            yield return new WaitForSeconds(TurnEndTime);
            {
                PersistentManagerScript.Instance.EnemyTurn = true;

            }

            DEX = DEXKeep;

        }


    }

    IEnumerator PlayerSuperAttack() ///SUPER ATTACK ///////////////////////////////////////////////////////////////////////////////
    {
        if (EnHealth >= 0 && PersistentManagerScript.Instance.EnemyTurn == false)
        {
            PersistentManagerScript.Instance.StartRandomCrit = true;


            yield return new WaitForSeconds(TurnStartTime);



            

            if (PlayerClass == 1) // Super Attack from Class1 -> makes 15 dmg/ stuns opponent///Cost-15 MP
            {

                CriticalHitClac();
                ClassBuffNerf();
                StunEffect += 1;///Check made in EnemyBasicAttack if true
                NoConfusionEffect = STR;
                STR = 15; // ATTACK
                PersistentManagerScript.Instance.PlayerMana -= 15;

                if (STR >= EnCon)
                {
                    DmgCalc = EnHealth;
                    EnHealth -= STR - EnCon;

                    PlayerDamageDone();
                    yield return new WaitForSeconds(DmgCalcTime);
                    DmgDoneTxt.text = " ";
                }
                else
                {

                    
                }

                
                STR = NoConfusionEffect; // ATTACK RESET VALUE                          

            }

            if (PlayerClass == 2) // Super Attack from Class2 -> makes 10 dmg/ poison damage ///Cost-10 MP
            {
                var DEXKeep = DEX;

                //if (PoisonEffect == 0)                                                
                
                    PersistentManagerScript.Instance.PlayerMana -= 10;
                    PoisonEffect += 2;
                
                if (Class2AttBuff == true)
                {
                    DEX += 5;
                    Class2AttBuff = false;
                }

                CriticalHitClac();
                ClassBuffNerf();


                if (DEXNerf >= EnCon) 
                {

                    DmgCalc = EnHealth;
                    EnHealth -= DEXNerf - EnCon;
                    PlayerDamageDone();
                    yield return new WaitForSeconds(DmgCalcTime);
                    DmgDoneTxt.text = " ";

                }
                else
                {
                    
                }

                DEX = DEXKeep;
            }

            if (PlayerClass == 3) // Super Attack from Class3 -> makes 10dmg/confusion effect //Cost-10MP
            {
                CriticalHitClac();
                ClassBuffNerf();

                PersistentManagerScript.Instance.PlayerMana -= 10;
                ConfusionEffect += 1;

                
                var confusion = EnCon-EnCon; // Enemy Con = 0

                if (INTBuff >= EnCon)
                {
                    DmgCalc = EnHealth;
                    EnHealth -= INTBuff - confusion;
                    PlayerDamageDone();
                    yield return new WaitForSeconds(DmgCalcTime);
                    DmgDoneTxt.text = " ";

                }
                else
                {
                    
                }


            }
            
            yield return new WaitForSeconds(TurnEndTime);
            {
                PersistentManagerScript.Instance.EnemyTurn = true;

            }
        }
    }

    IEnumerator PlayerBasicAttack() // Defense added too
    {



        if (EnHealth >= 0 && PersistentManagerScript.Instance.EnemyTurn == false)
        {
            PersistentManagerScript.Instance.StartRandomCrit = true;


            yield return new WaitForSeconds(TurnStartTime);

            var DEXKeep = DEX;

            if (PlayerClass == 1) // Basic Attack from Class1 to Class1
            {
                CriticalHitClac();
                ClassBuffNerf();


                if (STR >= EnCon)
                {
                    DmgCalc = EnHealth;
                    EnHealth -= STR - EnCon;

                    PlayerDamageDone();
                    yield return new WaitForSeconds(DmgCalcTime);
                    DmgDoneTxt.text = " ";
                }
                else
                {

                    
                }


            }

            if (PlayerClass == 2) // Basic Attack from Class2 to Class1
            {
                if (Class2AttBuff == true)
                {
                    DEX += 5;
                    Class2AttBuff = false;
                }

                CriticalHitClac();
                ClassBuffNerf();



                if (DEXNerf >= EnCon)
                {
                    DmgCalc = EnHealth;
                    EnHealth -= DEXNerf - EnCon;
                    PlayerDamageDone();
                    yield return new WaitForSeconds(DmgCalcTime);
                    DmgDoneTxt.text = " ";

                }
                else
                {
                    
                }


            }

            if (PlayerClass == 3) // Basic Attack from Class3 to Class1
            {
                CriticalHitClac();
                ClassBuffNerf();

                if (INTBuff >= EnCon)
                {
                    DmgCalc = EnHealth;
                    EnHealth -= INTBuff - EnCon;
                    PlayerDamageDone();
                    yield return new WaitForSeconds(DmgCalcTime);
                    DmgDoneTxt.text = " ";

                }
                else
                {
                    
                }


            }
            yield return new WaitForSeconds(TurnEndTime);
            {
                PersistentManagerScript.Instance.EnemyTurn = true;

            }
            DEX = DEXKeep;

        }


    }


    IEnumerator EnemyBasicAttack()
    {
        
        
        //PersistentManagerScript.Instance.BasicAttack = false;
        if (PlayerDefUP == true)
        {
            KeepValue = 10;

            KeepEnStr = EnStr;
            KeepEnDex = EnDex;
            KeepEnInt = EnInt;


            EnStr = (EnStr / 4) * 3; // Defense buff 30% (enemy strength down for next attack)
            EnDex = (EnDex / 4) * 3;
            EnInt = (EnInt / 4) * 3;
        }

        if (StunEffect == 0) // CHECK CLASS1 SUPER ATTACK STATUS EFFECTS
        {
            if (EnHealth >= 0)
            {
                yield return new WaitForSeconds(TurnStartTime);
                ClassBuffNerf();
                
                if (WeakenEffect == 5)
                {
                    WeakEnStr = EnStr;
                    WeakEnDex = EnDex;
                    WeakEnInt = EnInt;
                    WeakEnCon = EnCon;
                    /*
                    EnStr -= 4;
                    EnDex -= 4;
                    EnInt -= 4;
                    */
                    EnStr -= (EnStr / 4);
                    EnDex -= (EnDex / 4);
                    EnInt -= (EnInt / 4);
                    EnCon -= (EnCon / 2);

                    EnStrNerf -= (EnStrNerf / 4);
                    EnStrBuff -= (EnStrBuff / 4);
                    //WeakenEffect -= 1;
                    yield return new WaitForSeconds(DamageToAttack);

                }
                if (PoisonEffect >= 1)
                {
                    PoisonEffect -= 1;
                    EnHealth -= 5;
                    PersistentManagerScript.Instance.PoisonActive = true;
                    yield return new WaitForSeconds(DamageToAttack);
                }
                else
                {
                    PersistentManagerScript.Instance.PoisonActive = false;
                }
                if (ConfusionEffect >= 1)
                {
                        ConfusionEffectCon = EnCon;
                        //NoStatusEffect = EnCon;
                        EnCon -= 5;
                    PersistentManagerScript.Instance.ConfusionActive = true;
                    yield return new WaitForSeconds(DamageToAttack);
                }
                
                if (SlowEffect >= 2)
                {

                    
                    SlowEffect -= 1;
                    PersistentManagerScript.Instance.SlowActive = true;
                    yield return new WaitForSeconds(DamageToAttack);
                }


                if (BurnEffect >= 1)
                {
                    EnHealth -= 5;
                    BurnEffect -= 1;
                    PersistentManagerScript.Instance.BurnActive = true;
                    yield return new WaitForSeconds(DamageToAttack);
                }
                else
                {
                    PersistentManagerScript.Instance.BurnActive = false;
                }

                //yield return new WaitForSeconds(DamageToAttack);
                
                PersistentManagerScript.Instance.EnemyAnimAttack = true;
                yield return new WaitForSeconds(0.3f);

                if (PlayerClass == 1)
                {

                    {
                        if (EnStr >= CON)
                        {
                            DmgCalc = PlayerHealth;
                            PlayerHealth -= EnStr - CON;
                            PersistentManagerScript.Instance.PlayerHealth = PlayerHealth;
                            PlayerDamageTaken();
                            yield return new WaitForSeconds(DmgCalcTime);
                            DmgTakenTxt.text = " ";

                        }
                        if (CON >= EnStr)
                        {
                            DmgCalc = PlayerHealth;
                            PlayerHealth -= 1;
                            PersistentManagerScript.Instance.PlayerHealth = PlayerHealth;
                            PlayerDamageTaken();
                            yield return new WaitForSeconds(DmgCalcTime);
                            DmgTakenTxt.text = " ";
                        }

                    }
                }

                if (PlayerClass == 2)
                {
                    

                    if (EnStrBuff >= CON)
                    {
                        DmgCalc = PlayerHealth;
                        PlayerHealth -= EnStrBuff - CON;
                        Class2AttBuffValue = 0;
                        Class2AttBuffValue += (EnStr - CON) / 2;
                        PersistentManagerScript.Instance.PlayerHealth = PlayerHealth;
                        PlayerDamageTaken();
                        yield return new WaitForSeconds(DmgCalcTime);
                        DmgTakenTxt.text = " ";
                    }
                    if (CON >= EnStrBuff)
                    {
                        DmgCalc = PlayerHealth;
                        PlayerHealth -= 1;
                        PersistentManagerScript.Instance.PlayerHealth = PlayerHealth;
                        PlayerDamageTaken();
                        yield return new WaitForSeconds(DmgCalcTime);
                        DmgTakenTxt.text = " ";
                    }


                }

                if (PlayerClass == 3)
                {


                    if (EnStrNerf >= CON)
                    {
                        DmgCalc = PlayerHealth;
                        PlayerHealth -= EnStrNerf - CON;
                        PersistentManagerScript.Instance.PlayerHealth = PlayerHealth;
                        PlayerDamageTaken();
                        yield return new WaitForSeconds(DmgCalcTime);
                        DmgTakenTxt.text = " ";
                    }
                    if (CON >= EnStrBuff)
                    {
                        DmgCalc = PlayerHealth;
                        PlayerHealth -= 1;
                        PersistentManagerScript.Instance.PlayerHealth = PlayerHealth;
                        PlayerDamageTaken();
                        yield return new WaitForSeconds(DmgCalcTime);
                        DmgTakenTxt.text = " ";
                    }

                }
                ///RETURN STATUS EFFECTS HERE ///////////
                if (ConfusionEffect >= 1)
                {
                    //ConfusionEffectCon = EnCon;
                    //NoStatusEffect = EnCon;
                    //EnCon -= 5;
                    ConfusionEffect -= 1;
                    EnCon = ConfusionEffectCon;
                }
                

                if (WeakenEffect >= 1)
                {
                    WeakenEffect -= 1;
                    PersistentManagerScript.Instance.WeakenActive = true;
                }

                    if (WeakenEffect == 1)
                    {
                    EnStr = WeakEnStr;
                    EnDex = WeakEnDex;
                    EnInt = WeakEnInt;
                    EnCon = WeakEnCon;
                    //EnAgi = WeakEnAgi;
                    WeakenEffect = 0;
                    PersistentManagerScript.Instance.WeakenActive = false;

                    if (WeakenEffect <= 1)
                    {
                        PersistentManagerScript.Instance.WeakenActive = false;
                    }
                }

                if (SlowEffect == 1)
                {
                    EnAgi += 5;
                    SlowEffect = 0;
                    PersistentManagerScript.Instance.SlowActive = false;
                }
                // BurnEffect = No need stuff here
                // StunEffect = else command down below

            }


        }
        else //Return Stun status effect 
        {

            PersistentManagerScript.Instance.StunActive = true;
            StunEffect = 0;

        }


        if (PlayerDefUP == true)
            {
                EnStr = KeepEnStr;
                EnDex = KeepEnDex;
                EnInt = KeepEnInt;

                DmgCalc = EnHealth;

                if (PlayerClass == 1) // Class 3 Get Mana
                {
                    if ((STR / 2) >= EnCon)
                    {

                        EnHealth -= (STR / 2) - EnCon;


                    }

                    PersistentManagerScript.Instance.PlayerHealth = PlayerHealth;
                    DmgDoneTxt.text = " ";
                    PlayerDamageDone();
                    yield return new WaitForSeconds(DmgCalcTime);
                    DmgDoneTxt.text = " ";
                }
                /////////////////////////////////////////////////////////
                if (PlayerClass == 2)
                {
                Class2AttBuff = true; // Buff Player's attack for next turn
                }

            if (PlayerClass == 3) // Player1 Defense skill counter attack 50%/STR
            {
                if ((INT / 2) >= EnCon)
                {

                    PersistentManagerScript.Instance.PlayerMana += (INT / 2) - EnCon;


                }
                /*
                PersistentManagerScript.Instance.PlayerHealth = PlayerHealth;
                DmgDoneTxt.text = "MP Drain";
                //PlayerDamageDone();
                yield return new WaitForSeconds(DmgCalcTime);
                DmgDoneTxt.text = " ";
            */
            }

            PlayerDefUP = false;
            PersistentManagerScript.Instance.BasicDefense = false;
            }



        //TurnEndTime = TurnEndTimeSeconds;
        yield return new WaitForSeconds(TurnEndTime);
        {
            PersistentManagerScript.Instance.PlayerTurn = true;

        }
        
    }



}
