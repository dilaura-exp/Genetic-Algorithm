using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Attributes {
    public float health;
    public float attack;
    public float defense;
}

public class Unit : MonoBehaviour {

    [SerializeField]
    private Attributes attributes;

    public Attributes GetAttributes() {
        return attributes;
    }

    public float Health {
        get { return attributes.health; }
        set { attributes.health = value; }
    }

    public float Attack {
        get { return attributes.attack; }
        set { attributes.attack = value; }
    }

    public float Defense {
        get { return attributes.defense; }
        set { attributes.defense = value; }
    }

    public void SetAttributes(float health, float attack, float defense) {
        attributes.health = health;
        attributes.attack = attack;
        attributes.defense = defense;
    }

    public void SetAttributes(Attributes attributes) {
        this.attributes = attributes;
    }

    public void SetAttributesByIndex(int index, float value) {
        if (index == 0) {
            Health = value;
        }
        else if (index == 1) {
            Attack = value;
        }
        else {
            Defense = value;
        }
    }
}
