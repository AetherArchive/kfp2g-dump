using System;
using CriWare;
using CriWare.CriMana;
using SGNFW.Ab;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020000F7 RID: 247
public class MoviePlayer : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x17000307 RID: 775
	// (get) Token: 0x06000BCB RID: 3019 RVA: 0x00045B50 File Offset: 0x00043D50
	// (set) Token: 0x06000BCC RID: 3020 RVA: 0x00045BA8 File Offset: 0x00043DA8
	private float time
	{
		get
		{
			if (this.step != MoviePlayer.STEP.PLAY || this.length <= 0 || this.player.player.frameInfo == null)
			{
				return 0f;
			}
			return (float)this.player.player.frameInfo.frameNo / (float)this.length;
		}
		set
		{
			if (this.step == MoviePlayer.STEP.PLAY && this.length > 0 && this.seek < 0)
			{
				this.step = MoviePlayer.STEP.SEEK;
				this.player.player.Stop();
				this.seek = (int)((float)this.length * Mathf.Clamp01(value));
			}
		}
	}

	// Token: 0x17000308 RID: 776
	// (get) Token: 0x06000BCD RID: 3021 RVA: 0x00045BFC File Offset: 0x00043DFC
	// (set) Token: 0x06000BCE RID: 3022 RVA: 0x00045C04 File Offset: 0x00043E04
	public bool touch { get; private set; }

	// Token: 0x06000BCF RID: 3023 RVA: 0x00045C0D File Offset: 0x00043E0D
	private void play(string mov, bool lp)
	{
		this.movie = mov;
		this.loop = lp;
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x00045C1D File Offset: 0x00043E1D
	private bool playing()
	{
		return !string.IsNullOrEmpty(this.movie) || this.step > MoviePlayer.STEP.IDLE;
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x00045C37 File Offset: 0x00043E37
	private bool loading()
	{
		return this.step == MoviePlayer.STEP.LOAD;
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x00045C44 File Offset: 0x00043E44
	private void OnEnable()
	{
		this.img = base.GetComponent<RawImage>();
		if (this.img != null)
		{
			this.img.color = Color.clear;
			this.img.raycastTarget = true;
		}
		this.touch = false;
		this.seek = -1;
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x00045C98 File Offset: 0x00043E98
	private void Update()
	{
		switch (this.step)
		{
		case MoviePlayer.STEP.LOAD:
			if (!string.IsNullOrEmpty(this.movie))
			{
				this.step = MoviePlayer.STEP.IDLE;
				return;
			}
			if (AssetManager.IsDownloadFinishAssetData(this.path))
			{
				if (AssetManager.IsExsistAssetData(this.path))
				{
					if (this.img != null)
					{
						this.img.color = Color.white;
					}
					this.step = MoviePlayer.STEP.PLAY;
					this.player = base.GetComponent<CriManaMovieControllerForUI>();
					if (this.player == null)
					{
						this.player = base.gameObject.AddComponent<CriManaMovieControllerForUI>();
					}
					this.path = Manager.AssetPath + "assets/" + AssetManager.GetAssetDataFullPath(this.path);
					this.player.player.SetFile(null, this.path, Player.SetMode.New);
					this.player.player.Loop(this.loop);
					this.player.player.Start();
					this.length = 0;
					this.seek = -1;
					return;
				}
				this.step = MoviePlayer.STEP.IDLE;
				return;
			}
			break;
		case MoviePlayer.STEP.PLAY:
		{
			bool flag = false;
			switch (this.player.player.status)
			{
			case Player.Status.Stop:
			case Player.Status.Ready:
			case Player.Status.Error:
			case Player.Status.StopProcessing:
				flag = true;
				break;
			case Player.Status.PlayEnd:
				if (this.loop)
				{
					this.player.player.Start();
				}
				else
				{
					flag = true;
				}
				break;
			}
			if (flag || !string.IsNullOrEmpty(this.movie))
			{
				this.player.player.Stop();
				this.step = MoviePlayer.STEP.IDLE;
			}
			if (this.length <= 0 && this.player.player.movieInfo != null)
			{
				this.length = (int)this.player.player.movieInfo.totalFrames;
				return;
			}
			break;
		}
		case MoviePlayer.STEP.SEEK:
			if (this.player.player.status == Player.Status.Stop)
			{
				this.step = MoviePlayer.STEP.PLAY;
				this.player.player.SetSeekPosition(this.seek);
				this.player.player.Start();
				this.seek = -1;
				return;
			}
			if (this.player.player.status != Player.Status.StopProcessing || !string.IsNullOrEmpty(this.movie))
			{
				this.player.player.Stop();
				this.step = MoviePlayer.STEP.IDLE;
			}
			break;
		default:
			if (this.img != null)
			{
				this.img.color = Color.clear;
			}
			if (!string.IsNullOrEmpty(this.movie))
			{
				this.path = AssetManager.PREFIX_PATH_MOVIE + this.movie;
				AssetManager.DownloadAssetData(this.path, AssetManager.OWNER.MoviePlayer, 0);
				this.movie = null;
				this.step = MoviePlayer.STEP.LOAD;
				return;
			}
			break;
		}
	}

	// Token: 0x06000BD4 RID: 3028 RVA: 0x00045F60 File Offset: 0x00044160
	private void OnDisable()
	{
		if (this.player != null)
		{
			Object.Destroy(this.player);
			this.player = null;
		}
		this.movie = null;
		if (this.img != null)
		{
			this.img.color = Color.clear;
		}
		this.touch = false;
		this.step = MoviePlayer.STEP.IDLE;
	}

	// Token: 0x06000BD5 RID: 3029 RVA: 0x00045FC0 File Offset: 0x000441C0
	public static void Play(GameObject obj, string mov, bool lp = false)
	{
		MoviePlayer moviePlayer = obj.GetComponent<MoviePlayer>();
		if (moviePlayer == null)
		{
			moviePlayer = obj.AddComponent<MoviePlayer>();
		}
		moviePlayer.play(mov, lp);
	}

	// Token: 0x06000BD6 RID: 3030 RVA: 0x00045FEC File Offset: 0x000441EC
	public static bool Playing(GameObject obj)
	{
		MoviePlayer component = obj.GetComponent<MoviePlayer>();
		return component != null && component.playing();
	}

	// Token: 0x06000BD7 RID: 3031 RVA: 0x00046014 File Offset: 0x00044214
	public static bool Loading(GameObject obj)
	{
		MoviePlayer component = obj.GetComponent<MoviePlayer>();
		return component != null && component.loading();
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x0004603C File Offset: 0x0004423C
	public static void Pause(GameObject obj, bool sw)
	{
		MoviePlayer component = obj.GetComponent<MoviePlayer>();
		if (component != null && component.player != null)
		{
			component.player.player.Pause(sw);
		}
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x00046078 File Offset: 0x00044278
	public static float GetTime(GameObject obj)
	{
		MoviePlayer component = obj.GetComponent<MoviePlayer>();
		if (!(component != null) || !(component.player != null))
		{
			return 0f;
		}
		return component.time;
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x000460B0 File Offset: 0x000442B0
	public static void SetTime(GameObject obj, float time)
	{
		MoviePlayer component = obj.GetComponent<MoviePlayer>();
		if (component != null && component.player != null)
		{
			component.time = time;
		}
	}

	// Token: 0x06000BDB RID: 3035 RVA: 0x000460E4 File Offset: 0x000442E4
	public static bool Touch(GameObject obj)
	{
		MoviePlayer component = obj.GetComponent<MoviePlayer>();
		return component != null && component.touch;
	}

	// Token: 0x06000BDC RID: 3036 RVA: 0x00046109 File Offset: 0x00044309
	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData != null && eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		this.touch = true;
	}

	// Token: 0x06000BDD RID: 3037 RVA: 0x0004611E File Offset: 0x0004431E
	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData != null && eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		this.touch = false;
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x00046133 File Offset: 0x00044333
	public void SetVolume(float volume)
	{
		this.player = base.GetComponent<CriManaMovieControllerForUI>();
		if (this.player == null)
		{
			this.player = base.gameObject.AddComponent<CriManaMovieControllerForUI>();
		}
		this.player.player.SetVolume(volume);
	}

	// Token: 0x0400092E RID: 2350
	private RawImage img;

	// Token: 0x0400092F RID: 2351
	private CriManaMovieControllerForUI player;

	// Token: 0x04000930 RID: 2352
	private string movie;

	// Token: 0x04000931 RID: 2353
	private bool loop;

	// Token: 0x04000932 RID: 2354
	private MoviePlayer.STEP step;

	// Token: 0x04000933 RID: 2355
	private string path;

	// Token: 0x04000934 RID: 2356
	private int seek;

	// Token: 0x04000935 RID: 2357
	private int length;

	// Token: 0x02000810 RID: 2064
	private enum STEP
	{
		// Token: 0x0400361A RID: 13850
		IDLE,
		// Token: 0x0400361B RID: 13851
		LOAD,
		// Token: 0x0400361C RID: 13852
		PLAY,
		// Token: 0x0400361D RID: 13853
		SEEK
	}
}
