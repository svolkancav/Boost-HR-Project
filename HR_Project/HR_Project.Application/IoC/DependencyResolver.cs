using Autofac;
using AutoMapper;
using HR_Project.Application.Mapper;
using HR_Project.Application.Services.AbsenceService;
using HR_Project.Application.Services.AdvanceService;
using HR_Project.Application.Services.CityService;
using HR_Project.Application.Services.CompanyService;
using HR_Project.Application.Services.DepartmentService;
using HR_Project.Application.Services.ExpenseService;
using HR_Project.Application.Services.FileService;
using HR_Project.Application.Services.PersonelServices;
using HR_Project.Application.Services.Storage.Azure;
using HR_Project.Common;
using HR_Project.Domain.Entities.Concrete.FileEntities;
using HR_Project.Domain.Repositories;
using HR_Project.Domain.Services.StorageService;
using HR_Project.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Application.IoC
{
    public class DependencyResolver : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PersonelRepository>().As<IPersonelRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PersonnelService>().As<IPersonnelService>().InstancePerLifetimeScope();

            builder.RegisterType<AdvanceRepository>().As<IAdvanceRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AdvanceServise>().As<IAdvanceServise>().InstancePerLifetimeScope();

            builder.RegisterType<AbsenceRepository>().As<IAbsenceRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AbsenceService>().As<IAbsenceService>().InstancePerLifetimeScope();

			builder.RegisterType<CompanyRepository>().As<ICompanyRepository>().InstancePerLifetimeScope();
			builder.RegisterType<CompanyService>().As<ICompanyService>().InstancePerLifetimeScope();

            builder.RegisterType<CityRepository>().As<ICityRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CityService>().As<ICityService>().InstancePerLifetimeScope();

            builder.RegisterType<RegionRepository>().As<IRegionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RegionService>().As<IRegionService>().InstancePerLifetimeScope();

            builder.RegisterType<ExpenseRepository>().As<IExpenseRepository>().InstancePerLifetimeScope();
			builder.RegisterType<ExpenseService>().As<IExpenseService>().InstancePerLifetimeScope();

            builder.RegisterType<DepartmentRepository>().As<IDepartmentRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DepartmentService>().As<IDepartmentService>().InstancePerLifetimeScope();

            builder.RegisterType<EmailService>().As<IEmailService>().InstancePerLifetimeScope();
            builder.RegisterType<TokenService>().As<ITokenService>().InstancePerLifetimeScope();

            builder.RegisterType<ProfileImageService>().As<IProfileImageService>().InstancePerLifetimeScope();

            builder.RegisterType<ExpenseImageService>().As<IExpenseImageService>().InstancePerLifetimeScope();

            builder.RegisterType<AzureStorage>().As<IStorage>().InstancePerLifetimeScope();

            builder.RegisterType<FileRepository<PersonnelPicture>>().As<IFileRepository<PersonnelPicture>>().InstancePerLifetimeScope();
            builder.RegisterType<FileRepository<CostPicture>>().As<IFileRepository<CostPicture>>().InstancePerLifetimeScope();



			#region AutoMapper
			builder.Register(context => new MapperConfiguration(cfg =>
            {
                //Register Mapper Profile
                cfg.AddProfile<Mapping>(); /// AutoMapper klasörünün altına eklediğimiz Mapping classını bağlıyoruz.
            }
            )).AsSelf().SingleInstance();

            builder.Register(c =>
            {
                //This resolves a new context that can be used later.
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            })
            .As<IMapper>()
            .InstancePerLifetimeScope();
            #endregion

            base.Load(builder);
        }
    }
}
