using System;

namespace Ploc.Ploud.Library
{
    public sealed class SyncObjectsOptions
    {
        public SyncObjectsOptions(long timestamp, String callerId)
        {
            if(timestamp == 0)
            {
                throw new ArgumentNullException("Timestamp");
            }
            if(String.IsNullOrEmpty(callerId))
            {
                throw new ArgumentNullException("CallerId");
            }
            this.Timestamp = timestamp;
            this.CallerId = callerId;
        }

        public long Timestamp { get; private set; }

        public String CallerId { get; private set; }
    }
}
