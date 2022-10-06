namespace Slothsoft.Challenger.Models;

/// <summary>
/// These constants can be used to compare to <code>ShopMenu.storeContext</code>.
/// </summary>
internal static class ShopIds {
    public const string Pierre = "SeedShop";
    public const string Clint = "Blacksmith";
    public const string JoJo = "JojaMart";
}

/// <summary>
/// These constants can be used to compare to <code>Object.ParentSheetIndex</code>.
/// See https://stardewcommunitywiki.com/Modding:Object_data
/// </summary>
internal static class ObjectIds {
    public const int Juice = 350;
    public const int UnmilledRice = 271;
}

/// <summary>
/// These constants can be used to compare to <code>Object.Category</code>.
/// See https://stardewvalleywiki.com/Modding:Items#Categories
/// </summary>
internal static class CategoryIds {

    public const int ArtisanGoods = -26;
}

/// <summary>
/// These constants can be used to compare to <code>Object.ParentSheetIndex</code>.
/// See https://stardewcommunitywiki.com/Modding:Object_data
/// </summary>
internal static class SeedIds {
    
    // Seeds - Spring
    public const int BlueJazz = 429;
    public const int Cauliflower = 474;
    public const int CoffeeBean = 433; // also summer
    public const int Garlic = 476;
    public const int GreenBean = 473;
    public const int Kale = 477;
    public const int Parsnip = 472;
    public const int Potato = 475;
    public const int Rhubarb = 478;
    public const int Strawberry = 745;
    public const int Tulip = 427;

    public const int Rice = 273;

    // Seeds - Summer
    public const int Blueberry = 481;
    public const int Corn = 487; // also fall
    public const int Hops = 302;
    public const int HotPepper = 482;
    public const int Melon = 479;
    public const int Poppy = 453;
    public const int Radish = 484;
    public const int RedCabbage = 485;
    public const int Starfruit = 268;
    public const int SummerSpangle = 455;
    public const int Sunflower = 431; // also fall
    public const int Tomato = 480;

    public const int Wheat = 483; // also fall

    // Seeds - Fall
    public const int Amaranth = 299;
    public const int Artichoke = 489;
    public const int Beet = 494;
    public const int BokChoy = 491;
    public const int Cranberries = 493;
    public const int Eggplant = 488;
    public const int FairyRose = 425;
    public const int Grape = 301;
    public const int Pumpkin = 490;

    public const int Yam = 492;

    // Seeds - Special
    public const int AncientFruit = 499;
    public const int CactusFruit = 802;
    public const int Pineapple = 833;
    public const int QiFruit = 890;
    public const int SweetGemBerry = 347;
    public const int TaroRoot = 831;
    public const int TeaLeaves = 251;

    public static readonly int[] AllSeeds = new[] {
        BlueJazz, Cauliflower, CoffeeBean, Garlic, GreenBean, Kale, Parsnip, Potato, Rhubarb, Strawberry, Tulip, Rice,
        Blueberry, Corn, Hops, HotPepper, Melon, Poppy, Radish, RedCabbage, Starfruit, SummerSpangle, Sunflower, Tomato,
        Wheat, Amaranth, Artichoke, Beet, BokChoy, Cranberries, Eggplant, FairyRose, Grape, Pumpkin, Yam, AncientFruit,
        CactusFruit, Pineapple, QiFruit, SweetGemBerry, TaroRoot, TeaLeaves,
    };
}
