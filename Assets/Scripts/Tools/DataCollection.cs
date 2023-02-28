public enum ItemType
{
    None,
    Weapon, //武器
    Bullet, //子弹
    Armor, //防具
    Ring, //戒指
    Material, //材料
    Mission, //任务道具
}

public enum EnemyState
{
    NoFind,
    floowPlayer,
    canAttack
}

public enum SlotType
{
    Bag,
    Box,
    Shop,
    Weapon,
    Bullet,
    Ring,
    Armor,
    MadeEq,
}

public enum InventoryLocation
{
    Player,
    Equipment,
    Box,
    Shop,
    MadeEq,
}

public enum RingType
{
    simple,
    other,
}

public enum BulletType
{
    Player,
    Enemy,
}

public enum TipType
{
    Save,
    BuySuccessful,
    BuyFail,
    SellSuccessful,
    SellFail,
    MadeEqSuccessful,
    MadeEqFail,
}

public enum GridType
{
    isObstacle,canDig
}

