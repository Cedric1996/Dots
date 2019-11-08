using Unity.Entities;
using Unity.DotsNetKit.Transport.Utilities;

namespace Unity.DotsNetKit.NetCode
{
    // For each connection, there is one NetworkSnapshotAckComponent on the client-side,
    // and one NetworkSnapshotAckComponent on the server side.
    // 
    // The server uses:
    //      - LastReceivedSnapshotByRemote      unit: tick/frame (server)
    //      - ReceivedSnapshotByRemoteMaskX     256bits, last 256 snapshots, high bit old, low bit new
    //      - LastReceivedRemoteTime            unit: milliseconds (client time)
    //      - LastReceivedRTT                   unit: milliseconds
    //      - LastReceiveTimestamp              unit: milliseconds (server time)
    // The client uses:
    //      - LastReceivedSnapshotByLocal       unit: tick/frame (server)
    //      - ReceivedSnapshotByLocalMask       32bits, last 32 snapshots, high bit old, low bit new
    //      - LastReceivedRemoteTime            unit: milliseconds (server time)
    //      - LastReceivedRTT                   unit: milliseconds
    //      - LastReceiveTimestamp              unit: milliseconds (client time)
    public struct NetworkSnapshotAckComponent : IComponentData
    {
        // Only updated on the server-side, when receiving commands from client.
        public void UpdateReceivedByRemote(uint tick, uint mask)
        {
            if (LastReceivedSnapshotByRemote == 0)
            {
                ReceivedSnapshotByRemoteMask0 = mask;
                LastReceivedSnapshotByRemote = tick;        // server tick
            }
            else if (SequenceHelpers.IsNewer(tick, LastReceivedSnapshotByRemote))
            {
                // TODO: this assumes the delta between acks is less than 64
                int shamt = (int)(tick - LastReceivedSnapshotByRemote);
                ReceivedSnapshotByRemoteMask3 = (ReceivedSnapshotByRemoteMask3 << shamt) |
                                                (ReceivedSnapshotByRemoteMask2 >> (64 - shamt));
                ReceivedSnapshotByRemoteMask2 = (ReceivedSnapshotByRemoteMask2 << shamt) |
                                                (ReceivedSnapshotByRemoteMask1 >> (64 - shamt));
                ReceivedSnapshotByRemoteMask1 = (ReceivedSnapshotByRemoteMask1 << shamt) |
                                                (ReceivedSnapshotByRemoteMask0 >> (64 - shamt));
                ReceivedSnapshotByRemoteMask0 = (ReceivedSnapshotByRemoteMask0 << shamt) |
                                                mask;
                LastReceivedSnapshotByRemote = tick;
            }
        }

        public bool IsReceivedByRemote(uint tick)
        {
            if (tick == 0 || LastReceivedSnapshotByRemote == 0)
                return false;
            if (SequenceHelpers.IsNewer(tick, LastReceivedSnapshotByRemote))
                return false;
            int bit = (int)(LastReceivedSnapshotByRemote - tick);
            if (bit >= 256)
                return false;
            if (bit >= 192)
            {
                bit -= 192;
                return (ReceivedSnapshotByRemoteMask3 & (1ul << bit)) != 0;
            }
            if (bit >= 128)
            {
                bit -= 128;
                return (ReceivedSnapshotByRemoteMask2 & (1ul << bit)) != 0;
            }
            if (bit >= 64)
            {
                bit -= 64;
                return (ReceivedSnapshotByRemoteMask1 & (1ul << bit)) != 0;
            }
            return (ReceivedSnapshotByRemoteMask0 & (1ul << bit)) != 0;
        }
        public uint LastReceivedSnapshotByRemote;       // unit : server tick
        public ulong ReceivedSnapshotByRemoteMask0;
        public ulong ReceivedSnapshotByRemoteMask1;
        public ulong ReceivedSnapshotByRemoteMask2;
        public ulong ReceivedSnapshotByRemoteMask3;
        public uint LastReceivedSnapshotByLocal;
        public uint ReceivedSnapshotByLocalMask;

        // This is updated on both server and client side, used to track RTT on both sides.
        // It is updated when receiving command or snapshot.
        //
        // public void UpdateRemoteTime(uint remoteTime, uint localTimeMinusRTT, uint localTime)
        // TODO: LZ:
        //      to be confirmed: fix RTT calculation:
        //      A                           B
        //      t0: A send time 
        //                                  T0: B receive time
        //                                  T1: B send time
        //      t1: A receive time
        public void UpdateRemoteTime(uint remoteTime, uint localSentPlusRomoteProcess, uint localTime)
        {
            if (remoteTime != 0 && SequenceHelpers.IsNewer(remoteTime, LastReceivedRemoteTime))
            {
                LastReceivedRemoteTime = remoteTime;
                LastReceivedRTT = localTime - localSentPlusRomoteProcess; // t1 - (t0 + (T1 - T0))
                LastReceiveTimestamp = localTime;
            }
        }
        public uint LastReceivedRemoteTime; // unit : milliseconds
        public uint LastReceivedRTT;        // unit : milliseconds
        public uint LastReceiveTimestamp;   // unit : milliseconds
    }
}
