using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DamageIndicator : MonoBehaviour
{
    private Image _image;
    private float _flashSpeed = 0.5f;

    private Coroutine _coroutine;
    private void Start()
    {
        _image = GetComponent<Image>();
        PlayerManager.Instance.Player.condition.OnTakeDamage += Flash;
    }

    private void Flash()
    {
        _image.enabled = true;
        _image.color = new Color(1f, 100 / 255f, 100 / 255f);

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(FadeAway());
    }
    
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float alpha = startAlpha;

        while (alpha > 0)
        {
            alpha -= (startAlpha / _flashSpeed) * Time.deltaTime;
            _image.color = new Color(1f, 100 / 255f, 100 / 255f, alpha);
            yield return null;
        }

        _image.enabled = false;
    }
}
