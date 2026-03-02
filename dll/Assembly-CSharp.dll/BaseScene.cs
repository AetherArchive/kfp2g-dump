using System;
using System.Collections.Generic;

// Token: 0x020000E8 RID: 232
public class BaseScene
{
	// Token: 0x06000A73 RID: 2675 RVA: 0x0003C96F File Offset: 0x0003AB6F
	public virtual List<string> FirstLoadResourcesList()
	{
		return null;
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x0003C972 File Offset: 0x0003AB72
	public virtual void OnCreateScene()
	{
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x0003C974 File Offset: 0x0003AB74
	public virtual bool OnCreateSceneWait()
	{
		return true;
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x0003C977 File Offset: 0x0003AB77
	public virtual void OnEnableScene(object args)
	{
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x0003C979 File Offset: 0x0003AB79
	public virtual bool OnEnableSceneWait()
	{
		return true;
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x0003C97C File Offset: 0x0003AB7C
	public virtual void OnStartSceneFade()
	{
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x0003C97E File Offset: 0x0003AB7E
	public virtual void OnStartSceneFadeWait()
	{
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x0003C980 File Offset: 0x0003AB80
	public virtual void OnStartControl()
	{
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x0003C982 File Offset: 0x0003AB82
	public virtual void Update()
	{
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x0003C984 File Offset: 0x0003AB84
	public virtual void LateUpdate()
	{
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x0003C986 File Offset: 0x0003AB86
	public virtual void OnStopControl()
	{
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x0003C988 File Offset: 0x0003AB88
	public virtual void OnStopControlFadeWait()
	{
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x0003C98A File Offset: 0x0003AB8A
	public virtual void OnDisableScene()
	{
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x0003C98C File Offset: 0x0003AB8C
	public virtual bool OnDisableSceneWait()
	{
		return true;
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x0003C98F File Offset: 0x0003AB8F
	public virtual void OnDestroyScene()
	{
	}
}
