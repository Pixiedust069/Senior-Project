// Kenneth Gower
// GSP 497
// 2/20/2015

/* The Finite State Machine. Sets the current state, transitions between states, and executes the current state.*/
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
            setState(newState);
        }
        // Transition from Chase to Scare.
        else if (curState == chase && newState == scare)
        {
            this.GetComponent<Script_Behaviors>().saveOldLights();
            this.GetComponent<Script_Behaviors>().stopTimerDim();
            setState(newState);
        }
        // Transition from Chase to Search. Set searchCount to 0 and run pickRandomPosition() 
        // to pick the first random search point.
        else if (curState == chase && newState == search)
        {
            this.GetComponent<Script_Behaviors>().searchCount = 0;
            this.GetComponent<Script_Behaviors>().pickRandomPosition();            
            setState(newState);
        }
        // Transition from Search to Die.
        else if (curState == search && newState == die)
        {
            this.GetComponent<Script_Behaviors>().searchCount = 0;            
            setState(newState);
        }
        // Transition from Search to Chase.
        else if (curState == search && newState == chase)
        {
            this.GetComponent<Script_Behaviors>().searchCount = 0;            
            setState(newState);
        }
        // Transition from Scare to Search
        else if (curState == scare && newState == search)
        {
            this.GetComponent<Script_Behaviors>().destroyCandy();
            this.GetComponent<Script_Behaviors>().subPoints = false;
            this.GetComponent<Script_Behaviors>().restoreOldLights();
            this.GetComponent<Script_Behaviors>().startTimerDim();
            this.GetComponent<Script_Behaviors>().searchCount = 0;
            this.GetComponent<Script_Behaviors>().pickRandomPosition();            
            setState(newState);
        }
        // Transition from Scare to Chase
        else if (curState == scare && newState == chase)
        {
            this.GetComponent<Script_Behaviors>().destroyCandy();
            this.GetComponent<Script_Behaviors>().subPoints = false;
            this.GetComponent<Script_Behaviors>().restoreOldLights();
            this.GetComponent<Script_Behaviors>().startTimerDim();
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
    void idleState()
    {
        this.GetComponent<Script_Behaviors>().idle();
    }

    void searchState()
    {
        this.GetComponent<Script_Behaviors>().search();
    }

    void chaseState()
    {
        this.GetComponent<Script_Behaviors>().chase();
    }

    void dieState()
    {
        this.GetComponent<Script_Behaviors>().die();
    }

    void scareState()
    {
        this.GetComponent<Script_Behaviors>().scare();
    }
}