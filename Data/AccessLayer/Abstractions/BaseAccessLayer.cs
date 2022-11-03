namespace Data.AccessLayer.Abstractions;

using System.Linq.Expressions;

using Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

/// <summary>
///     Abstraction that provides basic CRUD operations on models data.
/// </summary>
/// <typeparam name="TContext">Data context that is using this implementation.</typeparam>
/// <typeparam name="TEntity">Object entity that is using this implementation.</typeparam>
public abstract class BaseAccessLayer<TContext, TEntity> : IBaseAccessLayer<TEntity>
        where TEntity : ADataObject
        where TContext : DbContext
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="BaseAccessLayer{TContext, TEntity}" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    protected BaseAccessLayer(TContext context)
    {
        this.Context = context;
        this.ModelSet = this.Context.Set<TEntity>();
    }

    /// <summary>
    ///     Gets the Db context.
    /// </summary>
    protected TContext Context { get; }

    /// <summary>
    ///     Gets the Db model set.
    /// </summary>
    protected DbSet<TEntity> ModelSet { get; }

    /// <inheritdoc />
    public virtual async Task<int> AddAsync(TEntity model, CancellationToken cancellationToken = default)
    {
        var result = this.ModelSet.Add(model);
        _ = await this.Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return result.Entity.Id;
    }

    /// <inheritdoc />
    public virtual async Task<int> AddRangeAsync(IEnumerable<TEntity> models, CancellationToken cancellationToken = default)
    {
        this.ModelSet.AddRange(models);

        return await this.Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public IQueryable<TEntity> GetCollection(Expression<Func<TEntity, bool>>? filter = null, bool trackingEnabled = false, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? navigationProperties = null)
    {
        var dbQuery = this.ModelSet.AsQueryable();

        if (navigationProperties != null)
            dbQuery = navigationProperties(dbQuery);

        if (filter != null)
            dbQuery = dbQuery.Where(filter);

        var collection = trackingEnabled
                                 ? dbQuery
                                 : dbQuery.AsNoTracking();

        return collection;
    }

    /// <inheritdoc />
    public IQueryable<TResult> GetCollection<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>>? filter = null, bool trackingEnabled = false,
                                                      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? navigationProperties = null)
    {
        var dbQuery = this.ModelSet.AsQueryable();

        if (navigationProperties != null)
            dbQuery = navigationProperties(dbQuery);

        if (filter is not null)
            dbQuery = dbQuery.Where(filter);

        var collection = trackingEnabled
                                 ? dbQuery.Select(selector)
                                 : dbQuery.AsNoTracking().Select(selector);

        return collection;
    }

    /// <inheritdoc />
    public Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>>? filter = null, bool trackingEnabled = false, bool asSplitQuery = false,
                                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? navigationProperties = null, CancellationToken cancellationToken = default)
    {
        var dbQuery = this.ModelSet.AsQueryable();

        if (navigationProperties != null)
            dbQuery = navigationProperties(dbQuery);

        if (filter != null)
            dbQuery = dbQuery.Where(filter);

        if (asSplitQuery)
            dbQuery = dbQuery.AsSplitQuery();

        return trackingEnabled
                       ? dbQuery.FirstOrDefaultAsync(cancellationToken)
                       : dbQuery.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public TResult? GetSingle<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>>? filter = null, bool trackingEnabled = false,
                                       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? navigationProperties = null)
    {
        var dbQuery = this.ModelSet.AsQueryable();

        if (navigationProperties != null)
            dbQuery = navigationProperties(dbQuery);

        if (filter != null)
            dbQuery = dbQuery.Where(filter);

        var item = trackingEnabled
                           ? dbQuery.Select(selector).FirstOrDefault()
                           : dbQuery.AsNoTracking().Select(selector).FirstOrDefault();

        return item;
    }

    /// <inheritdoc />
    public virtual async Task<int> UpdateAsync(TEntity model, CancellationToken cancellationToken = default)
    {
        _ = this.ModelSet.Update(model);

        return await this.Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<int> RemoveAsync(TEntity model, CancellationToken cancellationToken = default)
    {
        _ = this.ModelSet.Remove(model);

        return await this.Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<int> RemoveAsync(int id, CancellationToken cancellationToken = default)
    {
        var model = this.ModelSet.FirstOrDefault(model => model.Id == id);

        if (model == null)
            return -1;

        this.ModelSet.Remove(model);

        return await this.Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<int> RemoveRangeAsync(IEnumerable<TEntity> models, CancellationToken cancellationToken = default)
    {
        this.ModelSet.RemoveRange(models);

        return await this.Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<int> RemoveRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        this.ModelSet.RemoveRange(this.ModelSet.Where(model => ids.Contains(model.Id)));

        return await this.Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        => this.GetCollection(filter: o => o.Id == id).AnyAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<int> UpdateListAsync(IEnumerable<TEntity> models, CancellationToken cancellationToken = default)
    {
        this.ModelSet.UpdateRange(models);

        return await this.Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
