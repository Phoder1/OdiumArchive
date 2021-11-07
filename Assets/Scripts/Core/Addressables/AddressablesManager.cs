using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using static UnityEngine.AddressableAssets.Addressables;

namespace WizardParty.AddressablesManagement
{
    public static class AddressablesManager
    {
        /* All asset reference types:
         * AssetReferenceT<T> where T : UnityEngine.Object
         * AssetLabelReference
         * AssetReferenceAtlasedSprite
         * AssetReferenceGameObject
         * AssetReferenceSprite
         * AssetReferenceTexture
         * AssetReferenceTexture
         * AssetReferenceTexture2D
         * AssetReferenceTexture3D
         */
        public static async Task<IList<IResourceLocation>> LoadLocationsAsync(this IKeyEvaluator keyEvaluator)
        {
            if (keyEvaluator == null)
                return null;

            return await LoadResourceLocationsAsync(keyEvaluator).Task;
        }
        public static async Task<IList<IResourceLocation>> LoadLocationsAsync(this IEnumerable<IKeyEvaluator> keyEvaluator, MergeMode mergeMode = default, Type type = null)
        {
            if (keyEvaluator == null)
                return null;

            return await LoadResourceLocationsAsync(keyEvaluator, mergeMode, type).Task;
        }
        public static async Task<GameObject> InstantiateAsync(this IResourceLocation location, InstantiationParameters instantiationParameters = default, bool trackHandler = true)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            return await Addressables.InstantiateAsync(location, instantiationParameters, trackHandler).Task;
        }
        public static void Release<TObject>(this TObject obj)
        {
            if (obj == null)
                return;

            Addressables.Release(obj);
        }
        public static void Release<TObject>(this AsyncOperationHandle<TObject> handle) => Addressables.Release(handle);
        public static void Release(this AsyncOperationHandle handle) => Addressables.Release(handle);
        public static bool ReleaseInstance(this GameObject instance)
        {
            if (instance == null)
                return false;

            return Addressables.ReleaseInstance(instance);
        }
        public static bool ReleaseInstance(this AsyncOperationHandle handle) => Addressables.ReleaseInstance(handle);
        public static bool ReleaseInstance(this AsyncOperationHandle<GameObject> handle) => Addressables.ReleaseInstance(handle);
        public static async Task<SceneInstance> LoadSceneAsync(this IResourceLocation sceneResourceLocation, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100)
        {
            if (sceneResourceLocation == null)
                throw new ArgumentNullException(nameof(sceneResourceLocation));

            return await Addressables.LoadSceneAsync(sceneResourceLocation, loadMode, activateOnLoad, priority).Task;
        }
        public static async Task<SceneInstance> UnloadSceneAsync(this SceneInstance scene, UnloadSceneOptions unloadOptions)
        {
            if(scene.Scene == null)
                throw new ArgumentNullException(nameof(scene));

            return await Addressables.UnloadSceneAsync(scene, unloadOptions, true).Task;
        }
        public static async Task<SceneInstance> UnloadSceneAsync(this AsyncOperationHandle handle, UnloadSceneOptions unloadOptions)
            => await Addressables.UnloadSceneAsync(handle, unloadOptions, true).Task;
        public static async Task<SceneInstance> UnloadSceneAsync(this AsyncOperationHandle<SceneInstance> handle)
            => await Addressables.UnloadSceneAsync(handle, true).Task;
        public static void UnloadUnusedAssets() => Resources.UnloadUnusedAssets();
    }
}
