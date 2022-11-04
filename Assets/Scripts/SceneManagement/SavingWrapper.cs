using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;
namespace RPG.SceneManagement
{

    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        [SerializeField] float fadeInTime = 0.4f;
        

         private void Awake() 
        {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene() 
        {
            // Load Scene BEFORE we try to find fader and also Start CoRoutine in Awake();
            yield return  GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Save()
        {
             GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
            // call to Saving System
             StartCoroutine(GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile));
        }
        
        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }

}