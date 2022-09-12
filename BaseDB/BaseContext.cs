using Entities.DBModels.AccountModels;
using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.AppInfoModels;
using Entities.DBModels.DashboardAdministrationModels;
using Entities.DBModels.LocationModels;
using Entities.DBModels.NewsModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.PrivateLeagueModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.SharedModels;
using Entities.DBModels.SponsorModels;
using Entities.DBModels.TeamModels;
using Entities.DBModels.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using ModelBuilderConfig.Configurations.AccountModels;
using ModelBuilderConfig.Configurations.AccountTeamModels;
using ModelBuilderConfig.Configurations.AppInfoModels;
using ModelBuilderConfig.Configurations.DashboardAdministrationModels;
using ModelBuilderConfig.Configurations.PlayerScoreModels;
using ModelBuilderConfig.Configurations.PrivateLeagueModels;
using ModelBuilderConfig.Configurations.SeasonModels;
using ModelBuilderConfig.Configurations.SponsorModels;
using ModelBuilderConfig.Configurations.UserModels;

namespace BaseDB
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions options) : base(options)
        {
        }

        #region UserModels

        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Verification> Verifications { get; set; }

        #endregion

        #region DashboardAdministrationModels

        public DbSet<AdministrationRolePremission> AdministrationRolePremissions { get; set; }
        public DbSet<DashboardAccessLevel> DashboardAccessLevels { get; set; }
        public DbSet<DashboardAdministrationRole> DashboardAdministrationRoles { get; set; }
        public DbSet<DashboardAdministrator> DashboardAdministrators { get; set; }
        public DbSet<DashboardView> DashboardViews { get; set; }

        #endregion

        #region AppInfoModels
        public DbSet<AppAbout> AppAbout { get; set; }
        #endregion

        #region LocationModels
        public DbSet<Country> Countries { get; set; }
        #endregion

        #region AccountModels
        public DbSet<Account> Accounts { get; set; }
        #endregion

        #region SponsorModels
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<SponsorView> SponsorViews { get; set; }
        #endregion

        #region NewsModels
        public DbSet<News> News { get; set; }
        public DbSet<NewsAttachment> NewsAttachments { get; set; }
        #endregion

        #region TeamModels
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerPosition> PlayerPositions { get; set; }
        public DbSet<PlayerPrice> PlayerPrices { get; set; }

        #endregion

        #region SeasonModels
        public DbSet<Season> Seasons { get; set; }
        public DbSet<GameWeak> GameWeaks { get; set; }
        public DbSet<TeamGameWeak> TeamGameWeaks { get; set; }
        #endregion

        #region PlayerScoreModels
        public DbSet<ScoreType> ScoreTypes { get; set; }
        public DbSet<PlayerGameWeak> PlayerGameWeaks { get; set; }
        public DbSet<PlayerGameWeakScore> PlayerGameWeakScores { get; set; }
        #endregion

        #region AccountTeamModels
        public DbSet<AccountTeam> AccountTeams { get; set; }
        public DbSet<AccountTeamPlayer> AccountTeamPlayers { get; set; }
        public DbSet<AccountTeamGameWeak> AccountTeamGameWeaks { get; set; }
        public DbSet<TeamPlayerType> TeamPlayerTypes { get; set; }
        public DbSet<AccountTeamPlayerGameWeak> AccountTeamPlayerGameWeaks { get; set; }

        #endregion

        #region PrivateLeagueModels
        public DbSet<PrivateLeague> PrivateLeagues { get; set; }
        public DbSet<PrivateLeagueMember> PrivateLeagueMembers { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes()
               .Where(t => t.ClrType.IsSubclassOf(typeof(BaseEntity))))
            {
                _ = modelBuilder.Entity(
                    entityType.Name,
                    x =>
                    {
                        _ = x.Property("CreatedAt")
                            .HasDefaultValueSql("getutcdate()");
                    });
            }

            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes()
               .Where(t => t.ClrType.IsSubclassOf(typeof(AuditEntity))))
            {
                _ = modelBuilder.Entity(
                    entityType.Name,
                    x =>
                    {
                        _ = x.Property("LastModifiedAt")
                            .HasDefaultValueSql("getutcdate()");
                    });
            }

            #region UserModels
            _ = modelBuilder.ApplyConfiguration(new UserConfiguration());
            #endregion

            #region DashboardAdministrationModels

            _ = modelBuilder.ApplyConfiguration(new DashboardAccessLevelConfiguration());
            _ = modelBuilder.ApplyConfiguration(new DashboardAccessLevelLangConfiguration());
            _ = modelBuilder.ApplyConfiguration(new DashboardAdministrationRoleConfiguration());
            _ = modelBuilder.ApplyConfiguration(new DashboardAdministrationRoleLangConfiguration());
            _ = modelBuilder.ApplyConfiguration(new AdministrationRolePremissionConfiguration());
            _ = modelBuilder.ApplyConfiguration(new DashboardAdministratorConfiguration());

            #endregion

            #region AppInfoModels
            _ = modelBuilder.ApplyConfiguration(new AppAboutConfiguration());
            _ = modelBuilder.ApplyConfiguration(new AppAboutLangConfiguration());
            #endregion

            #region AccountModels
            _ = modelBuilder.ApplyConfiguration(new AccountConfiguration());
            #endregion

            #region SponsorModels
            _ = modelBuilder.ApplyConfiguration(new SponsorViewConfiguration());
            #endregion

            #region SeasonModels
            _ = modelBuilder.ApplyConfiguration(new GameWeakConfiguration());
            _ = modelBuilder.ApplyConfiguration(new TeamGameWeakConfiguration());
            #endregion

            #region PlayerScoreModels
            _ = modelBuilder.ApplyConfiguration(new PlayerGameWeakConfiguration());
            _ = modelBuilder.ApplyConfiguration(new PlayerGameWeakScoreConfiguration());
            #endregion

            #region AccountTeamModels
            _ = modelBuilder.ApplyConfiguration(new AccountTeamConfiguration());
            _ = modelBuilder.ApplyConfiguration(new AccountTeamGameWeakConfiguration());
            _ = modelBuilder.ApplyConfiguration(new AccountTeamPlayerConfiguration());
            _ = modelBuilder.ApplyConfiguration(new AccountTeamPlayerGameWeakConfiguration());
            #endregion

            #region PrivateLeagueModels
            _ = modelBuilder.ApplyConfiguration(new PrivateLeagueMemberConfiguration());
            #endregion
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(
           bool acceptAllChangesOnSuccess,
           CancellationToken cancellationToken = default
        )
        {
            OnBeforeSaving();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess,
                          cancellationToken);
        }

        private void OnBeforeSaving()
        {
            IEnumerable<EntityEntry> entries = ChangeTracker.Entries();
            DateTime utcNow = DateTime.UtcNow;

            foreach (EntityEntry entry in entries)
            {
                if (entry.Entity is BaseEntity basetrackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.Property(nameof(BaseEntity.CreatedAt)).IsModified = false;
                            break;

                        case EntityState.Added:
                            basetrackable.CreatedAt = utcNow;
                            break;
                    }
                }
                else if (entry.Entity is AuditEntity audittrackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            audittrackable.LastModifiedAt = utcNow;
                            entry.Property(nameof(AuditEntity.CreatedAt)).IsModified = false;
                            break;

                        case EntityState.Added:
                            audittrackable.CreatedAt = utcNow;
                            audittrackable.LastModifiedAt = utcNow;
                            break;
                    }
                }

            }
        }
    }
}