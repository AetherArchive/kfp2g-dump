using System;
using CriWare;
using CriWare.CriMana;
using SGNFW.Ab;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoviePlayer : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
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

	public bool touch { get; private set; }

	private void play(string mov, bool lp)
	{
		this.movie = mov;
		this.loop = lp;
	}

	private bool playing()
	{
		return !string.IsNullOrEmpty(this.movie) || this.step > MoviePlayer.STEP.IDLE;
	}

	private bool loading()
	{
		return this.step == MoviePlayer.STEP.LOAD;
	}

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

	public static void Play(GameObject obj, string mov, bool lp = false)
	{
		MoviePlayer moviePlayer = obj.GetComponent<MoviePlayer>();
		if (moviePlayer == null)
		{
			moviePlayer = obj.AddComponent<MoviePlayer>();
		}
		moviePlayer.play(mov, lp);
	}

	public static bool Playing(GameObject obj)
	{
		MoviePlayer component = obj.GetComponent<MoviePlayer>();
		return component != null && component.playing();
	}

	public static bool Loading(GameObject obj)
	{
		MoviePlayer component = obj.GetComponent<MoviePlayer>();
		return component != null && component.loading();
	}

	public static void Pause(GameObject obj, bool sw)
	{
		MoviePlayer component = obj.GetComponent<MoviePlayer>();
		if (component != null && component.player != null)
		{
			component.player.player.Pause(sw);
		}
	}

	public static float GetTime(GameObject obj)
	{
		MoviePlayer component = obj.GetComponent<MoviePlayer>();
		if (!(component != null) || !(component.player != null))
		{
			return 0f;
		}
		return component.time;
	}

	public static void SetTime(GameObject obj, float time)
	{
		MoviePlayer component = obj.GetComponent<MoviePlayer>();
		if (component != null && component.player != null)
		{
			component.time = time;
		}
	}

	public static bool Touch(GameObject obj)
	{
		MoviePlayer component = obj.GetComponent<MoviePlayer>();
		return component != null && component.touch;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData != null && eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		this.touch = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData != null && eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		this.touch = false;
	}

	public void SetVolume(float volume)
	{
		this.player = base.GetComponent<CriManaMovieControllerForUI>();
		if (this.player == null)
		{
			this.player = base.gameObject.AddComponent<CriManaMovieControllerForUI>();
		}
		this.player.player.SetVolume(volume);
	}

	private RawImage img;

	private CriManaMovieControllerForUI player;

	private string movie;

	private bool loop;

	private MoviePlayer.STEP step;

	private string path;

	private int seek;

	private int length;

	private enum STEP
	{
		IDLE,
		LOAD,
		PLAY,
		SEEK
	}
}
