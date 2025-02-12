using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandlordAI : MonoBehaviour
{
    public enum States
    {
        Wait,
        Attack,
    }

    public enum Attacks
    {
        LandCall,
        Leap,
        Armbar,
        Berate,
        NumAttacks
    }

    GameObject player;
    public States state = States.Wait;
    public Attacks attack;
    Attacks previousAttack;
    float lagTime = 0.0f;
    public float startWait = 2.0f;
    public float leapLag = 1.0f;
    public float landLag = 1.0f;
    public float armbarLag = 1.0f;
    public float berateLag = 1.0f;
    public GameObject[] berateBullets;
    public float berateFireRate = 1f;
    float berateFireTimer = 0f;
    public float berateYOffset = 1.0f;
    public int numBerateAttacks = 3;
    int berateAttacksMade;
    public float armbarStartup = 1.0f;
    float armbarStartTimer;
    public float armbarDuration = 2f;
    float armbarTimer;
    public float armbarSpeed;
    bool armbarStarted = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lagTime = startWait;
        armbarStartTimer = armbarStartup;
        berateFireTimer = berateFireRate;
        armbarTimer = armbarDuration;
    }

    // Update is called once per frame
    void Update()
    {
        //Chooses what to do based on the current state.
        switch(state)
        {
            case States.Wait:
                lagTime -= Time.deltaTime;
                if(lagTime <= 0.0f)
                {
                    state = States.Attack;
                    previousAttack = attack;

                    do
                    {
                        attack = (Attacks)Random.Range(0, (int)Attacks.NumAttacks);
                    }while(previousAttack == Attacks.LandCall && attack == Attacks.LandCall);
                }
            break;

            case States.Attack:

                switch(attack)
                {
                    case Attacks.LandCall:

                        lagTime = landLag;
                        state = States.Wait;
                    break;

                    case Attacks.Leap:

                        lagTime = leapLag;
                        state = States.Wait;
                    break;

                    case Attacks.Armbar:

                        armbarStartTimer -= Time.deltaTime;
                        if (armbarStartTimer <= 0)
                        {
                            if (!armbarStarted)
                            {
                                if (player.transform.position.x > gameObject.transform.position.x)
                                {
                                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.right * armbarSpeed;
                                }
                                else
                                {
                                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.left * armbarSpeed;
                                }
                                armbarStarted = true;
                            }

                            armbarTimer -= Time.deltaTime;

                            if (armbarTimer <= 0.0f)
                            {
                                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                                lagTime = armbarLag;
                                state = States.Wait;
                                armbarStarted = false;
                                armbarTimer = armbarDuration;
                                armbarStartTimer = armbarStartup;
                            }
                        }
                    break;

                    case Attacks.Berate:

                        berateFireTimer -= Time.deltaTime;

                        if(berateFireTimer <= 0.0f)
                        {
                            GameObject BerateBullet = Instantiate(berateBullets[Random.Range(0, berateBullets.Length)]);
                            BerateBullet.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + berateYOffset);
                            berateFireTimer = berateFireRate;
                            berateAttacksMade++;
                        }

                        if (berateAttacksMade >= numBerateAttacks)
                        {
                            lagTime = berateLag;
                            berateAttacksMade = 0;
                            state = States.Wait;
                        }
                    break;
                }

            break;
        }
    }
}
