
public class UserShopData : UserModel
{
    public string IdOfProduct { get; set; }
    public bool IsAvai { get; set; }
    

    #region Getter

    protected MasterShopData _product;
    public MasterShopData Product
    {
        get
        {
            return _product ?? (_product = MasterGet<MasterShopData>(IdOfProduct));
        }
    }

    #endregion
}
