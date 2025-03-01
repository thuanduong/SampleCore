
public class UserQuestChallenge : UserModel
{
    public string IdOfChallenge { get; set; }
    public float Current { get; set; }
    public bool Claimable { get; set; }
    public bool Claimed { get; set; }

    #region Getter
    private MasterQuestChallenge _challenge;
    public MasterQuestChallenge Challenge
    {
        get
        {
            return _challenge ??= Core.Model.MasterData.Instance.Get<MasterQuestChallenge>(IdOfChallenge);
        }
    }

    #endregion
}
