using UnityEngine;
using UnityEngine.Pool;

public class VFXPool : MonoBehaviour
{
    [SerializeField] private VFXObject _vfxPrefab;

    private ObjectPool<VFXObject> _vfxPool;

    private void Awake()
    {
        _vfxPool = new ObjectPool<VFXObject>(CreateVFX, OnGet, OnRelease, OnDestroyVFX, false);
    }

    public void SpawnVFX(Vector2 position)
    {
        var vfx = _vfxPool.Get();
        vfx.SetCallbackAction(OnVFXStop);
        vfx.transform.position = position;
        vfx.Play();
    }

    private void OnVFXStop(VFXObject vfx)
    {
        _vfxPool.Release(vfx);
    }

    private VFXObject CreateVFX() => Instantiate(_vfxPrefab);
    private void OnGet(VFXObject vfx) => vfx.gameObject.SetActive(true);
    private void OnRelease(VFXObject vfx) => vfx.gameObject.SetActive(false);
    private void OnDestroyVFX(VFXObject vfx) => Destroy(vfx.gameObject);
}
