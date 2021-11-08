using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace WizardParty.AddressablesManagement
{
    public class AddressableTest : MonoBehaviour
    {
        [SerializeField]
        private GameObject _capsulePrefab;

        [SerializeField]
        private AssetLabelReference _label;

        [SerializeField]
        private AssetReference _scene;

        IList<IResourceLocation> _locations;
        bool _loadingScene = false;

        GameObject _prefab;
        readonly List<GameObject> _instanceGO = new();
        [Button]
        private async void GetLocation()
        {
            _locations = await _label.LoadLocationsAsync();
        }
        [Button]
        private async void LoadOnceAndInstantiate()
        {
            if (!await TryLoadPrefab())
                return;

            Instantiate(_prefab);
        }

        [Button]
        private async void InstantiatePrefabThroughAddressables()
        {
            if (_locations == null)
                return;

            if (!await TryLoadPrefab())
                return;

            _instanceGO.Add(await _locations[0].InstantiateAsync());
        }
        [Button]
        private async void LoadNextScene()
        {
            if (_loadingScene)
                return;

            _loadingScene = true;

            await (await _scene.LoadLocationsAsync())[0].LoadSceneAsync();
            ClearMemory();
            Debug.Log("Scene loaded!");
        }
        [Button]
        private void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        [Button]
        private void UnloadAssets()
        {
            AddressablesManager.UnloadUnusedAssets();
        }

        [Button]
        private void ClearMemory()
        {
            if (_instanceGO != null)
            {
                while (_instanceGO.Count > 0)
                {
                    _instanceGO[0].ReleaseInstance();

                    _instanceGO.RemoveAt(0);
                }
            }

            if (_locations != null)
            {
                _locations.Release();
                _locations = null;
            }

            if (_prefab != null)
            {
                _prefab.ReleaseInstance();
                _prefab = null;
            }
        }

        private async Task<bool> TryLoadPrefab()
        {
            if (_prefab != null)
                return true;
            if (_locations == null)
                return false;
            foreach (var location in _locations)
                _prefab = await Addressables.LoadAssetAsync<GameObject>(location).Task;


            return _prefab != null;
        }

        private void OnDestroy()
        {
            ClearMemory();
        }
    }




    public static class AddressableHandler
    {
        public static async Task<GameObject> InstantiateAsync(this AssetReference reference, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        => await reference.InstantiateAsync(position, rotation, parent).Task;
        public static async Task<GameObject> InstantiateAsync(this AssetReference reference, Transform parent, bool worldSpace = default)
         => await reference.InstantiateAsync(parent, worldSpace).Task;

        public static async Task<List<GameObject>> InstantiateAsync(this List<AssetReference> references, Transform parent = default, bool worldSpace = default)
        {
            List<GameObject> objList = new(references.Count);

            foreach (var reference in references)
                objList.Add(await InstantiateAsync(reference, parent, worldSpace));

            return objList;
        }
        public static async Task<List<T>> InstantiateAsync<T>(this List<AssetReference> references, Transform parent = default, bool worldSpace = default) where T : Component
        {
            List<T> objList = new(references.Count);

            foreach (var reference in references)
                objList.Add((await reference.InstantiateAsync(parent, worldSpace).Task).GetComponent<T>());

            return objList;
        }
        public static async Task<T> InstantiateAsync<T>(this AssetReference reference, Transform parent = default, bool worldSpace = default) where T : Component
         => (await InstantiateAsync(reference, parent, worldSpace)).GetComponent<T>();








        public async static Task LoadResourceLocationsAsync(IList<IResourceLocation> resources, params AssetLabelReference[] labels)
        => await LoadResourceLocationsAsync(resources, labels.GetLabels());
        public async static Task LoadResourceLocationsAsync(IList<IResourceLocation> resources, params string[] labels)
        {
            if (labels == null)
                return;

            int length = labels.Length;
            for (int i = 0; i < length; i++)
            {
                var unLoadedLocations = await Addressables.LoadResourceLocationsAsync(labels[i]).Task;

                foreach (var location in unLoadedLocations)
                    resources.Add(location);
            }
        }





        // Extenstion Methods
        public static string[] GetLabels(this IList<AssetLabelReference> assetLabelReferences)
        {
            if (assetLabelReferences == null || assetLabelReferences.Count == 0)
                return null;

            var labels = assetLabelReferences.Select(x => x.labelString);
            return labels.ToArray();
        }
    }
}
