namespace ChatBot_Articles_Prof_FSBM.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Libelle { get; set; } = null!;

    public virtual ICollection<Etudiant> Etudiants { get; set; } = new List<Etudiant>();
}
