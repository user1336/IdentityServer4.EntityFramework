// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.EntityFramework.Services
{
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly ILogger<CorsPolicyService> _logger;

        public CorsPolicyService(IServiceScopeFactory scopeFactory, ILogger<CorsPolicyService> logger)
        {
            Context = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IConfigurationDbContext>();
            _logger = logger;
        }

        private IConfigurationDbContext Context { get; }

        public Task<bool> IsOriginAllowedAsync(string origin)
        {
                var origins = Context.Clients.SelectMany(x => x.AllowedCorsOrigins.Select(y => y.Origin)).ToList();

                var distinctOrigins = origins.Where(x => x != null).Distinct();

                var isAllowed = distinctOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase);

                _logger.LogDebug("Origin {origin} is allowed: {originAllowed}", origin, isAllowed);

                return Task.FromResult(isAllowed);
            }
        
    }
}