using BoosterRaise.Common.Logging;
using GameData;
using Microsoft.Extensions.Logging;

namespace BoosterRaise.Booster;

public class BoosterService
{
    private readonly ILogger logger = LoggerFactory.CreateLogger<BoosterService>();
    private readonly Dictionary<uint, BoosterTemplate> templates = new();
    private readonly HashSet<uint> preparedInstances = new();
    private readonly HashSet<uint> overriedInstances = new();

    public void ReloadTemplates()
    {
        var templateBlocks = BoosterImplantTemplateDataBlock.GetAllBlocksForEditor();
        foreach (var item in templateBlocks)
        {
            if (item.internalEnabled)
            {
                var template = new BoosterTemplate(item);

                logger.LogTrace($"Load template {template.Id}, {template.Name}, Effects: [{string.Join(',', template.Effects.Select(it => it.Id))}], Conditions: [{string.Join(',', template.Conditions)}]");

                templates.Add(template.Id, template);
            }
        }

        logger.LogDebug($"Loaded {templates.Count} templates");
    }

    public void OnBoosterImplantInventoryChanged(BoosterImplantInventoryModel boosterImplantInventory)
    {
        var inventories = boosterImplantInventory.Categories.SelectMany(it => it.Inventory._items);

        foreach (BoosterImplantInventoryItem item in inventories)
        {
            if (null == item || null == item.Implant)
            {
                continue;
            }

            PerfectBooster(item);

            OverrideBooster(item);
        }
    }

    private void PerfectBooster(BoosterImplantInventoryItem item)
    {
        try
        {
            var implant = item.Implant;
            var template = templates[implant.TemplateId] ?? throw new InvalidOperationException("Template not found");

            implant.Effects = implant.Effects.Select(original =>
            {
                var effect = template.Effects.Single(it => original.Id == it.Id);
                return new BoosterImplant.Effect()
                {
                    Id = original.Id,
                    Value = effect.MaxValue
                };
            }).ToArray();

            if (preparedInstances.Add(item.InstanceId))
            {
                logger.LogTrace($"Perfect Booster {item.InstanceId} {template.Name}({template.Id}), Effects: {template.Effects.Count}");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Perfect Booster {} failed, {}", item.InstanceId, ex.Message);
        }
    }

    private void OverrideBooster(BoosterImplantInventoryItem item)
    {
        try
        {
            var implant = item.Implant;
            var template = templates[implant.TemplateId] ?? throw new InvalidOperationException("Template not found");

            List<BoosterImplant.Effect> effects = template.Id switch
            {
                21 => new() {
                    new() { Id = 41, Value = 1.6f }
                },
                37 => new() {
                    new() { Id = 5, Value = 2.0f },
                    new() { Id = 54, Value = 2.0f }
                },
                42 => new() {
                    new() { Id = 10, Value = 1.55f },
                    new() { Id = 11, Value = 1.55f }
                },
                _ => new(),
            };

            // ignored
            if (effects.Count == 0)
            {
                return;
            }

            implant.Effects = effects.ToArray();
            implant.Conditions = Array.Empty<uint>();

            if (overriedInstances.Add(item.InstanceId))
            {
                logger.LogTrace($"Override Booster {item.InstanceId} {template.Name}({template.Id}) {(item.Prepared ? "prepared" : string.Empty)}, Effects: {template.Effects.Count}, Conditions: {template.Conditions.Count}");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Override Booster {} failed, {}", item.InstanceId, ex.Message);
        }
    }
}