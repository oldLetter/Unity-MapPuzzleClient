using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

/*
 *	
 *  Base Animate View
 *
 *	by Xuanyi
 *
 */

public abstract class AnimateView : BaseView 
    {
        [SerializeField]
        protected Animator _animator;

    public override void OnEnter(BaseContext context, object[] obj = null)
        {
        this.gameObject.SetActive(true);
            _animator.SetTrigger("OnEnter");
        }

        public override void OnExit(BaseContext context)
        {
            _animator.SetTrigger("OnExit");
       Invoke("Hide", 0.70f);
    }

        public override void OnPause(BaseContext context)
        {
        _animator.SetTrigger("OnExit");
        Invoke("Hide", 0.70f);
    }

    public override void OnResume(BaseContext context)
        {
        gameObject.SetActive(true);
        _animator.SetTrigger("OnEnter");
    }
    public void Hide()
    {
            gameObject.SetActive(false);       
    }

}

