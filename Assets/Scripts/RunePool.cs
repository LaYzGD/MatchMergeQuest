using UnityEngine;
using UnityEngine.Pool;

public class RunePool : MonoBehaviour
{
    [SerializeField] private Rune _runePrefab;

    private ObjectPool<Rune> _pool;

    public void Init()
    {
        _pool = new ObjectPool<Rune>(CreateRune, OnGet, OnRelease, OnDestroyRune, false);
    }

    public Rune SpawnRune()
    {
        Rune rune = _pool.Get();
        rune.Init(DestroyRune);
        return rune;
    }

    private void DestroyRune(Rune rune) 
    {
        _pool.Release(rune);
    }

    private Rune CreateRune() => Instantiate(_runePrefab);
    private void OnGet(Rune rune) => rune.gameObject.SetActive(true);
    private void OnRelease(Rune rune) => rune.gameObject.SetActive(false);
    private void OnDestroyRune(Rune rune) => Destroy(rune.gameObject);
}
