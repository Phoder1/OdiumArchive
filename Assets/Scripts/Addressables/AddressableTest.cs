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
        public IList<IResourceLocation> AssetLocation { get; } = new List<IResourceLocation>();
        public List<GameObject> objects = new List<GameObject>();
        [Button()]
        public async void LoadObject1()
        {
            await InstaniateObject(0);
        }
        [Button()]
        public async void LoadObject2()
        {
           await InstaniateObject(1);
        }



        [Button()]
        public void ReleaseMemory()
        {
            foreach (var o in objects)
               Destroy(o);

            objects.Clear();
        }


        [Button()]
        public void LoadScene(int scene)
            => SceneManager.LoadScene(scene);







        private async Task InstaniateObject(int index)
        {
            await AddressableHandler.LoadResourceLocationsAsync(AssetLocation, _labelRefs[index].labelString);


            var task = Addressables.LoadAssetAsync<GameObject>(AssetLocation[0]);
            var obj = await task.Task;
            objects.Add( Instantiate(obj));
        }

        List<GameObject> _gameObjectsList = new List<GameObject>();

        void Start()
        {
           // Initiate();
        }
        public async Task Initiate()
        {
            await AddressableHandler.LoadResourceLocationsAsync(AssetLocation, _labelRefs);

            foreach (var asset in AssetLocation)
            {
                Debug.Log(asset.ToString());
            }
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
