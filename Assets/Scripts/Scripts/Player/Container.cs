using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    // HERE YOU JUST CALL EACH CLASS AND THEIR METHODS

    [SerializeField] public new playerHealth playerHealth;
    [SerializeField] public new PlayerMovement playerMovement;
    [SerializeField] public new HealthBar healthBar;
    [SerializeField] public new KnockBack knockBack;
    [SerializeField] public new obstacle obstacle;
    [SerializeField] public new SoundFx Sfx;

   // [SerializeField] public new Rigidbody2D rb;
    public static Container Instance { get;  private set;}
    public Rigidbody2D rb { get; private set; }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }




}
