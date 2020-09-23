using System;

namespace Admin.Models.SalesStore
{
    ///=======================================
    /// <summary>
    /// 상품 모델
    /// </summary>
    ///=======================================
    public class ItemModel
    {
        public Int64  ItemNo         { get; set; }
        public string ItemName       { get; set; }
        public Int64  ItemGroupNo    { get; set; }
        public string ItemGroupName  { get; set; }
        public Int64  SalesStoreNo   { get; set; }

        public string SalesStoreName { get; set; }
        public int    ItemPrice      { get; set; }
        public int    TotalINCnt     { get; set; }
        public int    TotalOUTCnt    { get; set; }
        public int    RemainCnt      { get; set; }

        public Int16  UseState       { get; set; }
        public string AdminID        { get; set; }
        public string RegDate        { get; set; }
        public string UpdDate        { get; set; }

        public ItemGroupModel objItemGroupModel { get; set; }
    }

    ///=======================================
    /// <summary>
    /// 상품 분류 모델
    /// </summary>
    ///=======================================
    public class ItemGroupModel
    {
        public Int64  ItemGroupNo   { get; set; }
        public string ItemGroupName { get; set; }
        public string ItemGroupDesc { get; set; }
        public Int16  UseState      { get; set; }
        public string AdminID       { get; set; }

        public string RegDate       { get; set; }
        public string UpdDate       { get; set; }
    }
}

