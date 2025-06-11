using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBot_Articles_Prof_FSBM.Models;

public partial class Etudiant
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public int FkRole { get; set; }

    [ForeignKey("FkRole")]
    public virtual Role FkRoleNavigation { get; set; } = null!;

    public virtual ICollection<Historique> Historiques { get; set; } = new List<Historique>();
}
