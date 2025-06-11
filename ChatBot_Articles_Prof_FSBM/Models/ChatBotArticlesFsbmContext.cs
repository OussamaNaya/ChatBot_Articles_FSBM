using Microsoft.EntityFrameworkCore;

namespace ChatBot_Articles_Prof_FSBM.Models;

public partial class ChatBotArticlesFsbmContext : DbContext
{
    public ChatBotArticlesFsbmContext()
    {
    }

    public ChatBotArticlesFsbmContext(DbContextOptions<ChatBotArticlesFsbmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Historique> Historiques { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-NT5V5HN;Database=ChatBot_Articles_FSBM;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Historique>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Historiq__3214EC0704886198");

            entity.ToTable("Historique");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
