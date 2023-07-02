﻿using AutoMapper;
using QueflityMVC.Application.Mapping;
using QueflityMVC.Application.ViewModels.Image;

namespace QueflityMVC.Application.ViewModels.ItemSet
{
    public class ItemSetForListVM : IMapFrom<Domain.Models.Item>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool ShouldBeShown { get; set; }

        public ImageForListVM? Image { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.ItemSet, ItemSetForListVM>()
                .ForMember(vm => vm.Image, opt => opt.MapFrom(ent => ent.Image));
        }
    }
}
