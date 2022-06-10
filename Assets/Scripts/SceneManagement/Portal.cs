using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using RPG.Control;

namespace RPG.SceneManagement
{

    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D
        }
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 0.5f;
        [SerializeField] float fadeWaitTime = 0.3f;
        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Your Scene To Load not set.");
                yield break;
            }
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            // Remove Control
            playerController.enabled = false;

            // fade out in series of frames.. Load Scene.. Then wait secs for series of Frames.. Then Fade in over Series of Frames 
            yield return fader.FadeOut(fadeOutTime);

            // save current Level
            wrapper.Save();
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            // Remove Control of new scene.
            PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;


            // Load current Level
            wrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);  
            // Save Again To Update and Save the Position of the Player before FadeIn
            
            wrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);

            // Restore Control

            newPlayerController.enabled = true;
            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;
                return portal;
            }

            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {

            GameObject player = GameObject.FindWithTag("Player");
            // without Warp, you can Disable and enable NavMeshAgent when setting the transform position of player
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            
        }
    }
}