﻿using UnityEngine;
using UnityEngine.UI;

public class HeroBase : MonoBehaviour
{
    [Header("Hero Data")]
    public HeroData hero;
    public int layer;
    public Transform restartPont;


    protected float[] skillTimer = new float[4];
    protected bool[] isSkill = new bool[4];
    protected Animator ani;
    protected Transform canvasHP;
    protected int timer;
    protected float hp;
    protected float MAX_HP;
    protected Text textHP;
    protected Image imgHP;

    private Rigidbody rig;
    private float restartTime = 3;

    protected virtual void Awake()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        canvasHP = transform.Find("Blood");
        textHP = canvasHP.Find("BloodValue").GetComponent<Text>();
        textHP.text = hero.HP.ToString();
        imgHP = canvasHP.Find("bloodValue").GetComponent<Image>();
        imgHP.fillAmount = 1;
    }

    private void Start()
    {
        hp = hero.HP;
        MAX_HP = hp;
    }

    public virtual void Damage(float damage)
    {
        hp -= damage;
        textHP.text = hp.ToString();
        imgHP.fillAmount = hp / MAX_HP;

        if (hp <= 0)
        {
            Dead();
        }
    }

    protected void Dead(bool needRestart = true)
    {
        gameObject.layer = 0;
        textHP.text = "0";
        ani.SetBool("isDead", true);
        enabled = false;
        canvasHP.eulerAngles = new Vector3(0, 90, 0);

        if (needRestart)
        {
            Invoke("Restart", restartTime);
        }
        else
        {
            Destroy(gameObject, 2f);
        }
    }

    private void Restart()
    {
        gameObject.layer = layer;
        hp = MAX_HP;
        textHP.text = hp.ToString();
        imgHP.fillAmount = 1;
        ani.SetBool("isDead", false);
        enabled = true;
        transform.position = restartPont.position;
    }

    protected virtual void Update()
    {
        TimerControl();
    }

    private void TimerControl()
    {
        for (int i = 0; i < isSkill.Length; i++)
        {
            if (isSkill[i])
            {
                skillTimer[i] += Time.deltaTime;

                if (skillTimer[i] >= hero.skills[i].cd)
                {
                    skillTimer[i] = 0;
                    isSkill[i] = false;
                }
            }
        }

    }

    protected virtual void Move(Transform target)
    {
        Vector3 pos = rig.position;

        rig.MovePosition(target.position);
        transform.LookAt(target);
        ani.SetBool("isRunning", pos != rig.position);

        canvasHP.eulerAngles = new Vector3(0, 90, 0);
    }

    public void Skill_1()
    {
        if (isSkill[0]) return;
        ani.SetTrigger("move1");
        isSkill[0] = true;

    }
    public void Skill_2()
    {
        if (isSkill[1]) return;
        ani.SetTrigger("move2");
        isSkill[1] = true;
    }
    public void Skill_3()
    {
        if (isSkill[2]) return;
        ani.SetTrigger("move3");
        isSkill[2] = true;
    }
    public void Big_Skill()
    {
        if (isSkill[3]) return;
        ani.SetTrigger("bigMove");
        isSkill[3] = true;
    }
}
