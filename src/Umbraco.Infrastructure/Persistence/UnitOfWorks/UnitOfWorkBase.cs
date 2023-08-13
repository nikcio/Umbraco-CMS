using System.Data;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.DistributedLocking;
using Umbraco.Cms.Core.Persistence;
using Umbraco.Cms.Core.Persistence.Repositories;
using Umbraco.Cms.Core.Scoping;

namespace Umbraco.Cms.Infrastructure.Persistence.UnitOfWorks;

/// <summary>
/// Represents a unit of work for a specific <see cref="IDatabaseRepository"/>
/// </summary>
internal abstract class UnitOfWorkBase : IDatabaseUnitOfWork
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWorkBase"/> class.
    /// </summary>
    /// <param name="loggerFactory"></param>
    /// <param name="lockingMechanismFactory"></param>
    protected UnitOfWorkBase(ILoggerFactory loggerFactory, IDistributedLockingMechanismFactory lockingMechanismFactory)
    {
        LockingMechanism = new LockingMechanism(lockingMechanismFactory, loggerFactory.CreateLogger<LockingMechanism>());
        InstanceId = Guid.NewGuid();
    }

    /// <summary>
    /// Gets unique id for this unit of work instance.
    /// </summary>
    protected Guid InstanceId { get; }

    /// <summary>
    /// Gets the locking mechanism.
    /// </summary>
    protected ILockingMechanism LockingMechanism { get; }

    /// <inheritdoc/>
    public abstract void Complete();

    /// <inheritdoc/>
    public void ReadLock(params int[] lockIds) => LockingMechanism.ReadLock(InstanceId, lockIds);

    /// <inheritdoc/>
    public void ReadLock(TimeSpan timeout, params int[] lockIds) => LockingMechanism.ReadLock(InstanceId, timeout, lockIds);

    /// <inheritdoc/>
    public abstract void Start(IsolationLevel? isolationLevel = null);

    /// <inheritdoc/>
    public void WriteLock(params int[] lockIds) => LockingMechanism.WriteLock(InstanceId, lockIds);

    /// <inheritdoc/>
    public void WriteLock(TimeSpan timeout, params int[] lockIds) => LockingMechanism.WriteLock(InstanceId, timeout, lockIds);
}
