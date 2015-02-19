// Kenneth Gower
// GSP 497
// 2/5/2015
using UnityEngine;
using System.Collections;

public class Script_FSM : MonoBehaviour
{
    // States //
    char idle = 'a';
    char search = 'b';
    char chase = 'c';
    char die = 'd';
    char scare = 'e';
    
    public char curState; // The current state.
    //********//

    public void setState(char newState)
    {
        curState = newState;        
    }

    public char getState()
    {
        return curState;
    }

    // Transition between each state.
    public void stateTransition(char newState)
    {
        // Transition from Idle to Chase.
        if (curState == idle && newState == chase)
        {
            Debug.Log("Leaving Idle entering Chase");
            setState(newState);
        }
        // Transition from Chase to Scare.
        else if (curState == chase && newState == scare)
        {
            Debug.Log("Leaving Chase entering Scare");
            this.GetComponent<Script_Behaviors>().saveOldLights();
            setState(newState);
        }
        // Transition from Chase to Search. Set searchCount to 0 and run pickRandomPosition() 
        // to pick the first random search point.
        else if (curState == chase && newState == search)
        {
            Debug.Log("Leaving Chase entering Search");
            this.GetComponent<Script_Behaviors>().searchCount = 0;
            this.GetComponent<Script_Behaviors>().pickRandomPosition();
            setState(newState);
        }
        // Transition from Search to Die.
        else if (curState == search && newState == die)
        {
            Debug.Log("Leaving Search entering Die");
            setState(newState);
        }
        // Transition from Search to Chase.
        else if (curState == search && newState == chase)
        {
            Debug.Log("Leaving Search entering Chase");
            setState(newState);
        }
        // Transition from Scare to Search
        else if (curState == scare && newState == search)
        {
            Debug.Log("Leaving Scare entering Search");
            this.GetComponent<Script_Behaviors>().restoreOldLights();
            setState(newState);
        }
    }

    // Decide which state we should be in and then execute it.
    public void executeState()
    {
        switch (curState)
        {
            case 'a':
                idleState();
                break;
            case 'b':
                searchState();
                break;
            case 'c':
                chaseState();
                break;
            case 'd':
                dieState();
                break;
            case 'e':
                scareState();
                break;
        }
    }

    // Execute the states.
    public void idleState()
    {
        //Debug.Log("Now in Idle");
        this.GetComponent<Script_Behaviors>().idle();
    }

    public void searchState()
    {
        //Debug.Log("Now in Search");
        this.GetComponent<Script_Behaviors>().search();
    }

    public void chaseState()
    {
        //Debug.Log("Now in Chase");
        this.GetComponent<Script_Behaviors>().chase();
    }

    public void dieState()
    {
        this.GetComponent<Script_Behaviors>().die();
    }

    public void scareState()
    {
        this.GetComponent<Script_Behaviors>().scare();
    }
}