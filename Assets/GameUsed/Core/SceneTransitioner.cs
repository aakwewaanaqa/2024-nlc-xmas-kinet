using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameUsed.Core
{
    public static class SceneTransitioner
    {
        public static async UniTask Load(string sceneName)
        {
            await Program.Wiper.Show(null);
            await SceneManager.LoadSceneAsync(sceneName);
            await Program.Wiper.Hide(null);
        }
    }
}