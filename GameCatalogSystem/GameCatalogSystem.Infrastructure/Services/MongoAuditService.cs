using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCatalogSystem.Application.Services.Interfaces;
using GameCatalogSystem.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace GameCatalogSystem.Infrastructure.Services;

public class MongoAuditService : IAuditService
{
    private readonly IMongoCollection<AuditLog> _auditLogs;

    public MongoAuditService(IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("MongoDbSettings:ConnectionString").Value;
        var databaseName = configuration.GetSection("MongoDbSettings:DatabaseName").Value;

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        _auditLogs = database.GetCollection<AuditLog>("AuditLogs");
    }

    public async Task LogAsync(string action, string entityName, string entityId, string details)
    {
        var log = new AuditLog
        {
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            Details = details
        };

        await _auditLogs.InsertOneAsync(log);
    }
}