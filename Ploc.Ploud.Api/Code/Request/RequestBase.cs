using System;
using System.Text.Json.Serialization;
using Ploc.Ploud.Library;

namespace Ploc.Ploud.Api
{
    public class RequestBase
    {
        private DateTime requestTime = DateTime.MinValue;

        public string Token { get; set; }

        public string Signature { get; set; }

        public long Timestamp { get; set; }

        public long LastSyncTime { get; set; }

        public string Device { get; set; }

        public Guid PublicKey { get; set; } // Taken from Appsettings.json

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
            
            if (string.IsNullOrEmpty(this.Token) | string.IsNullOrEmpty(this.Signature))
            {
                return ValidationStatus.InvalidParams;
            }

            return ValidationStatus.Ok;
        }
    }
}
