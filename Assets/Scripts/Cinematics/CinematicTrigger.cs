using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using  RPG.Saving;
namespace RPG.Cinematics
{

    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        [SerializeField]bool alreadyTriggered = false;
        private void OnTriggerEnter(Collider other) {

            if (!alreadyTriggered && other.gameObject.tag == "Player")
            {

                print("Play!");
                alreadyTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }

        public object CaptureState()
        {
            return alreadyTriggered;
        }

        public void RestoreState(object state)
        {
            bool triggeredState = (bool)state;
            alreadyTriggered = triggeredState;
        }
    }

}
