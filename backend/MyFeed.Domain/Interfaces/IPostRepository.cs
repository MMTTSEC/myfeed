using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyFeed.Domain.Entities;

namespace MyFeed.Domain.Interfaces
{
    public interface IPostRepository
    {
        Task AddAsync(Post post);
    }
}
