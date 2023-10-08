using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.XR;
using static GuardFSM;
using static UnityEditor.VersionControl.Asset;

public class StateMachine : MonoBehaviour
{

    public class State
    {
        public StateMachine.State patrol, chase, attack, runAway;
        public string Name;
        public System.Action onFrame; // DEFUALT
        public System.Action onEnter; // State is Entered
        public System.Action onExit; // State is Exited

        public Transform[] waypoints;
        public GameObject enemy;
        public float closeEnoughCutoff = 2;
        public float closeEnoughSenseCutoff = 50;
        public float GuardFOV = 89; //degrees
        private float cosGuardFOVover2InRad;
        public float strength = 90;
        public float speed = 2;
        public int nextWaypointsIndex = 0;
        public float closeEnoughAttackCutoff = 2;


        public override string ToString()
        {
            return Name;
        }
    }

    public void start()
    {

    }


    Dictionary<string, State> states = new Dictionary<string, State>();
    public State currentState { get; private set; }

    public State initialState;

    public State CreateState(string name)
    {
        State state = new State();
        state.Name = name;
        if (states.Count == 0)
        {
            initialState = state;
        }
        states[name] = state;
        return state;

    }

    void PatrolOnFrame()
    {
    
    }


    void ChaseOnFrame()
    {

    }

    void AttackOnFrame()
    {

    }


    public void Update()
    {
        //No states yet
        if (states.Count == 0)
        {
            Debug.LogError("*** State machine has no states!");
            return;
        }

        //no current state yet
        if (currentState == null)
        {
            ChangeState(initialState);
        }

        if (currentState.onFrame != null)
        {
            currentState.onFrame();
        }


    }
    public void ChangeState(State newState)
    {
        //
        if (newState == null)
        {
            Debug.LogError("*** Can't change to a null state!");
            return;
        }
        //do onExit of current state
        if (currentState != null && currentState.onExit != null)
        {
            currentState.onExit();
        }


        //change to newState
        Debug.LogFormat("Changing from statet {0} to state {1}", currentState, newState);
        currentState = newState;



        // do onEnter to the newState
        if (currentState.onEnter != null)
        {
            currentState.onEnter();
        }
    }

    public void ChangeState(string newStateName)
    {
        if (states.ContainsKey(newStateName))
        {
            ChangeState(states[newStateName]);
        }
        else
        {
            Debug.LogErrorFormat("State machine doesn't have the state {0}", newStateName);
            return;
        }

    }
}