using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using UnityEngine;

// Token: 0x020000A2 RID: 162
public class DataManagerScenario
{
	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06000716 RID: 1814 RVA: 0x00031324 File Offset: 0x0002F524
	// (set) Token: 0x06000717 RID: 1815 RVA: 0x0003132C File Offset: 0x0002F52C
	private List<DataManagerScenario.LoginScenarioPlayedIdData> _scenarioPlayedIdList { get; set; }

	// Token: 0x06000718 RID: 1816 RVA: 0x00031338 File Offset: 0x0002F538
	public DataManagerScenario(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x0003138C File Offset: 0x0002F58C
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

	// Token: 0x0600071A RID: 1818 RVA: 0x000314BC File Offset: 0x0002F6BC
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

	// Token: 0x0600071B RID: 1819 RVA: 0x0003153C File Offset: 0x0002F73C
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

	// Token: 0x0600071C RID: 1820 RVA: 0x000316BC File Offset: 0x0002F8BC
	private void RegistMemory(DataManagerScenario.LoginScenarioData data)
	{
		this._loginScenarioMemoryList.Add(data);
		if (!this._loginScenarioMemoryGroupMap.ContainsKey(data.memoryGroupName))
		{
			this._loginScenarioMemoryGroupMap.Add(data.memoryGroupName, new List<DataManagerScenario.LoginScenarioData>());
		}
		this._loginScenarioMemoryGroupMap[data.memoryGroupName].Add(data);
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x00031718 File Offset: 0x0002F918
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

	// Token: 0x0600071E RID: 1822 RVA: 0x000318F8 File Offset: 0x0002FAF8
	public bool IsPlayed()
	{
		return this._playableScenarioLoginList.Count <= 0;
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x0003190B File Offset: 0x0002FB0B
	public bool IsRandomScenario(int randomId)
	{
		return this._mstRandomScenarioDataMap.ContainsKey(randomId);
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x0003191C File Offset: 0x0002FB1C
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

	// Token: 0x06000721 RID: 1825 RVA: 0x000319F4 File Offset: 0x0002FBF4
	public List<DataManagerScenario.LoginScenarioData> GetLoginScenarioList()
	{
		return this._mstLoginScenarioDataList;
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x000319FC File Offset: 0x0002FBFC
	public DataManagerScenario.LoginScenarioData GetMstLoginScenarioData(int id)
	{
		return this._mstLoginScenarioDataList.Find((DataManagerScenario.LoginScenarioData data) => data.id == id);
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x00031A2D File Offset: 0x0002FC2D
	public List<DataManagerScenario.LoginScenarioData> GetPlayableLoginScenarioList()
	{
		return this._playableScenarioLoginList;
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x00031A35 File Offset: 0x0002FC35
	public List<DataManagerScenario.LoginScenarioData> GetLoginScenarioMemoryList()
	{
		return this._loginScenarioMemoryList;
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x00031A3D File Offset: 0x0002FC3D
	public Dictionary<string, List<DataManagerScenario.LoginScenarioData>> GetLoginScenarioGroupMap()
	{
		return this._loginScenarioMemoryGroupMap;
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x00031A48 File Offset: 0x0002FC48
	public DataManagerScenario.LoginScenarioData GetLoginScenarioData(int id)
	{
		return this._loginScenarioMemoryList.Find((DataManagerScenario.LoginScenarioData item) => item.id == id);
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x00031A7C File Offset: 0x0002FC7C
	public string GetRandomScenarioFileName(int randomId, int scenarioId)
	{
		return this._mstRandomScenarioDataMap[randomId].Find((DataManagerScenario.RandomLoginScenarioData data) => data.scenarioId == scenarioId).scenarioFileName;
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x00031AB8 File Offset: 0x0002FCB8
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

	// Token: 0x04000648 RID: 1608
	private DataManager parentData;

	// Token: 0x04000649 RID: 1609
	private List<DataManagerScenario.LoginScenarioData> _playableScenarioLoginList = new List<DataManagerScenario.LoginScenarioData>();

	// Token: 0x0400064A RID: 1610
	private List<DataManagerScenario.LoginScenarioData> _mstLoginScenarioDataList = new List<DataManagerScenario.LoginScenarioData>();

	// Token: 0x0400064B RID: 1611
	private Dictionary<int, List<DataManagerScenario.RandomLoginScenarioData>> _mstRandomScenarioDataMap = new Dictionary<int, List<DataManagerScenario.RandomLoginScenarioData>>();

	// Token: 0x0400064C RID: 1612
	private List<DataManagerScenario.LoginScenarioData> _loginScenarioMemoryList = new List<DataManagerScenario.LoginScenarioData>();

	// Token: 0x0400064D RID: 1613
	private Dictionary<string, List<DataManagerScenario.LoginScenarioData>> _loginScenarioMemoryGroupMap = new Dictionary<string, List<DataManagerScenario.LoginScenarioData>>();

	// Token: 0x0200076F RID: 1903
	public class LoginScenarioData
	{
		// Token: 0x06003637 RID: 13879 RVA: 0x001C5F28 File Offset: 0x001C4128
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

		// Token: 0x06003638 RID: 13880 RVA: 0x001C5FE6 File Offset: 0x001C41E6
		public LoginScenarioData()
		{
		}

		// Token: 0x04003317 RID: 13079
		public int id;

		// Token: 0x04003318 RID: 13080
		public string scenarioFileName;

		// Token: 0x04003319 RID: 13081
		public int orderId;

		// Token: 0x0400331A RID: 13082
		public DateTime startDateTime;

		// Token: 0x0400331B RID: 13083
		public DateTime endDateTime;

		// Token: 0x0400331C RID: 13084
		public string memoryGroupName;

		// Token: 0x0400331D RID: 13085
		public string memoryTitleName;

		// Token: 0x0400331E RID: 13086
		public int memoryCharaId01;

		// Token: 0x0400331F RID: 13087
		public int memoryCharaId02;

		// Token: 0x04003320 RID: 13088
		public string memoryText01;

		// Token: 0x04003321 RID: 13089
		public string memoryText02;

		// Token: 0x04003322 RID: 13090
		public int randomId;

		// Token: 0x04003323 RID: 13091
		public int scenarioId;
	}

	// Token: 0x02000770 RID: 1904
	public class RandomLoginScenarioData
	{
		// Token: 0x06003639 RID: 13881 RVA: 0x001C5FF0 File Offset: 0x001C41F0
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

		// Token: 0x04003324 RID: 13092
		public int id;

		// Token: 0x04003325 RID: 13093
		public int scenarioId;

		// Token: 0x04003326 RID: 13094
		public string scenarioFileName;

		// Token: 0x04003327 RID: 13095
		public int randomWeight;

		// Token: 0x04003328 RID: 13096
		public string memoryTitleName;

		// Token: 0x04003329 RID: 13097
		public int memoryCharaId01;

		// Token: 0x0400332A RID: 13098
		public int memoryCharaId02;

		// Token: 0x0400332B RID: 13099
		public string memoryText01;

		// Token: 0x0400332C RID: 13100
		public string memoryText02;

		// Token: 0x0400332D RID: 13101
		public DateTime endDateTime;
	}

	// Token: 0x02000771 RID: 1905
	[Serializable]
	public class LoginScenarioPlayedIdData
	{
		// Token: 0x0400332E RID: 13102
		public int id;

		// Token: 0x0400332F RID: 13103
		public int randomId;

		// Token: 0x04003330 RID: 13104
		public int scenarioId;
	}

	// Token: 0x02000772 RID: 1906
	[Serializable]
	public class SaveDatas
	{
		// Token: 0x04003331 RID: 13105
		public List<DataManagerScenario.LoginScenarioPlayedIdData> datas;
	}
}
