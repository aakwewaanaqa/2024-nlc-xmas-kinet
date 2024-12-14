using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameUsed.Core
{
    public static class SceneTransitioner
    {
        public static async UniTask Load(string sceneName)
        {
            await Program.WiperView.Show(null);
            await SceneManager.LoadSceneAsync(sceneName);
            await Program.WiperView.Hide(null);
        }
    }
}