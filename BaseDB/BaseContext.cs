using Entities.DBModels.AccountModels;
using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.AppInfoModels;
using Entities.DBModels.AuditModels;
using Entities.DBModels.DashboardAdministrationModels;
using Entities.DBModels.LocationModels;
using Entities.DBModels.LogModels;
using Entities.DBModels.NewsModels;
using Entities.DBModels.NotificationModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.PlayerStateModels;
using Entities.DBModels.PlayersTransfersModels;
using Entities.DBModels.PrivateLeagueModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.SharedModels;
using Entities.DBModels.SponsorModels;
using Entities.DBModels.StandingsModels;
using Entities.DBModels.SubscriptionModels;
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
using ModelBuilderConfig.Configurations.PlayerStateModels;
using ModelBuilderConfig.Configurations.PrivateLeagueModels;
using ModelBuilderConfig.Configurations.SeasonModels;
using ModelBuilderConfig.Configurations.SponsorModels;
using ModelBuilderConfig.Configurations.StandingsModels;
using ModelBuilderConfig.Configurations.UserModels;
using Newtonsoft.Json;

namespace BaseDB
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions options) : base(options)
        {
        }

        #region LogModels

        public DbSet<Log> Logs { get; set; }

        #endregion

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
        public DbSet<AccountSubscription> AccountSubscriptions { get; set; }
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

        #region PlayersTransfersModels
        public DbSet<PlayerTransfer> PlayerTransfers { get; set; }
        #endregion

        #region StandingsModels
        public DbSet<Standings> Standings { get; set; }

        #endregion

        #region SubscriptionModels
        public DbSet<Subscription> Subscriptions { get; set; }
        #endregion

        #region PlayerStateModels

        public DbSet<ScoreState> ScoreStates { get; set; }
        public DbSet<PlayerSeasonScoreState> PlayerSeasonScoreStates { get; set; }
        public DbSet<PlayerGameWeakScoreState> PlayerGameWeakScoreStates { get; set; }

        #endregion

        #region PlayerStateModels
        public DbSet<Payment> Payments { get; set; }
        #endregion

        #region Audit Models
        public DbSet<Audit> Audits { get; set; }
        public DbSet<JobAudit> JobAudits { get; set; }
        #endregion

        #region Notification
        public DbSet<Notification> Notifications { get; set; }
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
            _ = modelBuilder.ApplyConfiguration(new AccountSubscriptionConfiguration());
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
            _ = modelBuilder.ApplyConfiguration(new TeamPlayerTypeConfiguration());
            _ = modelBuilder.ApplyConfiguration(new TeamPlayerTypeLangConfiguration());

            #endregion

            #region PrivateLeagueModels
            _ = modelBuilder.ApplyConfiguration(new PrivateLeagueMemberConfiguration());
            #endregion

            #region StandingsModels
            _ = modelBuilder.ApplyConfiguration(new StandingsConfiguration());
            #endregion

            #region PlayerStateModels

            _ = modelBuilder.ApplyConfiguration(new ScoreStateConfiguration());
            _ = modelBuilder.ApplyConfiguration(new ScoreStateLangConfiguration());

            _ = modelBuilder.ApplyConfiguration(new PlayerGameWeakScoreStateConfiguration());
            _ = modelBuilder.ApplyConfiguration(new PlayerSeasonScoreStateConfiguration());

            #endregion
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken: cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(
           bool acceptAllChangesOnSuccess,
           CancellationToken cancellationToken = default
        )
        {
            List<AuditEntry> auditEntries = OnBeforeSaving();
            int result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await OnAfterSaving(auditEntries);
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess,
                         cancellationToken);
        }

        private List<AuditEntry> OnBeforeSaving()
        {
            IEnumerable<EntityEntry> entries = ChangeTracker.Entries();
            DateTime utcNow = DateTime.UtcNow;

            foreach (EntityEntry entry in entries)
            {
                if (entry.Entity is AuditEntity audittrackable)
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
                else if(entry.Entity is BaseEntity basetrackable)
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
            }

            ChangeTracker.DetectChanges();
            List<AuditEntry> auditEntries = new();
            foreach (EntityEntry entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                AuditEntry auditEntry = new(entry)
                {
                    TableName = entry.Metadata.GetTableName() // EF Core 3.1: entry.Metadata.GetTableName();
                };
                if (entry.Properties.Any(a => a.Metadata.Name == "LastModifiedBy") &&
                      entry.Properties.Where(a => a.Metadata.Name == "LastModifiedBy").FirstOrDefault().CurrentValue != null)
                {
                    auditEntry.CreatedBy = entry.Properties.Where(a => a.Metadata.Name == "LastModifiedBy").FirstOrDefault().CurrentValue.ToString();
                }
                else if (entry.Properties.Any(a => a.Metadata.Name == "CreatedBy") &&
                  entry.Properties.Where(a => a.Metadata.Name == "CreatedBy").FirstOrDefault().CurrentValue != null)
                {
                    auditEntry.CreatedBy = entry.Properties.Where(a => a.Metadata.Name == "CreatedBy").FirstOrDefault().CurrentValue.ToString();
                }

                auditEntries.Add(auditEntry);

                foreach (PropertyEntry property in entry.Properties)
                {
                    // The following condition is ok with EF Core 2.2 onwards.
                    // If you are using EF Core 2.1, you may need to change the following condition to support navigation properties: https://github.com/dotnet/efcore/issues/17700
                    // if (property.IsTemporary || (entry.State == EntityState.Added && property.Metadata.IsForeignKey()))
                    if (property.IsTemporary)
                    {
                        // value will be generated by the database, get the value after saving
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;

                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }

            // Save audit entities that have all the modifications
            foreach (AuditEntry auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                _ = Audits.Add(auditEntry.ToAudit());
            }

            // keep a list of entries where the value of some properties are unknown at this step
            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }

        private Task OnAfterSaving(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
            {
                return Task.CompletedTask;
            }

            foreach (AuditEntry auditEntry in auditEntries)
            {
                // Get the final value of the temporary properties
                foreach (PropertyEntry prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }

                // Save the Audit entry
                _ = Audits.Add(auditEntry.ToAudit());
            }

            return SaveChangesAsync();
        }

        public class AuditEntry
        {
            public AuditEntry(EntityEntry entry)
            {
                Entry = entry;
            }

            public EntityEntry Entry { get; }
            public string TableName { get; set; }
            public string CreatedBy { get; set; }
            public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
            public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
            public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
            public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

            public bool HasTemporaryProperties => TemporaryProperties.Any();

            public Audit ToAudit()
            {
                Audit audit = new()
                {
                    TableName = TableName,
                    CreatedBy = CreatedBy,
                    CreatedAt = DateTime.UtcNow,
                    KeyValues = JsonConvert.SerializeObject(KeyValues),
                    OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues),
                    NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues)
                };
                return audit;
            }
        }
    }
}