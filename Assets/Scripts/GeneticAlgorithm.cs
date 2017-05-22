using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm {

    private GameController controller;
    public GeneticAlgorithm(GameController controller) {
        this.controller = controller;
    }

    private int populations;
    private float[] fitness;
    private float[] probabilities;
    private float[] cummulativeProbabilites;
    private float crossOverRate;
    private float mutationRate;

    public void RunGeneticAlgorithm() {
        populations = controller.enemies.Length;
        fitness = new float[populations];
        probabilities = new float[populations];
        cummulativeProbabilites = new float[populations];
        crossOverRate = 0.5f;
        mutationRate = 0.25f;

        CalculateFitnesses();
        CalculateProbabilities();
        CalculateCummulativeProbabilities();
        RouleteWheelSelection();
        CrossOver();
        Mutation();
    }

    private float ObjectiveFunction(Unit enemy) {
        float a = (controller.player.Health - enemy.Health) / controller.player.Health;
        if (a < 0) a = 0;
        float b = (controller.player.Attack - enemy.Attack) / controller.player.Attack;
        if (b < 0) b = 0;
        float c = (controller.player.Defense - enemy.Defense) / controller.player.Defense;
        if (c < 0) c = 0;
        return (a + b + c) / 3;
    }

    private void CalculateFitnesses() {
        for (int i = 0; i < fitness.Length; i++) {
            fitness[i] = 1 / (ObjectiveFunction(controller.enemies[i]) + 1);
            Debug.Log("Fitness-" + i + ": " + fitness[i]);
        }
    }

    private void CalculateProbabilities() {
        float totalFitness = 0;
        for (int i = 0; i < fitness.Length; i++) {
            totalFitness += fitness[i];
        }
        for (int i = 0; i < probabilities.Length; i++) {
            probabilities[i] = fitness[i] / totalFitness;
            Debug.Log("Probability-" + i + ": " + probabilities[i]);
        }
    }

    private void CalculateCummulativeProbabilities() {
        for (int i=0; i<cummulativeProbabilites.Length; i++) {
            cummulativeProbabilites[i] = 0;
            for (int j=0; j<=i; j++) {
                cummulativeProbabilites[i] += probabilities[j];
            }
            Debug.Log("Cummulative-" + i + ": " + cummulativeProbabilites[i]);
        }
    }

    private void RouleteWheelSelection() {
        Attributes[] tempAttributes = new Attributes[populations];
        for (int i=0; i < tempAttributes.Length; i++) {
            tempAttributes[i] = controller.enemies[i].GetAttributes();
        }

        float[] r = new float[populations];
        for (int i=0; i < r.Length; i++) {
            r[i] = Random.Range(0f, 1f);
        }
        for (int i=0; i < r.Length; i++) {
            for (int j=0; j < cummulativeProbabilites.Length; j++) {
                if (j == 0) {
                    if (r[i] < cummulativeProbabilites[j]) {
                        controller.enemies[i].SetAttributes(tempAttributes[j]);
                        Debug.Log("Set enemy-" + i + " from " + j + ", r: " + r[i]);
                    }
                }
                else {
                    if (cummulativeProbabilites[j-1] < r[i] && r[i] < cummulativeProbabilites[j]) {
                        controller.enemies[i].SetAttributes(tempAttributes[j]);
                        Debug.Log("Set enemy-" + i + " from " + j + ", r: " + r[i]);
                    }
                }
            }
        }
    }

    private void CrossOver() {
        float[] r = new float[populations];
        for (int i = 0; i < r.Length; i++) {
            r[i] = Random.Range(0f, 1f);
        }

        int maxParents = populations / 2;
        List<Unit> parents = new List<Unit>();
        for (int i=0; i < populations; i++) {
            if (r[i] < crossOverRate) {
                parents.Add(controller.enemies[i]);
                Debug.Log("Parent at " + i);
                if (parents.Count >= maxParents) {
                    break;
                }
            }
        }

        bool[] parentTaken = new bool[parents.Count];
        List<Attributes> matches = new List<Attributes>();
        for (int i=0; i < parents.Count; i++) {
            int takenIndex = 0;
            do {
                takenIndex = Random.Range(0, parents.Count);
            } while (parentTaken[takenIndex]);
            parentTaken[takenIndex] = true;
            matches.Add(parents[takenIndex].GetAttributes());
            Debug.Log("Match at " + takenIndex);
        }

        int[] cutPoints = new int[parents.Count];
        for (int i=0; i < parents.Count; i++) {
            cutPoints[i] = Random.Range(0, 3);
            for (int j = cutPoints[i] + 1; j < 3; j++) {
                if (j == 0) {
                    parents[i].Health = matches[i].health;
                }
                else if (j == 1) {
                    parents[i].Health = matches[i].attack;
                }
                else if (j == 2) {
                    parents[i].Health = matches[i].defense;
                }
            }
        }
    }

    private void Mutation() {
        int mutationCount = (int)(mutationRate * 3 * populations);  //3 adalah jumlah gen
        for (int i=0; i < mutationCount; i++) {
            int randomIndex = Random.Range(0, mutationCount);
            int chromosomeIndex = randomIndex / 3;
            int genIndex = randomIndex % 3;

            float randomStats = generateRandomStats(genIndex);
            controller.enemies[chromosomeIndex].SetAttributesByIndex(genIndex, randomStats);
            Debug.Log("Change enemy-" + chromosomeIndex + " at gen-" + genIndex + " with value of " + randomStats);
        }
    }

    private float generateRandomStats(int genIndex) {
        if (genIndex == 0) {
            return Random.Range(0f, controller.player.Health);
        }
        else if (genIndex == 1) {
            return Random.Range(0f, controller.player.Attack);
        }
        else {
            return Random.Range(0f, controller.player.Defense);
        }
    }
}
