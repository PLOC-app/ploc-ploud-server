using Ploc.Ploud.Library;
using System;

namespace Ploc.Ploud.Api
{
    public static class SyncResponseExtensions
    {
        public static void MapUrl(this PloudObjectCollection<Document> documents, String downloadFileUrlFormat)
        {
            if ((documents == null)
                || (documents.Count == 0))
            {
                return;
            }
            foreach (Document document in documents)
            {
                document.Data = null;
                document.Url = String.Format(downloadFileUrlFormat, document.Identifier);
            }
        }
    }
}
