﻿using System;
namespace Cassandra
{
    public class RoutingKey
    {
        public static RoutingKey Empty = new RoutingKey();
        public byte[] RawRoutingKey = null;
        public static RoutingKey Compose(params RoutingKey[] components)
        {
            if (components.Length == 0)
                throw new ArgumentOutOfRangeException();

            if (components.Length == 1)
                return components[0];

            int totalLength = 0;
            foreach (RoutingKey bb in components)
                totalLength += 2 + bb.RawRoutingKey.Length+1;

            byte[] res = new byte[totalLength];
            int idx = 0;
            foreach (RoutingKey bb in components)
            {
                PutShortLength(res, idx, bb.RawRoutingKey.Length);
                idx+=2;
                Buffer.BlockCopy(bb.RawRoutingKey,0,res,idx,bb.RawRoutingKey.Length);
                idx+=bb.RawRoutingKey.Length;
                res[idx]=0;
                idx++;
            }
            return new RoutingKey() { RawRoutingKey = res };
        }

        private static void PutShortLength(byte[] bb, int idx, int length)
        {
            bb[idx] = ((byte)((length >> 8) & 0xFF));
            bb[idx + 1] = ((byte)(length & 0xFF));
        }

    }
}
