﻿using UnityEngine;
using System.Collections;

public class Tank1Movement : Movement
{
    public Transform targetFlag;
    private Transform lastTargetFlag;
    private GameObject tankHero;
    private bool targeting;//looking for the target

    void Awake()
    {
        tankHero = GameObject.FindGameObjectWithTag(Tags.hero);

        targeting = true;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log (other.tag);
        if (other.tag == Tags.edge)
        {
            this.targeting = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (tankHero == null) { return; }
        if (Time.deltaTime == 0f) { return; }

        var targetPoint = this.tankHero.transform.position;
        targetPoint.z = this.transform.position.z;

        if (this.targeting)
        {
            var direction = (targetPoint - this.transform.position).normalized;
            base.movingTarget = targetPoint + direction * 3;
            if (this.targetFlag != null)
            {
                if (this.lastTargetFlag != null) { Destroy(this.lastTargetFlag.gameObject); }
                lastTargetFlag = Instantiate(targetFlag, base.movingTarget, this.transform.rotation) as Transform;
            }
            this.targeting = false;
        }

        base.fireTarget = targetPoint;
        base.baseDirection = (base.movingTarget - this.transform.position).normalized;
        this.transform.position += base.baseDirection * base.speed * Time.deltaTime;

        if ((this.transform.position - base.movingTarget).magnitude < 0.02f)
        {
            this.targeting = true;
        }

        //Debug.Log (string.Format ("{0} -> {1} | {2}", this.transform.position, this.targetPosition, this.targeting));
    }

    //3D贴图是Material,2D贴图是Texture
    public Texture targetTexture;

    //OnGUI is called for rendering and handling GUI events.
    //渲染和处理GUI事件时调用。 	
    //This means that your OnGUI implementation might be called several times per frame (one call per event). For more information on GUI events see the Event reference. If the MonoBehaviour's enabled property is set to false, OnGUI() will not be called.
    //这意味着你的OnGUI程序将会在每一帧被调用。要得到更多的GUI事件的信息查阅Event手册。如果Monobehaviour的enabled属性设为false，OnGUI()将不会被调用。
    void OnGUI()
    { //	渲染GUI和处理GUI时调用。
        if (targetTexture != null)
        {
            // 计算图片左上角的坐标
            var screenPosition = Camera.main.WorldToScreenPoint(base.movingTarget);
            float left = screenPosition.x - targetTexture.width / 2;
            float top = Screen.height - screenPosition.y - targetTexture.height / 2;
            var rect = new Rect(left, top, targetTexture.width, targetTexture.height);
            GUI.DrawTexture(rect, targetTexture);
        }
    }

    void OnDestroy()
    {
        if (this.lastTargetFlag != null)
        {
            Destroy(this.lastTargetFlag.gameObject);
        }
    }
}

