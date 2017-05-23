using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *	
 *  Manage Context For UI Stack
 *
 *	by Xuanyi
 *
 */


public class ContextManager
{
    private Stack<BaseContext> _contextStack = new Stack<BaseContext>();

    private ContextManager()
    {
        Push(new StartContext());
    }

    public void Push(BaseContext nextContext, object[] obj = null)
    {
        if (_contextStack.Count != 0)
        {
            BaseContext curContext = _contextStack.Peek();
            BaseView curView = Singleton<UIManager>.Instance.GetSingleUI(curContext.ViewType).GetComponent<BaseView>();
            curView.OnPause(curContext);
        }

        _contextStack.Push(nextContext);
        BaseView nextView = Singleton<UIManager>.Instance.GetSingleUI(nextContext.ViewType).GetComponent<BaseView>();
        nextView.OnEnter(nextContext, obj);
    }

    public void Pop()
    {
        if (_contextStack.Count != 0)
        {
            BaseContext curContext = _contextStack.Peek();
            _contextStack.Pop();

            BaseView curView = Singleton<UIManager>.Instance.GetSingleUI(curContext.ViewType).GetComponent<BaseView>();
            curView.OnExit(curContext);
        }

        if (_contextStack.Count != 0)
        {
            BaseContext lastContext = _contextStack.Peek();
            BaseView curView = Singleton<UIManager>.Instance.GetSingleUI(lastContext.ViewType).GetComponent<BaseView>();
            curView.OnEnter(lastContext);
        }
    }

    public BaseContext PeekOrNull()
    {
        if (_contextStack.Count != 0)
        {
            return _contextStack.Peek();
        }
        return null;
    }
}

