﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (CharacterStats))]
public class HealthUI : MonoBehaviour {
    public GameObject uiPrefab;
    public Transform target;
    float visibleTime = 5f;
    float lastMadeVisibleTime;

    Transform ui;
    Image healthSlider;
    Transform cam;

    void Start () {
        cam = Camera.main.transform;

        // find a world space canvas
        // TODO just keep a reference??
        foreach (Canvas c in FindObjectsOfType<Canvas> ()) {
            if (c.renderMode == RenderMode.WorldSpace) {
                ui = Instantiate (uiPrefab, c.transform).transform; // parent to canvas
                healthSlider = ui.GetChild (0).GetComponent<Image> ();
                ui.gameObject.SetActive (false);
                break;
            }
        }

        GetComponent<CharacterStats> ().OnHealthChanged += OnHealthChanged;

    }

    void OnHealthChanged (int maxHealth, int currentHealth) {
        if (ui == null) {
            return;
        }

        ui.gameObject.SetActive (true);
        lastMadeVisibleTime = Time.time;

        float healthPct = (float) currentHealth / maxHealth;
        healthSlider.fillAmount = healthPct;

        if (currentHealth <= 0) {
            Destroy (ui.gameObject);
        }
    }

    void LateUpdate () {
        if (ui == null) {
            return;
        }

        ui.position = target.position;
        ui.forward = -cam.forward;

        if (Time.time - lastMadeVisibleTime > visibleTime) {
            ui.gameObject.SetActive (false);
        }
    }

}