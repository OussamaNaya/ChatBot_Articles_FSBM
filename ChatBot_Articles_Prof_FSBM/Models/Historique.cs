using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBot_Articles_Prof_FSBM.Models;

public partial class Historique
{
    public int Id { get; set; }

    [ForeignKey("Etudiant")]
    public int FkEtudiant { get; set; }

    [ForeignKey("Prof")]
    public int FkProf { get; set; }

    public string? Question { get; set; }

    public virtual Etudiant Etudiant { get; set; } = null!;
    public virtual Prof Prof { get; set; } = null!;
}
