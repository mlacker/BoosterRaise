using GameData;

namespace BoosterRaise.Booster;

public class BoosterTemplate
{
    public uint Id { get; private set; }
    public string Name { get; private set; }
    public List<BoosterEffect> Effects { get; private set; } = new();
    public List<uint> Conditions { get; private set; } = new();

    public BoosterTemplate(BoosterImplantTemplateDataBlock source)
    {
        Id = source.persistentID;
        Name = source.PublicName;

        foreach (var item in source.Effects)
        {
            Effects.Add(new BoosterEffect(item));
        }
        foreach (var group in source.RandomEffects)
        {
            foreach (var item in group)
            {
                Effects.Add(new BoosterEffect(item));
            }
        }

        foreach (var item in source.Conditions)
        {
            Conditions.Add(item);
        }
        foreach (var item in source.RandomConditions)
        {
            Conditions.Add(item);
        }
    }
}

public class BoosterEffect
{
    public uint Id { get; private set; }
    public float MinValue { get; private set; }
    public float MaxValue { get; private set; }

    public BoosterEffect(BoosterImplantEffectInstance source)
    {
        Id = source.BoosterImplantEffect;
        MinValue = source.MinValue;
        MaxValue = source.MaxValue;
    }
}