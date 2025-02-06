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

    public States state = States.Wait;
    public Attacks attack;
    Attacks previousAttack;
    float lagTime = 0.0f;
    public float startWait = 2.0f;
    public float leapLag = 1.0f;
    public float landLag = 1.0f;
    public float armbarLag = 1.0f;
    public float berateLag = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        lagTime = startWait;
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

                        lagTime = armbarLag;
                        state= States.Wait;
                    break;

                    case Attacks.Berate:

                        lagTime = berateLag;
                        state = States.Wait;
                    break;
                }

            break;
        }
    }
}
