using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

using UnityEngine;
using Cysharp.Threading.Tasks;
using Core.Model;

public class UIQuestPresenter : IDisposable
{
    private IDIContainer Container { get; }
    private CancellationTokenSource cts;

    private UIQuest uiQuest;
    private int CurrentTabIndex = -1;

    private QuestPresenter questPresenter;
    public QuestPresenter QuestPresenter => questPresenter ??= Container.Inject<QuestPresenter>();

    private LoadingPresenter loadingPresenter;
    public LoadingPresenter LoadingPresenter => loadingPresenter ??= Container.Inject<LoadingPresenter>();

    //Data
    private UserQuest mUserQuest;
    private UserQuestChallenge[] uChallenges;
    private UserQuestReward[] uRewards;
    private Dictionary<int, UserQuestReward[]> dRewards = new Dictionary<int, UserQuestReward[]>();

    public UIQuestPresenter(IDIContainer container)
    {
        Container = container;
    }

    public void Dispose()
    {
        cts.SafeCancelAndDispose();
        cts = default;
        UILoader.SafeRelease(ref uiQuest);
        
        uChallenges = default;
        uRewards = default;
        mUserQuest = default;
        uChallenges = default;
        dRewards.Clear();
    }

    public async UniTask ShowAsync()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        await LoadingPresenter.ActiveWaiting(true);

        await FetchData();
        uiQuest ??= await UILoader.Instantiate<UIQuest>(token: cts.Token);
        uiQuest.Rewards.HandleGrid(false);
        if (uiQuest.entity == default)
        {
            uiQuest.SetEntity(new UIQuest.Entity()
            {
                ToggleChallenge = new UIToggleComponent.Entity() { isOn = true, onActiveToggle = OnToggleChallenge },
                ToggleReward = new UIToggleComponent.Entity() { isOn = false, onActiveToggle = OnToggleReward },
                Challenges = new UIQuestChallengeTab.Entity()
                {
                    TopVisible = new IsVisibleComponent.Entity(true),
                    ListQuests = new UIQuestListChallengeItem.Entity() { entities = await getListChallenge() },
                    ResetTime = new UIComponentCountDownTimer.Entity()
                    {
                        outDatedEvent = OnOutDateChallenge,
                        utcTick = mUserQuest.DailyResetTime,
                    },
                },
                Rewards = new UIQuestRewardTab.Entity()
                {
                    ListFree = new UIQuestRewardListItem.Entity() { entities = await getListRewardFree() },
                    ListVIP = new UIQuestRewardListItem.Entity() { entities = await getListRewardVip() },
                    ListProcess = new UIQuestRewardListProcessItem.Entity() { entities = await getListRewardProcess() },
                    Process = mUserQuest.TotalDailyPoint > 0 ? new UIProgressBarComponent.Entity() { progress = (float)mUserQuest.CurrentPoint / mUserQuest.TotalDailyPoint } : null,
                    ProcessValue = mUserQuest.TotalDailyPoint > 0 ? $"{mUserQuest.CurrentPoint}/{mUserQuest.TotalDailyPoint}" : "",
                },
            });
        }

        await LoadingPresenter.ActiveWaiting(false);
        uiQuest.Rewards.HandleGrid(true);

