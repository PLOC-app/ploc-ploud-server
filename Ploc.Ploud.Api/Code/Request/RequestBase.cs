using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Ploc.Ploud.Library;

namespace Ploc.Ploud.Api
{
    public class RequestBase
    {
        private DateTime requestTime = DateTime.MinValue;

        public String Token { get; set; }

        public String Signature { get; set; }

        public long Timestamp { get; set; }

        public long LastSyncTime { get; set; }

        public String Device { get; set; }

        public String PublicKey { get; set; } // Taken from Appsettings.json

        [JsonIgnore]
        public DateTime RequestTime
        {
            get
            {
                this.DetermineRequestTime();
                return this.requestTime;
            }
        }

        protected virtual void DetermineRequestTime()
        {
            if (this.requestTime == DateTime.MinValue)
            {
                this.requestTime = this.Timestamp.DateTimeValue();
            }
        }

        public virtual ValidationStatus Validate()
        {
            DateTime utcNow = DateTime.UtcNow;
            this.DetermineRequestTime();
            if (this.RequestTime == DateTime.MinValue)
            {
                return ValidationStatus.InvalidTimestamp;
            }
            DateTime minTime = this.RequestTime.AddMinutes(-20);
            DateTime maxTime = this.RequestTime.AddMinutes(20);
            if (minTime > utcNow)
            {
                return ValidationStatus.InvalidTimestamp;
            }
            if (maxTime < utcNow)
            {
                return ValidationStatus.InvalidTimestamp;
            }
            if ((String.IsNullOrEmpty(this.Token))
                | (String.IsNullOrEmpty(this.Signature)))
            {
                return ValidationStatus.InvalidParams;
            }
            return ValidationStatus.Ok;
        }
    }
}
