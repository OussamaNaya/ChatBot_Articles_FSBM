using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ChatBot_Articles_Prof_FSBM.Models;

public partial class Article
{
    public int Id { get; set; }

    public string UrlArticle { get; set; } = null!;

    public int FkProf { get; set; }

    [ForeignKey("FkProf")]
    public virtual Prof FkProfNavigation { get; set; } = null!;
}
