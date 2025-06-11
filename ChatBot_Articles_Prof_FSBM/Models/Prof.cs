namespace ChatBot_Articles_Prof_FSBM.Models;

public partial class Prof
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual ICollection<Historique> Historiques { get; set; } = new List<Historique>();
}
