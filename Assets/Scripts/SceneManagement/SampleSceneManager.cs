using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WizardParty.SceneManagement
{
    public class SampleSceneManager : MonoBehaviour
    {
        [Button]
        private void LoadScene(string SceneName)
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}
