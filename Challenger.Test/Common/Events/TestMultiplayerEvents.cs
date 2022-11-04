using System;
using StardewModdingAPI.Events;

namespace ChallengerTest.Common.Events; 

public class TestMultiplayerEvents : IMultiplayerEvents {
    public event EventHandler<PeerContextReceivedEventArgs>? PeerContextReceived;
    public event EventHandler<PeerConnectedEventArgs>? PeerConnected;
    public event EventHandler<ModMessageReceivedEventArgs>? ModMessageReceived;
    public event EventHandler<PeerDisconnectedEventArgs>? PeerDisconnected;
}