using Ploc.Ploud.Library;

namespace Ploc.Ploud.Api
{
    public static class SyncResponseExtensions
    {
        public static void MapUrl(this PloudObjectCollection<Document> documents, string downloadFileUrlFormat)
        {
            if (documents == null || documents.Count == 0)
            {
                return;
            }

            foreach (Document document in documents)
            {
                document.Data = null;
                document.Url = string.Format(downloadFileUrlFormat, document.Identifier);
            }
        }
    }
}
