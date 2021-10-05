// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using FreeCourse.Shared.Constants;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace FreeCourse.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[] {
            new ApiResource(CustomIdentityServerConstants.resource_catalog){Scopes={ CustomIdentityServerConstants.catalog_fullpermission}},
             new ApiResource(CustomIdentityServerConstants.resource_photo_stock){Scopes={ CustomIdentityServerConstants.photo_stock_fullpermission}},
                new ApiResource(CustomIdentityServerConstants.resource_basket){Scopes={ CustomIdentityServerConstants.basket_fullpermission}},
                         new ApiResource(CustomIdentityServerConstants.resource_discount){Scopes={ CustomIdentityServerConstants.discount_fullpermission}},
                          new ApiResource(CustomIdentityServerConstants.resource_order){Scopes={ CustomIdentityServerConstants.order_fullpermission}},
                            new ApiResource(CustomIdentityServerConstants.resource_payment){Scopes={ CustomIdentityServerConstants.payment_fullpermission}},
                              new ApiResource(CustomIdentityServerConstants.resource_gateaway){Scopes={ CustomIdentityServerConstants.gateaway_fullpermission}},
                                 new ApiResource(IdentityServerConstants.LocalApi.ScopeName)

        };
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource(){Name="roles",DisplayName="Roles",Description="Kullanıcı Rolleri",UserClaims=new[]{"role"} }

                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
               new ApiScope(CustomIdentityServerConstants.catalog_fullpermission,"Catalog API için full erişim"),
                new ApiScope(CustomIdentityServerConstants.photo_stock_fullpermission,"Photo Stock API için full erişim"),
                new ApiScope(CustomIdentityServerConstants.basket_fullpermission,"Basket API için full erişim"),
                 new ApiScope(CustomIdentityServerConstants.discount_fullpermission,"Discount API için full erişim"),
                  new ApiScope(CustomIdentityServerConstants.order_fullpermission,"Order API için full erişim"),
                   new ApiScope(CustomIdentityServerConstants.payment_fullpermission,"Payment API için full erişim"),
                     new ApiScope(CustomIdentityServerConstants.gateaway_fullpermission,"Gateway için full erişim"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientName="Asp.Net Core MVC",
                    ClientId=CustomIdentityServerConstants.WebMvc_ClientId,
                    ClientSecrets= {new Secret(CustomIdentityServerConstants.WebMvc_ClientSecrets.Sha256())},
                    AllowedGrantTypes= GrantTypes.ClientCredentials,
                    AllowedScopes={ CustomIdentityServerConstants.catalog_fullpermission,CustomIdentityServerConstants.gateaway_fullpermission, CustomIdentityServerConstants.photo_stock_fullpermission, IdentityServerConstants.LocalApi.ScopeName }
                },
                 new Client
                {
                    ClientName="Asp.Net Core MVC",
                    ClientId=CustomIdentityServerConstants.WebMvc_clientForUser,
                    AllowOfflineAccess=true,
                    ClientSecrets= {new Secret(CustomIdentityServerConstants.WebMvc_ClientSecrets.Sha256())},
                    AllowedGrantTypes= GrantTypes.ResourceOwnerPassword,
                    AllowedScopes={
                         CustomIdentityServerConstants.gateaway_fullpermission,
                         CustomIdentityServerConstants.basket_fullpermission,
                         CustomIdentityServerConstants.discount_fullpermission,
                           CustomIdentityServerConstants.order_fullpermission,
                           CustomIdentityServerConstants.payment_fullpermission,
                         IdentityServerConstants.StandardScopes.Email,
                         IdentityServerConstants.StandardScopes.OpenId,
                         IdentityServerConstants.StandardScopes.Profile,
                         IdentityServerConstants.StandardScopes.OfflineAccess,
                         "roles",
                      IdentityServerConstants.LocalApi.ScopeName
                     },
                    AccessTokenLifetime=1*60*60,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                    RefreshTokenUsage=TokenUsage.ReUse
                },
            };
    }
}