using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float flashTime = 0.25f;
    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;
    // Start is called before the first frame update
    private Coroutine _damageFlashCoroutine;
    void Start()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        Init();
    }
    private void Init(){
        _materials = new Material[_spriteRenderers.Length];

        for(int i = 0;i<_spriteRenderers.Length;i++){
            _materials[i] = _spriteRenderers[i].material;
        }
    }
    public void CallDamageFlash(){
         _damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }
    private IEnumerator DamageFlasher()
    {
        SetFlashColor();

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;

        while(elapsedTime<flashTime){
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f,0f,elapsedTime/flashTime);
            SetFlashAmount(currentFlashAmount);
            yield return null;
        }
    }
    private void SetFlashAmount(float amount){
        for(int i = 0;i<_materials.Length;i++){
            _materials[i].SetFloat("_FlashAmount",amount);
        }
    }

    private void SetFlashColor(){
        for(int i = 0; i<_materials.Length;i++)
        {
            _materials[i].SetColor("_FlashColor",_flashColor);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
