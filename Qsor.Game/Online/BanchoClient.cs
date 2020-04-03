using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.IO.Network;
using Qsor.Game.Online.Bancho;

namespace Qsor.Game.Online
{
    public enum ConnectionStatus
    {
        Disconnected,
        Connecting,
        Connected
    }
    
    public class BanchoClient : Component
    {
        public const string WEB_URL = "https://akatsuki.pw";
        public const string AVATAR_URL = "https://a.akatsuki.pw";
        public const string BANCHO_URL = "https://c.akatsuki.pw";
        public const int PROTOCOL_VERSION = 19; // 20 will support websockets

        public Bindable<string> BindableToken { get; } = new Bindable<string>();
        public Bindable<ConnectionStatus> BindableConnectionStatus { get; } = new Bindable<ConnectionStatus>();
        private readonly BanchoStreamWriter _packetStream = BanchoStreamWriter.New();
        
        public async void Connect()
        {
            if (BindableConnectionStatus.Value == ConnectionStatus.Connected ||
                BindableConnectionStatus.Value == ConnectionStatus.Connecting)
                return;

            BindableConnectionStatus.Value = ConnectionStatus.Connecting;
            
            await PushPackets();
        }

        public void PushPacket(IPacket packet)
        {
            _packetStream.Write(packet);
        }

        // We push multiple packets at once.
        public async Task PushPackets()
        {
            var wr = new WebRequest(BANCHO_URL) { Method = HttpMethod.Post };
            if (BindableToken.Value != null)
                wr.AddHeader("osu-token", BindableToken.Value);

            if (BindableToken.Value != null && BindableConnectionStatus.Value == ConnectionStatus.Disconnected)
            { // in case we're disconnected.
                BindableToken.Value = null;
                Connect();
            }
            
            wr.AddRaw(_packetStream.BaseStream);
            
            await wr.PerformAsync();

            if (wr.Aborted) // Token not found, Timeout or what not. retry!
                BindableConnectionStatus.Value = ConnectionStatus.Disconnected;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
                _packetStream.Dispose();
        }
    }
}