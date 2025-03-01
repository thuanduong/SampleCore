public class PlayerSeasonRank : UserModel
{
    public int Rank { get; set; }
    public string IdOfUser { get; set; }
    public string PlayerName { get; set; }
    public double Money { get; set; }
    public TypeOfAvatar AvatarType { get; set; }
    public string UrlAvatar { get; set; }
    public string IdOfSeason { get; set; }

    #region Getter

    protected MasterSeason _season;
    public MasterSeason Season
    {
        get
        {
            return _season ??= Core.Model.MasterData.Instance.Get<MasterSeason>(IdOfSeason);
        }
    }

    #endregion
}
