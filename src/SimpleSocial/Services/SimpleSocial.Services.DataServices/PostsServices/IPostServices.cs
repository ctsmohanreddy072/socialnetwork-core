﻿using System.Threading.Tasks;
using SimpleSocia.Services.Models.Account;

namespace SimpleSocial.Services.DataServices.PostsServices
{
    public interface IPostServices
    {
        Task CreatePostAsync(MyProfileViewModel viewModel);
    }
}