        await UniTask.WhenAll(uiQuest.In(), ShowChallengeTab()).AttachExternalCancellation(cts.Token);
    }

    public async UniTask HideAsync()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        await uiQuest.Out();
    }

    private async UniTask FetchData()
    {
        await QuestPresenter.FetchData();

        mUserQuest = UserData.Instance.GetOne<UserQuest>();

        FetchChallengeData();
        FetchRewardData();
    }

    private void FetchChallengeData()
    {
        uChallenges = UserData.Instance.GetAll<UserQuestChallenge>().ToArray();
    }

    private void FetchRewardData()
    {
        var master = MasterData.Instance.GetAll<MasterQuestReward>().ToArray();
        uRewards = UserData.Instance.GetAll<UserQuestReward>().Where(x=>!string.IsNullOrEmpty(x.Id))
            .OrderBy(x=>x.IdInt).ToArray();
        var uni = master.Select(x => x.Vip).Distinct().Count();
        dRewards.Clear();
        int vip = 0;
        for(int i = 0;  i < uni; i++)
        {
            var mm = uRewards.Where(x => x.Reward.Vip == vip).ToArray();
            if (mm.Length > 0)
                dRewards.Add(vip, mm);
            vip++;
        }
    }


    private void OnToggleChallenge(bool active)
    {
        if (active)
        {
            cts.SafeCancelAndDispose();
            cts = new CancellationTokenSource();
            ShowChallengeTab().Forget();
        }
    }

    private void OnToggleReward(bool active)
    {
        if (active)
        {
            cts.SafeCancelAndDispose();
            cts = new CancellationTokenSource();
            ShowRewardTab().Forget();
        }
    }

    private async UniTask ShowChallengeTab()
    {
        if (CurrentTabIndex == 2)
        {
            CurrentTabIndex = 1;
            await UniTask.WhenAll(uiQuest.Rewards.Out(), uiQuest.Challenges.In()).AttachExternalCancellation(cts.Token);
        }
        else
        {
            CurrentTabIndex = 1;
            await uiQuest.Challenges.In();
        }
        
    }

    private async UniTask ShowRewardTab()
    {
        if (CurrentTabIndex == 1)
        {
            CurrentTabIndex = 2;
            await UniTask.WhenAll(uiQuest.Challenges.Out(), uiQuest.Rewards.In()).AttachExternalCancellation(cts.Token);
        }
        else
        {
            CurrentTabIndex = 2;
            await uiQuest.Rewards.In();
        }
        

    }

    private async UniTask<UIQuestChallengeItem.Entity[]> getListChallenge()
    {
        List<UIQuestChallengeItem.Entity> ml = new List<UIQuestChallengeItem.Entity>();
        
        for (int i = 0; i < uChallenges.Length; i++)
        {
            var item = uChallenges[i];
            var mItem = item.Challenge;
            var index = i;
            var e = new UIQuestChallengeItem.Entity()
            {
                Icon = new UIImageComponent.Entity(),
                Process = mItem.Max > 0 ? new UIProgressBarComponent.Entity() { progress = item.Current / mItem.Max } : null,
                ProcessValue = mItem.Max > 0 ? $"{item.Current}/{mItem.Max}" : "",
                RewardIcon = new UIImageComponent.Entity(),
                Reward = mItem.RewardValue > 0 ? mItem.RewardValue.ToString() : "",
                Title  = mItem.Name,
                Button = new ButtonEntity(()=> OnClaimChallenge(index).Forget(), isInteractable: item.Claimable && !item.Claimed),
            };
            ml.Add(e);
        }

        return ml.ToArray();
    }

    private async UniTask<UIQuestRewardItem.Entity[]> getListRewardFree()
    {
        return await getListReward(0);
    }

    private async UniTask<UIQuestRewardItem.Entity[]> getListRewardVip()
    {
        return await getListReward(1);
    }

    private async UniTask<UIQuestRewardItem.Entity[]> getListReward(int Vip)
    {
        List<UIQuestRewardItem.Entity> ml = new List<UIQuestRewardItem.Entity>();
        if (dRewards.ContainsKey(Vip) && dRewards[Vip].Length > 0)
        {
            var mm = dRewards[Vip];
            for (int i = 0; i < mm.Length; i++)
            {
                var item = mm[i];
                var Id = item.Id;
                UIQuestRewardItem.Entity e = new UIQuestRewardItem.Entity()
                {
                    Icon = new UIImageComponent.Entity(),
                    Value = item.Reward.RewardValue > 0 ? item.Reward.RewardValue.ToString() : "",
                    Button = new ButtonEntity(() => OnClaimReward(Id).Forget(), isInteractable: item.Claimable && !item.Claimed),
                };
                ml.Add(e);
            }
        }
        return ml.ToArray();
    }

    private async UniTask<UIQuestRewardProcessItem.Entity[]> getListRewardProcess()
    {
        List<UIQuestRewardProcessItem.Entity> ml = new List<UIQuestRewardProcessItem.Entity>();
        int[] keys = dRewards.Keys.ToArray();
        List<int> counter = new List<int>(dRewards.Count);
        for (int i = 0; i < keys.Length; i++)
        {
            counter.Add(0);
        }

        if (counter.Count > 0)
        {
            bool isEnd = false;
            while (!isEnd) {
                int lowest = -1;
                for (int i = 0; i < keys.Length; i++)
                {
                    var index = counter[i];
                    var mm = dRewards[keys[i]];
                    if (index < mm.Length)
                    {
                        var ke = (dRewards[keys[i]])[index];
                        if (ke.Reward.ChallengePoint < lowest || lowest == -1)
                            lowest = ke.Reward.ChallengePoint;
                    }
                }

                for (int i = 0; i < keys.Length; i++)
                {
                    var index = counter[i];
                    var mm = dRewards[keys[i]];
                    if (index < mm.Length)
                    {
                        var ke = mm[index];
                        if (ke.Reward.ChallengePoint == lowest)
                        {
                            counter[i]++;
                        }
                    }
                }

                if (lowest != -1)
                {
                    ml.Add(new UIQuestRewardProcessItem.Entity()
                    {
                        Number = lowest.ToString(),
                    });
                }
                else
                {
                    isEnd = true;
                }
            }

            if (ml.Count > 0)
            {
                ml[0].IsFirst = true;
                ml[ml.Count - 1].IsLast = true;
            }
        }

        return ml.ToArray();
    }

    private void OnOutDateChallenge()
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();
    }

    private async UniTask OnClaimChallenge(int index)
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        await LoadingPresenter.ActiveWaiting(true);
        var c = uChallenges[index];
        bool kq = await QuestPresenter.ClaimChallenge(c.Id);

        if (kq == true)
        {
            uiQuest.Challenges.ListQuests.instanceList[index].Button.SetInteracble(false);
        }

        await LoadingPresenter.ActiveWaiting(false);
    }

    private async UniTask OnClaimReward(string Id)
    {
        cts.SafeCancelAndDispose();
        cts = new CancellationTokenSource();

        await LoadingPresenter.ActiveWaiting(true);
        var c = uRewards.First(x => x.Id == Id);
        if (c != default)
        {
            
            bool kq = await QuestPresenter.ClaimReward(c.Id);
            if (dRewards.ContainsKey(c.Reward.Vip))
            {
                var mm = dRewards[c.Reward.Vip];
                var index = Array.IndexOf(mm, c);
                if (kq == true)
                {
                    if(c.Reward.Vip == 0)
                        uiQuest.Rewards.ListFree.instanceList[index].Button.SetInteracble(false);
                    else
                        uiQuest.Rewards.ListVIP.instanceList[index].Button.SetInteracble(false);
                }
            }
        }
        await LoadingPresenter.ActiveWaiting(false);
    }
}
