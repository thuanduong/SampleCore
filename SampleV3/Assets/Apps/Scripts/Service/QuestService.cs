using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Core.Model;

public class QuestService : IDisposable
{
    private readonly IDIContainer container;
    private CancellationTokenSource cts;
    private ISocketClient client;
    private ISocketClient Client => client ??= container.Inject<ISocketClient>();

    private static QuestService instance;
    public static QuestService Instance => instance;

    public static QuestService Instantiate(IDIContainer container)
    {
        if (instance == default)
        {
            instance = new QuestService(container);
        }
        return instance;
    }

    private QuestService(IDIContainer container)
    {
        this.container = container;
        cts = new CancellationTokenSource();
    }

    public void Dispose()
    {
        DisposeUtility.SafeDispose(ref cts);
        client = default;
        instance = default;
    }

    public async UniTask getPlayerQuest()
    {
        //FakeData
        try
        {
            //await Client.Send

            //FakeData
            DateTime s = DateTime.UtcNow;
            s = s.AddDays(1);
            UserData.Instance.Drop<UserQuest>();
            UserData.Instance.Insert(new UserQuest()
            {
                DailyResetTime = s.Ticks,
                CurrentPoint = 0,
                TotalDailyPoint = 100,
            });

        }
        catch (TimeoutException)
        {

        }

        await UniTask.CompletedTask;

    }

    public static async UniTask GetPlayerQuest(CancellationToken cancellationToken)
    {
        if (instance == default) return;

        await instance.getPlayerQuest().AttachExternalCancellation(cancellationToken);

        await UniTask.CompletedTask;

    }

    public async UniTask getPlayerQuestChallenge()
    {
        //FakeData
        try
        {
            //await Client.Send

            //FakeData
            //MasterData
            MasterData.Instance.Drop<MasterQuestChallenge>();
            MasterData.Instance.Insert(new MasterQuestChallenge()
            {
                Id = "1",
                TypeOfChallenge = TypeOfChallenge.WinMoney,
                Name = "Win 100k chip",
                Max = 100000,
                TypeOfReward = TypeOfReward.ChallengePoint,
                IdOfReward = "",
                RewardValue = 10,
            });
            MasterData.Instance.Insert(new MasterQuestChallenge()
            {
                Id = "2",
                TypeOfChallenge = TypeOfChallenge.WinMoney,
                Name = "Win 300k chip",
                Max = 300000,
                TypeOfReward = TypeOfReward.ChallengePoint,
                IdOfReward = "",
                RewardValue = 10,
            });

            //UserData
            UserData.Instance.Drop<UserQuestChallenge>();
            UserData.Instance.Insert(new UserQuestChallenge() {
                Id = "1",
                IdOfChallenge = "1",
                Claimable = true,
                Claimed = false,
                Current = 0,
            });
            UserData.Instance.Insert(new UserQuestChallenge()
            {
                Id = "2",
                IdOfChallenge = "2",
                Claimable = false,
                Claimed = false,
                Current = 0,
            });

        }
        catch (TimeoutException)
        {

        }

        await UniTask.CompletedTask;

    }

    public static async UniTask GetPlayerQuestChallenge(CancellationToken cancellationToken)
    {
        if (instance == default) return;

        await instance.getPlayerQuestChallenge().AttachExternalCancellation(cancellationToken);

        await UniTask.CompletedTask;

    }

    public async UniTask getPlayerQuestReward()
    {
        //FakeData
        try
        {
            //await Client.Send

            //FakeData
            //MasterData
            MasterData.Instance.Drop<MasterQuestReward>();
            MasterData.Instance.Insert(new MasterQuestReward()
            {
                Id = "1",
                ChallengePoint = 10,
                TypeOfReward = TypeOfReward.Chip,
                IdOfReward = "",
                RewardValue = 5000,
                Vip = 0,
            });
            MasterData.Instance.Insert(new MasterQuestReward()
            {
                Id = "2",
                ChallengePoint = 20,
                TypeOfReward = TypeOfReward.Chip,
                IdOfReward = "",
                RewardValue = 10000,
                Vip = 0,
            });
            MasterData.Instance.Insert(new MasterQuestReward()
            {
                Id = "1001",
                ChallengePoint = 10,
                TypeOfReward = TypeOfReward.Chip,
                IdOfReward = "",
                RewardValue = 5000,
                Vip = 1,
            });
            MasterData.Instance.Insert(new MasterQuestReward()
            {
                Id = "1002",
                ChallengePoint = 20,
                TypeOfReward = TypeOfReward.Chip,
                IdOfReward = "",
                RewardValue = 10000,
                Vip = 1,
            });

            //UserData
            UserData.Instance.Drop<UserQuestReward>();
            UserData.Instance.Insert(new UserQuestReward()
            {
                Id = "1",
                IdOfQuestReward = "1",
                Claimable = true,
                Claimed = false,
            });
            UserData.Instance.Insert(new UserQuestReward()
            {
                Id = "2",
                IdOfQuestReward = "2",
                Claimable = false,
                Claimed = false,
            });
            UserData.Instance.Insert(new UserQuestReward()
            {
                Id = "1001",
                IdOfQuestReward = "1001",
                Claimable = true,
                Claimed = false,
            });
            UserData.Instance.Insert(new UserQuestReward()
            {
                Id = "1002",
                IdOfQuestReward = "1002",
                Claimable = false,
                Claimed = false,
            });

        }
        catch (TimeoutException)
        {

        }

        await UniTask.CompletedTask;

    }

    public static async UniTask GetPlayerQuestReward(CancellationToken cancellationToken)
    {
        if (instance == default) return;

        await instance.getPlayerQuestReward().AttachExternalCancellation(cancellationToken);

        await UniTask.CompletedTask;

    }

    public async UniTask<bool> claimChallenge(string IdOfUserChallenge)
    {
        //FakeData
        try
        {
            //await Client.Send

            //FakeData
            //UserData
            var ss = UserData.Instance.Get<UserQuestChallenge>(IdOfUserChallenge);
            ss.Claimed = true;
            UserData.Instance.Update(ss);
            return true;
        }
        catch (TimeoutException)
        {

        }

        await UniTask.CompletedTask;
        return false;
    }
    public static async UniTask<bool> ClaimChallenge(string IdOfUserChallenge, CancellationToken cancellationToken)
    {
        if (instance == default) return false;

        bool kk = await instance.claimChallenge(IdOfUserChallenge).AttachExternalCancellation(cancellationToken);

        return kk;
    }

    public async UniTask<bool> claimReward(string IdOfUserReward)
    {
        //FakeData
        try
        {
            //await Client.Send

            //FakeData
            //UserData
            var ss = UserData.Instance.Get<UserQuestReward>(IdOfUserReward);
            ss.Claimed = true;
            UserData.Instance.Update(ss);
            return true;
        }
        catch (TimeoutException)
        {

        }

        await UniTask.CompletedTask;
        return false;
    }
    public static async UniTask<bool> ClaimReward(string IdOfUserReward, CancellationToken cancellationToken)
    {
        if (instance == default) return false;

        bool kk = await instance.claimReward(IdOfUserReward).AttachExternalCancellation(cancellationToken);

        return kk;
    }


}
