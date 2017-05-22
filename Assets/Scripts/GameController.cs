using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public bool updateStatsContinuously;
    public Unit player;
    public Unit[] enemies;

    [SerializeField] private TextMesh playerHUD;
    [SerializeField] private TextMesh[] enemyHUDs;
    private GeneticAlgorithm geneticAlgorithm;

    private void Start() {
        geneticAlgorithm = new GeneticAlgorithm(this);
        Init();
        SetHUD();
    }

    private void Update() {
        bool updateStats = false;
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            player.Health++;
            updateStats = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            player.Attack++;
            updateStats = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            player.Defense++;
            updateStats = true;
        }
        if (updateStatsContinuously || updateStats) {
            geneticAlgorithm.RunGeneticAlgorithm();
            SetHUD();
        }
    }

    private void Init() {
        player.SetAttributes(50, 45, 30);
        enemies[0].SetAttributes(40, 50, 20);
        enemies[1].SetAttributes(35, 40, 15);
        enemies[2].SetAttributes(55, 25, 25);
        enemies[3].SetAttributes(20, 30, 35);
    }

    private void SetHUD() {
        playerHUD.text = "Health: " + (int)player.Health + "\nAttack: " + (int)player.Attack + "\nDefense: " + (int)player.Defense;
        for (int i=0; i<enemies.Length; i++) {
            enemyHUDs[i].text = "Health: " + (int)enemies[i].Health + "\nAttack: " + (int)enemies[i].Attack + "\nDefense: " + (int)enemies[i].Defense;
        }
    }
}