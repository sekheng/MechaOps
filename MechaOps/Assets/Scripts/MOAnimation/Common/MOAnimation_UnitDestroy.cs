﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

public class MOAnimation_UnitDestroy : MOAnimation
{
    // Prefabs
    [SerializeField] private ParticleSystem m_ExplosionPrefab = null;
    [SerializeField] private ParticleSystem m_FlamePrefab = null;
    [SerializeField] private MOAnimator m_Animator;

    // Runtime Created
    private ParticleSystem m_Explosion = null;
    private ParticleSystem m_Flame = null;

    public override MOAnimator GetMOAnimator() { return m_Animator; }

    private void DeleteAnimationObjects()
    {
        if (m_Explosion != null)
        {
            Destroy(m_Explosion.gameObject);
        }

        if (m_Flame != null)
        {
            Destroy(m_Flame.gameObject);
        }
    }

    public override void StartAnimation()
    {
        Assert.IsTrue(m_ExplosionPrefab != null);
        Assert.IsTrue(m_FlamePrefab != null);

        DeleteAnimationObjects();

        m_Explosion = GameObject.Instantiate(m_ExplosionPrefab.gameObject, gameObject.transform, false).GetComponent<ParticleSystem>();
        m_Flame = GameObject.Instantiate(m_FlamePrefab.gameObject, gameObject.transform, false).GetComponent<ParticleSystem>();

        m_Animator.StartDeathAnimation(CompletionCallback);
    }

    public override void PauseAnimation()
    {
        if (m_Explosion != null)
        {
            m_Explosion.Pause();
        }

        if (m_Flame != null)
        {
            m_Flame.Pause();
        }

        m_Animator.PauseDeathAnimation();
    }

    public override void ResumeAnimation()
    {
        if (m_Explosion != null)
        {
            m_Explosion.Play();
        }

        if (m_Flame != null)
        {
            m_Flame.Play();
        }

        m_Animator.ResumeDeathAnimation();
    }

    public override void StopAnimation()
    {
        DeleteAnimationObjects();

        m_Animator.StopDeathAnimation();
    }
}