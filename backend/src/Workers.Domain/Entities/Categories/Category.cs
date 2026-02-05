using Workers.Domain.Common;

namespace Workers.Domain.Entities.Categories;

/// <summary>
/// Represents a service category or subcategory available on the platform.
/// </summary>
public class Category : BaseEntity
{
    /// <summary>
    /// Name of the category (e.g., "Plumbing", "Sanita").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the services in this category.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// URL to an icon or illustration representing the category.
    /// </summary>
    public string? IconUrl { get; set; }

    /// <summary>
    /// Slug for URL-friendly navigation (e.g., "vodo-instalaterstvo").
    /// </summary>
    public string Slug { get; set; } = string.Empty;
    
    /// <summary>
    /// ID of the parent category if this is a subcategory.
    /// </summary>
    public Guid? ParentId { get; set; }
    public Category? Parent { get; set; }
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
}
