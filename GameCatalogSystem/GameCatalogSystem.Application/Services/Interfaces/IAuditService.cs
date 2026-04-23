using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogSystem.Application.Services.Interfaces;

public interface IAuditService
{
    Task LogAsync(string action, string entityName, string entityId, string details);
}
