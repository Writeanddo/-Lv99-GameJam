using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHitSpriteShader : MonoBehaviour
{
    protected IDamageable damageable;
    [SerializeField] protected SpriteRenderer sRenderer;

    [SerializeField] protected Color hitGlowColor = Color.white;
    [SerializeField] protected Color healGlowColor = Color.white;
    [SerializeField] protected float speed = 4;
    [SerializeField] protected float glow = 2;

    private void Awake()
    {
        damageable = GetComponent<IDamageable>();
    }

    private void OnEnable()
    {
        sRenderer.material.DisableKeyword("HITEFFECT_ON");

        damageable.OnTakeDamage += HitEffect;
        damageable.OnHeal += HealEffect;
    }

    private void OnDisable()
    {
        damageable.OnTakeDamage -= HitEffect;
        damageable.OnHeal -= HealEffect;
    }

    private void HitEffect(Vector3 position)
    {
        StartCoroutine(Hit(hitGlowColor));
    }

    private void HealEffect()
    {
        StartCoroutine(Hit(healGlowColor));
    }

    private IEnumerator Hit(Color activeGlowColor)
    {
        sRenderer.material.SetColor("_HitEffectColor", activeGlowColor);
        sRenderer.material.EnableKeyword("HITEFFECT_ON");

        sRenderer.material.SetFloat("_HitEffectGlow", glow);
        sRenderer.material.SetFloat("_HitEffectBlend", 0);

        while (sRenderer.material.GetFloat("_HitEffectBlend") < 0.9f)
        {
            var amount = Mathf.Lerp(sRenderer.material.GetFloat("_HitEffectBlend"), 1, speed * Time.deltaTime);
            sRenderer.material.SetFloat("_HitEffectBlend", amount);

            yield return new WaitForEndOfFrame();
        }

        sRenderer.material.SetFloat("_HitEffectBlend", 1);

        yield return new WaitForEndOfFrame();

        while (sRenderer.material.GetFloat("_HitEffectBlend") > 0.1f)
        {
            var amount = Mathf.Lerp(sRenderer.material.GetFloat("_HitEffectBlend"), 0, speed * Time.deltaTime);
            sRenderer.material.SetFloat("_HitEffectBlend", amount);

            yield return new WaitForEndOfFrame();
        }

        sRenderer.material.SetFloat("_HitEffectBlend", 0);
    }
}
