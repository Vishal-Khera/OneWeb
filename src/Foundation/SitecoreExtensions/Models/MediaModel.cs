using OneWeb.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace OneWeb.Foundation.SitecoreExtensions.Models
{
    public class MediaModel
    {
        public MediaModel()
        {
        }

        public MediaModel(Item item, ID fieldId)
        {
            var mediaField = (ImageField)item.Fields[fieldId];
            if (mediaField != null) GetModel(mediaField.MediaItem);
        }

        public MediaModel(Item item, string fieldName)
        {
            var mediaField = (ImageField)item.Fields[fieldName];
            if (mediaField != null) GetModel(mediaField.MediaItem);

            var internalField = (InternalLinkField)item.Fields[fieldName];
            if (internalField != null && internalField.TargetItem.IsMediaItem())
            {
                GetModel(internalField.TargetItem);
            }
        }
        
        public MediaModel(string mediaId)
        {
            var mediaItem = ItemExtensions.GetItem(mediaId);
            if (mediaItem != null && mediaItem.IsMediaItem())
            {
                GetModel(mediaItem);
            }
        }

        public MediaModel(Item mediaItem)
        {
            if (mediaItem != null && mediaItem.IsMediaItem())
            {
                GetModel(mediaItem);
            }
        }

        public string Url { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public string Alt { get; set; }

        public string MediaType { get; set; }
        public string MediaExtension { get; set; }

        private void GetModel(MediaItem mediaItem)
        {
            if (mediaItem == null) return;

            Url = MediaExtensions.GetMediaUrl(mediaItem);
            Description = mediaItem.Description;
            Title = mediaItem.Title;
            Alt = mediaItem.Alt;
            MediaType = mediaItem.MimeType;
            MediaExtension = mediaItem.Extension;
        }
    }
}