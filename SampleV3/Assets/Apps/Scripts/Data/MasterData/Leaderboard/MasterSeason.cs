using System.Collections.Generic;
using System.Linq;
using Core.Model;

public class MasterSeason : MasterModel
{
    public long StartTime { get; set; }
    public long EndTime { get; set; }
    public double Target { get; set; }

    public float BenefitBuyIn { get; set; }
    public float BenefitBonus { get; set; }
    public float BenefitPayment { get; set; }

    public int Upper { get; set; }
    public int Alive { get; set; }

    public List<SeasonRankRewardEmbed> RankRewards { get; set; }

    public MasterSeason()
    {
        RankRewards = new List<SeasonRankRewardEmbed>();
    }

    public SeasonRankRewardEmbed getRank(int rank)
    {
        return RankRewards.FirstOrDefault(x => x.Rank == rank);
    }

}


public class SeasonRankRewardEmbed : OriginalModel
{
    public int Rank { get; set; }
    public int Ticket { get; set; }
}



