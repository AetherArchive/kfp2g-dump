using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using UnityEngine;

public class DataManagerScenario
{
	private List<DataManagerScenario.LoginScenarioPlayedIdData> _scenarioPlayedIdList { get; set; }

	public DataManagerScenario(DataManager p)
	{
		this.parentData = p;
	}

	public void InitializeMstData(MstManager mstManager)
	{
		this._scenarioPlayedIdList = new List<DataManagerScenario.LoginScenarioPlayedIdData>();
		List<MstScenarioLogin> mst = mstManager.GetMst<List<MstScenarioLogin>>(MstType.SCENARIO_LOGIN);
		List<MstRandomScenarioLogin> mst2 = mstManager.GetMst<List<MstRandomScenarioLogin>>(MstType.RANDOM_SCENARIO_LOGIN);
		if (mst != null)
		{
			this._mstLoginScenarioDataList.Clear();
			foreach (MstScenarioLogin mstScenarioLogin in mst)
			{
				this._mstLoginScenarioDataList.Add(new DataManagerScenario.LoginScenarioData(mstScenarioLogin));
			}
		}
		if (mst2 != null)
		{
			this._mstRandomScenarioDataMap.Clear();
			foreach (MstRandomScenarioLogin mstRandomScenarioLogin in mst2)
			{
				DataManagerScenario.RandomLoginScenarioData randomLoginScenarioData = new DataManagerScenario.RandomLoginScenarioData(mstRandomScenarioLogin);
				if (!this._mstRandomScenarioDataMap.ContainsKey(mstRandomScenarioLogin.id))
				{
					List<DataManagerScenario.RandomLoginScenarioData> list = new List<DataManagerScenario.RandomLoginScenarioData>();
					list.Add(randomLoginScenarioData);
					this._mstRandomScenarioDataMap.Add(mstRandomScenarioLogin.id, list);
				}
				else
				{
					this._mstRandomScenarioDataMap[mstRandomScenarioLogin.id].Add(randomLoginScenarioData);
				}
			}
		}
	}

	public void UpdateUserDataByServer(PlayerInfo playerInfo)
	{
		this._scenarioPlayedIdList.Clear();
		if (!string.IsNullOrEmpty(playerInfo.played_login_scenario_list))
		{
			foreach (DataManagerScenario.LoginScenarioPlayedIdData loginScenarioPlayedIdData in JsonUtility.FromJson<DataManagerScenario.SaveDatas>(playerInfo.played_login_scenario_list).datas)
			{
				this._scenarioPlayedIdList.Add(loginScenarioPlayedIdData);
			}
		}
		this.UpdatePlayLoginScenarioList();
	}

