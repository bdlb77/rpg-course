using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject player;
        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
        }
        private void OnEnable()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }
        private void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }
        // playable director that is to call DisbleControl
        private void DisableControl(PlayableDirector pd)
        {
            print("Disable Control");
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }


        private void EnableControl(PlayableDirector pd)
        {
            print("Enable Control");
            player.GetComponent<PlayerController>().enabled = true;

        }
    }

}