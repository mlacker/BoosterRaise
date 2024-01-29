using BoosterRaise.Common.Logging;
using GameData;
using Microsoft.Extensions.Logging;

namespace BoosterRaise.Booster;

public class BoosterService
{
    private readonly ILogger logger = LoggerFactory.CreateLogger<BoosterService>();
    private readonly Dictionary<uint, BoosterTemplate> templates = new();

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
        }
    }

    private void PerfectBooster(BoosterImplantInventoryItem item)
    {
        try
        {
            var implant = item.Implant;
            var template = templates[implant.TemplateId] ?? throw new InvalidOperationException("Template not found");

            logger.LogTrace($"Perfect Booster {item.InstanceId} {template.Name}({template.Id}), Effects: {template.Effects.Count}");

            implant.Effects = implant.Effects.Select(original =>
            {
                var effect = template.Effects.Single(it => original.Id == it.Id);
                return new BoosterImplant.Effect()
                {
                    Id = original.Id,
                    Value = effect.MaxValue
                };
            }).ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Perfect Booster {} failed, {}", item.InstanceId, ex.Message);
        }
    }
}