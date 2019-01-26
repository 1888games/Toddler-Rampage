﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTarget : MonoBehaviour
{
    [SerializeField] protected List<AbstractTool> _itemsIWillReactWith;

    protected bool waitingForConfirmation;

    // Start is called before the first frame update
    public virtual void Start()
    {
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (waitingForConfirmation && Input.GetKeyDown(KeyCode.Space))
        {
            waitingForConfirmation = false;

            ConfirmationReceived(ToddlerController.CurrentTool, this);

            TryReact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Tool"))
        {
            if (IsReactableTool())
            {
                waitingForConfirmation = true;
                InteractionAttempt(ToddlerController.CurrentTool, this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Tool"))
        {
            if (IsReactableTool())
            {
                waitingForConfirmation = false;
                OnWalkaway(ToddlerController.CurrentTool, this);
            }
        }
    }

    public virtual void TryReact()
    {
        if (IsReactableTool())
        {
            React();
        }
        else
        {
            print("interaction fail. current tool: " + ToddlerController.CurrentTool.name);
        }
    }

    private AbstractTool IsReactableTool()
    {
        return _itemsIWillReactWith.Find(tool => tool.name == ToddlerController.CurrentTool.name);
    }

    public abstract void React();

    public static Action<string> ReactionComplete= delegate {  };

    public static Action<AbstractTool, AbstractTarget> ConfirmationReceived = delegate {  };

    public static Action<AbstractTool, AbstractTarget> InteractionAttempt= delegate {  };

    public static Action<AbstractTool, AbstractTarget> OnWalkaway = delegate { };
}
