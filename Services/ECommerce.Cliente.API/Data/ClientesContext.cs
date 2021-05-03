using ECommerce.Core.Data;
using ECommerce.Core.DomainObjects;
using ECommerce.Core.Mediator;
using ECommerce.Core.Messages;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Cliente.API.Data
{
    public sealed class ClientesContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;

        public ClientesContext(DbContextOptions<ClientesContext> options, IMediatorHandler mediatorHandler)
            : base(options)
        {
            _mediatorHandler = mediatorHandler;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Models.Cliente> Clientes { get; set; }
        public DbSet<Models.Endereco> Enderecos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();

            //Caso eu esqueça de configurar uma propriedade do tipo string eu estou forçando que seja criado um varchar de 20
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(20)");

            //Estou forçando que o relacionamento não seja do tipo Cascate
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            //Aplicando o Map de forma generica fazendo para todos implementacoes de IEntityTypeConfiguration do Assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientesContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            var sucesso = await base.SaveChangesAsync() > 0;

            //Vou disparar eventos relacionados a este Contexto
            if (sucesso) await _mediatorHandler.PublicarEventos(this);

            return sucesso;
        }
    }

    public static class MediatorExtension
    {
        public static async Task PublicarEventos<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
        {

            //Vou pegar todas as entidades que possuem evento
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

            //Vou pegar todas as notificações que foram adicionadas
            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notificacoes)
                .ToList();

            //Vou limpar todos os eventos pois eu já peguei eles e armazenei na variavel acima
            domainEntities.ToList()
                .ForEach(entity => entity.Entity.LimparEventos());


            //Vou disparar os eventos que foram publicados
            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.PublicarEvento(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
