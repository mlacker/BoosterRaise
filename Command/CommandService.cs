using BoosterRaise.Common.Logging;
using Microsoft.Extensions.Logging;
using Player;
using SNetwork;

namespace BoosterRaise.Command
{
    public class CommandService
    {
        private readonly ILogger logger = LoggerFactory.CreateLogger<CommandService>();

        public void Dispatch(string text)
        {
            if (string.IsNullOrEmpty(text) || !text.StartsWith('/'))
                return;

            var args = new Queue<string>(text.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
            var command = args.Dequeue().Substring(1);

            switch (command)
            {
                case "save":
                    StoreCheckpoint();
                    break;
                default:
                    logger.LogWarning("Command not found, text: {}", text);
                    break;
            }
        }

        private void StoreCheckpoint()
        {
            logger.LogInformation("store checkpoint");

            var player = PlayerManager.GetLocalPlayerAgent();

            CheckpointManager.StoreCheckpoint(player.EyePosition);

            SNet.Capture.CaptureGameState(eBufferType.Checkpoint);

            PlayerChatManager.WantToSentTextMessage(player, "[System] checkpoint saved.");
        }
    }
}
