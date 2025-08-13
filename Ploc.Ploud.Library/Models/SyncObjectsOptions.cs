using System;

namespace Ploc.Ploud.Library
{
    public sealed class SyncObjectsOptions
    {
        public long Timestamp { get; private set; }

        public string CallerId { get; private set; }

        public SyncObjectsOptions(long timestamp, string callerId)
        {
            if (timestamp == 0)
            {
                throw new ArgumentNullException("Timestamp");
            }

            if (string.IsNullOrEmpty(callerId))
            {
                throw new ArgumentNullException("CallerId");
            }

            this.Timestamp = timestamp;
            this.CallerId = callerId;
        }
    }
}
