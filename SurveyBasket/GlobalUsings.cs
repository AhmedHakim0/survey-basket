global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using SurveyBasket.Entites;
global using SurveyBasket.Services;

global using FluentValidation;
global using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
global using System.Reflection;
global using Microsoft.EntityFrameworkCore;
global using SurveyBasket.Persistance;
global using SurveyBasket; 
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using SurveyBasket.Contracts.Authentication;

global using Microsoft.IdentityModel.Tokens;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;
global using Microsoft.AspNetCore.Identity;
global using SurveyBasket.Authentication;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using System.ComponentModel.DataAnnotations;


global using System.Security.Cryptography;
global using SurveyBasket.Contracts.Poll;
global using Microsoft.AspNetCore.Authorization;

global using SurveyBasket.Mapping;
global using Microsoft.Extensions.Options;
global using SurveyBasket.Abstractions;
global  using SurveyBasket.Errors;