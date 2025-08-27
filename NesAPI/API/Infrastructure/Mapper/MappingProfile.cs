using AutoMapper;
using Entities.Models;
using Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Infrastructure.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<MailDto, MailRequest>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CorrelationId, opt => opt.Ignore())
                .ForMember(dest => dest.Request, opt => opt.Ignore());
            CreateMap<NotificationDto, NotificationRequest>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CorrelationId, opt => opt.Ignore())
                .ForMember(dest => dest.Request, opt => opt.Ignore())
                .ForMember(dest => dest.From, opt => opt.Ignore());
            CreateMap<SmsDto, SmsRequest>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CorrelationId, opt => opt.Ignore())
                .ForMember(dest => dest.Request, opt => opt.Ignore());
            CreateMap<MailAttachmentDto, MailAttachment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MailId, opt => opt.Ignore())
                .ForMember(dest => dest.Mail, opt => opt.Ignore());
            CreateMap<Stat, StatDto>();
        }
    }
}