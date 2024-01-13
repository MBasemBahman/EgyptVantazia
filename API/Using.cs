global using API.ActionFilters;
global using API.Extensions;
global using API.Middlewares;
global using API.Models;
global using API.Resources;
global using API.Utility;
global using AspNetCoreRateLimit;
global using AutoMapper;
global using Contracts;
global using Contracts.Constants;
global using Contracts.Extensions;
global using Contracts.Logger;
global using Contracts.Services;
global using CoreServices;
global using DAL;
global using Entities;
global using Entities.AuthenticationModels;
global using Entities.DBModels.UserModels;
global using Entities.RequestFeatures;
global using Entities.ResponseFeatures;
global using FirebaseAdmin;
global using Google.Apis.Auth.OAuth2;
global using LoggerService;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Localization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Controllers;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Localization;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Any;
global using Microsoft.OpenApi.Models;
global using Newtonsoft.Json.Linq;
global using NLog;
global using Repository;
global using Services;
global using System.Globalization;
global using System.IdentityModel.Tokens.Jwt;
global using System.Linq.Dynamic.Core;
global using System.Reflection;
global using System.Security.Claims;
global using System.Text;
global using TenantConfiguration;
global using static TenantConfiguration.TenantData;
global using Asp.Versioning;
global using static Entities.EnumData.LogicEnumData;






