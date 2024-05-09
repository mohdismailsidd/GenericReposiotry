using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Reflection;

namespace DataAccessLayer.DBContexts
{
    public abstract class DBContextBase<TContext> : DbContext
    {
        /// <summary>
        /// Abstract method to configure implement our database connection.
        /// <para>Example:</para>
        /// <para>optionsBuilder.UseSqlServer(connectionString);</para>
        /// </summary>
        /// <param name="optionsBuilder">Model building options.</param>
        public abstract void DatabaseConfig(DbContextOptionsBuilder optionsBuilder);

        /// <summary>
        /// Virtual method to ignore specific entities from our context.
        /// <para>Example:</para>
        /// <para>modelBuilder.Ignore&lt;AspNetUsers&gt;();</para>
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder instance.</param>
        public virtual void IgnoreEntities(ModelBuilder modelBuilder)
        {
            //No ignored entities by default.
        }

        /// <summary>
        /// overridden method to configure the database connection conventions.
        /// </summary>
        /// <param name="configurationBuilder"></param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
        }

        /// <summary>
        /// overridden method to configure the database connection.
        /// </summary>
        /// <param name="optionsBuilder">Model building options.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Abstract method to map our entities using reflection.
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder instance.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Base ModelBuilder

            try
            {
                base.OnModelCreating(modelBuilder);
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }

            #endregion

            #region Ignoring specific entities

            IgnoreEntities(modelBuilder);

            #endregion

            #region Generic model building

            // Interface of our entities.
            var mappingInterface = typeof(IEntityTypeConfiguration<>);

            // Entity types to be mapped.
            var mappingTypes = typeof(TContext).GetTypeInfo()
                                               .Assembly.GetTypes()
                                               .Where(x => x.GetInterfaces()
                                               .Any(y => y.GetTypeInfo().IsGenericType &&
                                                         y.GetGenericTypeDefinition() == mappingInterface));

            // ModelBuilder's generic method.
            var entityMethod = typeof(ModelBuilder).GetMethods()
                                                   .Single(x => x.Name == "Entity" &&
                                                                x.IsGenericMethod &&
                                                                x.ReturnType.Name == "EntityTypeBuilder`1");

            foreach (var mappingType in mappingTypes)
            {
                try
                {
                    // Entity type to be mapped.
                    var genericTypeArg = mappingType.GetInterfaces().Single().GenericTypeArguments.Single();

                    // builder.Entity<TEntity> method.
                    var genericEntityMethod = entityMethod.MakeGenericMethod(genericTypeArg);

                    // Calling builder.Entity<TEntity> to obtain the model builder of our entity.
                    var entityBuilder = genericEntityMethod.Invoke(modelBuilder, null);

                    // Creating a new mapping instance.
                    var mapper = Activator.CreateInstance(mappingType);

                    //Invokes the "Configure" method of each entity's mapping class.
                    mapper.GetType().GetMethod("Configure")?.Invoke(mapper, new[] { entityBuilder });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debugger.Break();
                    throw;
                }
            }

            #endregion
        }
    }
}
