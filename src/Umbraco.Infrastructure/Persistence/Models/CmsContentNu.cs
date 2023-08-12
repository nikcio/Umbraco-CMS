namespace Umbraco.Cms.Infrastructure.Persistence.Models;

public class CmsContentNu
{
    public int NodeId { get; set; }

    public bool Published { get; set; }

    public string? Data { get; set; }

    public long Rv { get; set; }

    public byte[]? DataRaw { get; set; }

    public virtual UmbracoContent Node { get; set; } = null!;
}
