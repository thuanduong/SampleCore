using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Core.Model;


public class LeaderboardService : IDisposable
{
    private readonly IDIContainer container;
    private CancellationTokenSource cts;
    private ISocketClient client;
    private ISocketClient Client => client ??= container.Inject<ISocketClient>();

    private static LeaderboardService instance;
    public static LeaderboardService Instance => instance;

    public static LeaderboardService Instantiate(IDIContainer container)
    {
        if (instance == default)
        {
            instance = new LeaderboardService(container);
        }
        return instance;
    }

    private LeaderboardService(IDIContainer container)
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

    public async UniTask getPlayerSeasonRank()
    {
        //FakeData
        try
        {
            //await Client.Send

            //FakeData
            DateTime s = DateTime.UtcNow;
            s = s.AddDays(1);
            UserData.Instance.Drop<PlayerSeasonRank>();
            for (int i = 0; i < 123; i++)
            {
                string id = i.ToString();
                UserData.Instance.Insert(new PlayerSeasonRank()
                {
                    Id = id,
                    PlayerName = $"Player {id}",
                    AvatarType = TypeOfAvatar.Asset,
                    UrlAvatar = "",
                    IdOfSeason = "1",
                    IdOfUser = id,
                    Money = 12345678,
                    Rank = (i + 1),
                });
            }

        }
        catch (TimeoutException)
        {

        }

        await UniTask.CompletedTask;

    }

    public static async UniTask GetPlayerSeasonRank(CancellationToken cancellationToken)
    {
        if (instance == default) return;

        await instance.getPlayerSeasonRank().AttachExternalCancellation(cancellationToken);

        await UniTask.CompletedTask;

    }

    public async UniTask getUserSeasonRank()
    {
        //FakeData
        try
        {
            //await Client.Send

            //FakeData
            UserData.Instance.Drop<PlayerSeasonRank>();
            UserData.Instance.Insert(new UserSeasonRank()
            {
                Id = "0",
                IdOfSeason = "1",
                Money = 12345678,
                Rank = 123,
            });

        }
        catch (TimeoutException)
        {

        }

        await UniTask.CompletedTask;

    }

    public static async UniTask GetUserSeasonRank(CancellationToken cancellationToken)
    {
        if (instance == default) return;

        await instance.getUserSeasonRank().AttachExternalCancellation(cancellationToken);

        await UniTask.CompletedTask;

    }

    public async UniTask getMasterDataSeasons()
    {
        //FakeData
        try
        {
            //await Client.Send

            //FakeData
            DateTime s = new DateTime(2024, 8, 26, 0, 0, 0, DateTimeKind.Utc);
            DateTime e = new DateTime(2024, 9, 1, 23, 59, 59, DateTimeKind.Utc);
            MasterData.Instance.Drop<MasterSeason>();
            var ee = new MasterSeason()
            {
                Id = "1",
                Name = "Beginer",
                StartTime = s.Ticks,
                EndTime = e.Ticks,
                Target = 10000000,
                BenefitBuyIn = 0.25f,
                BenefitPayment = 0.25f,
                BenefitBonus = 0.15f,
                Upper = 40,
                Alive = 100,
            };
            ee.RankRewards.Add(new SeasonRankRewardEmbed() { 
                Rank = 1,
                Ticket = 30,
            });
            for (int i = 2; i < 11; i++)
            {
                ee.RankRewards.Add(new SeasonRankRewardEmbed()
                {
                    Rank = i,
                    Ticket = 20,
                });
            }
            for (int i = 11; i < 31; i++)
            {
                ee.RankRewards.Add(new SeasonRankRewardEmbed()
                {
                    Rank = i,
                    Ticket = 10,
                });
            }
            MasterData.Instance.Insert(ee);

        }
        catch (TimeoutException)
        {

        }

        await UniTask.CompletedTask;
    }

    public static async UniTask GetMasterDataSeasons(CancellationToken cancellationToken)
    {
        if (instance == default) return;

        await instance.getMasterDataSeasons().AttachExternalCancellation(cancellationToken);

        await UniTask.CompletedTask;

    }

}
