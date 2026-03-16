using System;
using System.Collections.Generic;

public class BaseScene
{
	public virtual List<string> FirstLoadResourcesList()
	{
		return null;
	}

	public virtual void OnCreateScene()
	{
	}

	public virtual bool OnCreateSceneWait()
	{
		return true;
	}

	public virtual void OnEnableScene(object args)
	{
	}

	public virtual bool OnEnableSceneWait()
	{
		return true;
	}

	public virtual void OnStartSceneFade()
	{
	}

	public virtual void OnStartSceneFadeWait()
	{
	}

	public virtual void OnStartControl()
	{
	}

	public virtual void Update()
	{
	}

	public virtual void LateUpdate()
	{
	}

	public virtual void OnStopControl()
	{
	}

	public virtual void OnStopControlFadeWait()
	{
	}

	public virtual void OnDisableScene()
	{
	}

	public virtual bool OnDisableSceneWait()
	{
		return true;
	}

	public virtual void OnDestroyScene()
	{
	}
}
