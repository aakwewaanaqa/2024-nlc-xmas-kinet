using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameUsed.Scenes.Title
{
    [AddComponentMenu("Title/Claw")]
    public class Claw : MonoBehaviour
    {
        [SerializeField] private Transform clawL;
        [SerializeField] private Transform clawR;

        public async UniTask Grab()
        {

        }

        public async UniTask Release()
        {

        }
    }
}