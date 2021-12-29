using Microsoft.AspNetCore.Mvc;

namespace Ploc.Ploud.Api
{
    public class DocumentRequest : RequestBase
    {
        public string Document { get; set; }
    }
}
