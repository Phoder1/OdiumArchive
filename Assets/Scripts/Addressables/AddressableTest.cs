using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;

namespace WizardParty.AddressablesManagement
{
    public class AddressableTest : MonoBehaviour
    {

        [SerializeField]
        AssetLabelReference[] _labelRefs;
        public List<IResourceLocation> AssetLocation1 { get; set; }
   
        public List<GameObject> objects = new List<GameObject>();

        GameObject _object;
        [Button()]
        public async void LoadObject1()
        {
            
            if (_object == null)
            {
                AssetLocation1 = new List<IResourceLocation>();
              await  AddressableHandler.LoadResourceLocationsAsync(AssetLocation1, _labelRefs[0]);
                _object = await Addressables.LoadAssetAsync<GameObject>(AssetLocation1[0]).Task;
                
            }

          Instantiate(_object);
         
        }

        [Button()]
        public void ReleaseMemory()
        {
            Addressables.Release(AssetLocation1);
        } 
        [Button()]
        public void ReleaseMemoryAndDestroy()
        {
        
            Addressables.ReleaseInstance(_object);
           Addressables.Release(AssetLocation1);
        }

        [Button()]
        public void LoadScene(int scene)
            => Addressables.LoadSceneAsync(scene);


        private void OnDestroy()
        {
            ReleaseMemory();
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
            List<GameObject> objList = new List<GameObject>(references.Count);

            foreach (var reference in references)
                objList.Add(await InstantiateAsync(reference, parent, worldSpace));

            return objList;
        }
        public static async Task<List<T>> InstantiateAsync<T>(this List<AssetReference> references, Transform parent = default, bool worldSpace = default) where T : Component
        {
            List<T> objList = new List<T>(references.Count);

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
