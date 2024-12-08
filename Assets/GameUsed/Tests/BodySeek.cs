using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BodySeek
{
    private BodySourceManager src { get; set; }

    [SetUp]
    public void SetUp()
    {
        src = new GameObject("BodySrc")
           .AddComponent<BodySourceManager>();
    }

    [UnityTest]
    public IEnumerator SeekHead()
    {
        var bodies = src?.GetData();
        yield return Run().ToCoroutine();
        Assert.That(bodies,        Is.Not.Null);
        Assert.That(bodies.Length, Is.GreaterThan(0));
        yield break;

        async UniTask Run()
        {
            await UniTask.WaitUntil(() => src is not null);
            while (bodies is null || bodies.Length == 0)
            {
                Debug.Log("Refreshed...");
                await UniTask.Delay(500);
                bodies = src.GetData();
            }
        }
    }
}