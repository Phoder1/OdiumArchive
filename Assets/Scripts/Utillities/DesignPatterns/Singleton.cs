using System;
using UnityEngine;

namespace WizardParty
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T> 
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                        instance = Create();
                }
                return instance;
            }
        }
        private static T Create()
        {
            GameObject obj = new()
            {
                name = typeof(T).Name
            };
            var singleton = obj.AddComponent<T>();
            return singleton;
        }
        public virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(transform.root);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
