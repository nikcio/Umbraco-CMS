using System;
using System.Collections.Generic;

namespace Umbraco.Cms.Persistence.EFCore.Models;

public class CmsPropertyTypeGroup
{
    public int Id { get; set; }

    public Guid UniqueId { get; set; }

    public int ContenttypeNodeId { get; set; }

    public int Type { get; set; }

    public string Text { get; set; } = null!;

    public string Alias { get; set; } = null!;

    public int Sortorder { get; set; }

    public virtual ICollection<CmsPropertyType> CmsPropertyTypes { get; set; } = new List<CmsPropertyType>();

    public virtual CmsContentType ContenttypeNode { get; set; } = null!;
}