	private void UpdatePlayLoginScenarioList()
	{
		this._playableScenarioLoginList.Clear();
		this._loginScenarioMemoryList.Clear();
		this._loginScenarioMemoryGroupMap.Clear();
		List<DataManagerScenario.LoginScenarioData> loginScenarioList = this.GetLoginScenarioList();
		DateTime now = TimeManager.Now;
		using (List<DataManagerScenario.LoginScenarioData>.Enumerator enumerator = loginScenarioList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DataManagerScenario.LoginScenarioData scenarioData = enumerator.Current;
				DateTime startDateTime = scenarioData.startDateTime;
				DateTime endDateTime = scenarioData.endDateTime;
				if (!(now < startDateTime))
				{
					bool flag = this.IsRandomScenario(scenarioData.randomId);
					if (now >= startDateTime && now < endDateTime && this._scenarioPlayedIdList.Find((DataManagerScenario.LoginScenarioPlayedIdData data) => data.id == scenarioData.id) == null)
					{
						if (flag)
						{
							scenarioData.scenarioId = this.GetRandomScenarioId(scenarioData.randomId);
						}
						this._playableScenarioLoginList.Add(scenarioData);
					}
					if (!flag)
					{
						this.RegistMemory(scenarioData);
					}
					else if (this._loginScenarioMemoryList.Find((DataManagerScenario.LoginScenarioData data) => data.randomId == scenarioData.randomId) == null)
					{
						this.RegistRandomScenario(scenarioData, now);
					}
				}
			}
		}
		this._playableScenarioLoginList.Sort((DataManagerScenario.LoginScenarioData a, DataManagerScenario.LoginScenarioData b) => a.orderId - b.orderId);
	}

	private void RegistMemory(DataManagerScenario.LoginScenarioData data)
	{
		this._loginScenarioMemoryList.Add(data);
		if (!this._loginScenarioMemoryGroupMap.ContainsKey(data.memoryGroupName))
		{
			this._loginScenarioMemoryGroupMap.Add(data.memoryGroupName, new List<DataManagerScenario.LoginScenarioData>());
		}
		this._loginScenarioMemoryGroupMap[data.memoryGroupName].Add(data);
	}

	private void RegistRandomScenario(DataManagerScenario.LoginScenarioData target, DateTime nowTime)
	{
		List<DataManagerScenario.RandomLoginScenarioData> list;
		this._mstRandomScenarioDataMap.TryGetValue(target.randomId, out list);
		if (list == null)
		{
			return;
		}
		DateTime endDateTime = list[0].endDateTime;
		DataManagerScenario.LoginScenarioData mstLoginScenarioData = this.GetMstLoginScenarioData(target.id);
		List<DataManagerScenario.LoginScenarioPlayedIdData> list2 = this._scenarioPlayedIdList.FindAll((DataManagerScenario.LoginScenarioPlayedIdData data) => data.randomId == target.randomId);
		int num = 100000 + mstLoginScenarioData.id;
		int num2 = ((nowTime < endDateTime) ? list2.Count : list.Count);
		for (int i = 0; i < num2; i++)
		{
			int num3 = ((nowTime < endDateTime) ? list2[i].id : list[i].id);
			int targetScenarioId = ((nowTime < endDateTime) ? list2[i].scenarioId : list[i].scenarioId);
			DataManagerScenario.RandomLoginScenarioData randomLoginScenarioData = list.Find((DataManagerScenario.RandomLoginScenarioData mst) => mst.scenarioId == targetScenarioId);
			DataManagerScenario.LoginScenarioData loginScenarioData = new DataManagerScenario.LoginScenarioData();
			loginScenarioData.id = num + i;
			loginScenarioData.memoryGroupName = mstLoginScenarioData.memoryGroupName;
			if (nowTime < endDateTime)
			{
				loginScenarioData.memoryTitleName = this.GetMstLoginScenarioData(num3).orderId.ToString() + "日目 ";
			}
			DataManagerScenario.LoginScenarioData loginScenarioData2 = loginScenarioData;
			loginScenarioData2.memoryTitleName += randomLoginScenarioData.memoryTitleName;
			loginScenarioData.memoryCharaId01 = randomLoginScenarioData.memoryCharaId01;
			loginScenarioData.memoryCharaId02 = randomLoginScenarioData.memoryCharaId02;
			loginScenarioData.memoryText01 = randomLoginScenarioData.memoryText01;
			loginScenarioData.memoryText02 = randomLoginScenarioData.memoryText02;
			loginScenarioData.scenarioFileName = randomLoginScenarioData.scenarioFileName;
			loginScenarioData.randomId = randomLoginScenarioData.id;
			this.RegistMemory(loginScenarioData);
		}
	}

	public bool IsPlayed()
	{
		return this._playableScenarioLoginList.Count <= 0;
	}

	public bool IsRandomScenario(int randomId)
	{
		return this._mstRandomScenarioDataMap.ContainsKey(randomId);
	}

	private int GetRandomScenarioId(int randomId)
	{
		int num = 0;
		if (!this.IsRandomScenario(randomId))
		{
			return num;
		}
		List<DataManagerScenario.RandomLoginScenarioData> list = this._mstRandomScenarioDataMap[randomId];
		int num2 = 0;
		foreach (DataManagerScenario.RandomLoginScenarioData randomLoginScenarioData in list)
		{
			num2 += randomLoginScenarioData.randomWeight;
		}
		Random.InitState(Random.Range(1, 999999));
		int num3 = Random.Range(0, num2);
		foreach (DataManagerScenario.RandomLoginScenarioData randomLoginScenarioData2 in list)
		{
			num3 -= randomLoginScenarioData2.randomWeight;
			if (num3 <= 0)
			{
				num = randomLoginScenarioData2.scenarioId;
				break;
			}
		}
		return num;
	}

	public List<DataManagerScenario.LoginScenarioData> GetLoginScenarioList()
	{
		return this._mstLoginScenarioDataList;
	}

	public DataManagerScenario.LoginScenarioData GetMstLoginScenarioData(int id)
	{
		return this._mstLoginScenarioDataList.Find((DataManagerScenario.LoginScenarioData data) => data.id == id);
	}

	public List<DataManagerScenario.LoginScenarioData> GetPlayableLoginScenarioList()
	{
		return this._playableScenarioLoginList;
	}

	public List<DataManagerScenario.LoginScenarioData> GetLoginScenarioMemoryList()
	{
		return this._loginScenarioMemoryList;
	}

	public Dictionary<string, List<DataManagerScenario.LoginScenarioData>> GetLoginScenarioGroupMap()
	{
		return this._loginScenarioMemoryGroupMap;
	}

	public DataManagerScenario.LoginScenarioData GetLoginScenarioData(int id)
	{
		return this._loginScenarioMemoryList.Find((DataManagerScenario.LoginScenarioData item) => item.id == id);
	}

	public string GetRandomScenarioFileName(int randomId, int scenarioId)
	{
		return this._mstRandomScenarioDataMap[randomId].Find((DataManagerScenario.RandomLoginScenarioData data) => data.scenarioId == scenarioId).scenarioFileName;
	}

	public string GetJsonPlayedScenarioList()
	{
		DataManagerScenario.SaveDatas saveDatas = new DataManagerScenario.SaveDatas();
		saveDatas.datas = new List<DataManagerScenario.LoginScenarioPlayedIdData>();
		foreach (DataManagerScenario.LoginScenarioPlayedIdData loginScenarioPlayedIdData in this._scenarioPlayedIdList)
		{
			DataManagerScenario.LoginScenarioPlayedIdData loginScenarioPlayedIdData2 = new DataManagerScenario.LoginScenarioPlayedIdData();
			loginScenarioPlayedIdData2.id = loginScenarioPlayedIdData.id;
			loginScenarioPlayedIdData2.randomId = loginScenarioPlayedIdData.randomId;
			loginScenarioPlayedIdData2.scenarioId = loginScenarioPlayedIdData.scenarioId;
			saveDatas.datas.Add(loginScenarioPlayedIdData2);
		}
		foreach (DataManagerScenario.LoginScenarioData loginScenarioData in this._playableScenarioLoginList)
		{
			DataManagerScenario.LoginScenarioPlayedIdData loginScenarioPlayedIdData3 = new DataManagerScenario.LoginScenarioPlayedIdData();
			loginScenarioPlayedIdData3.id = loginScenarioData.id;
			loginScenarioPlayedIdData3.randomId = loginScenarioData.randomId;
			loginScenarioPlayedIdData3.scenarioId = loginScenarioData.scenarioId;
			saveDatas.datas.Add(loginScenarioPlayedIdData3);
		}
		return JsonUtility.ToJson(saveDatas);
	}

	private DataManager parentData;

	private List<DataManagerScenario.LoginScenarioData> _playableScenarioLoginList = new List<DataManagerScenario.LoginScenarioData>();

	private List<DataManagerScenario.LoginScenarioData> _mstLoginScenarioDataList = new List<DataManagerScenario.LoginScenarioData>();

	private Dictionary<int, List<DataManagerScenario.RandomLoginScenarioData>> _mstRandomScenarioDataMap = new Dictionary<int, List<DataManagerScenario.RandomLoginScenarioData>>();

	private List<DataManagerScenario.LoginScenarioData> _loginScenarioMemoryList = new List<DataManagerScenario.LoginScenarioData>();

	private Dictionary<string, List<DataManagerScenario.LoginScenarioData>> _loginScenarioMemoryGroupMap = new Dictionary<string, List<DataManagerScenario.LoginScenarioData>>();

	public class LoginScenarioData
	{
		public LoginScenarioData(MstScenarioLogin data)
		{
			this.id = data.id;
			this.scenarioFileName = data.scenarioFileName;
			this.orderId = data.orderId;
			this.startDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(data.startTime));
			this.endDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(data.endTime));
			this.memoryGroupName = data.memoryGroupName;
			this.memoryTitleName = data.memoryTitleName;
			this.memoryCharaId01 = data.memoryCharaId01;
			this.memoryCharaId02 = data.memoryCharaId02;
			this.memoryText01 = data.memoryText01;
			this.memoryText02 = data.memoryText02;
			this.randomId = data.randomId;
			this.scenarioId = 0;
		}

		public LoginScenarioData()
		{
		}

		public int id;

		public string scenarioFileName;

		public int orderId;

		public DateTime startDateTime;

		public DateTime endDateTime;

		public string memoryGroupName;

		public string memoryTitleName;

		public int memoryCharaId01;

		public int memoryCharaId02;

		public string memoryText01;

		public string memoryText02;

		public int randomId;

		public int scenarioId;
	}

	public class RandomLoginScenarioData
	{
		public RandomLoginScenarioData(MstRandomScenarioLogin data)
		{
			this.id = data.id;
			this.scenarioFileName = data.scenarioFileName;
			this.scenarioId = data.scenarioId;
			this.randomWeight = data.randomWeight;
			this.memoryTitleName = data.memoryTitleName;
			this.memoryCharaId01 = data.memoryCharaId01;
			this.memoryCharaId02 = data.memoryCharaId02;
			this.memoryText01 = data.memoryText01;
			this.memoryText02 = data.memoryText02;
			this.endDateTime = new DateTime(PrjUtil.ConvertTimeToTicks(data.endTime));
		}

		public int id;

		public int scenarioId;

		public string scenarioFileName;

		public int randomWeight;

		public string memoryTitleName;

		public int memoryCharaId01;

		public int memoryCharaId02;

		public string memoryText01;

		public string memoryText02;

		public DateTime endDateTime;
	}

	[Serializable]
	public class LoginScenarioPlayedIdData
	{
		public int id;

		public int randomId;

		public int scenarioId;
	}

	[Serializable]
	public class SaveDatas
	{
		public List<DataManagerScenario.LoginScenarioPlayedIdData> datas;
	}
}
