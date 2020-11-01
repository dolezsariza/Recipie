using Recipie.Domain.Models;
using Recipie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Repositories.TagRepository.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllTags();
        Task<Tag> GetTag(int id);
        Task<bool> ModifyTag(int id, Tag tag);
        Task<bool> AddTag(Tag tag);
        Task<bool> DeleteTag(int id);
        Task<IEnumerable<Recipe>> GetRecipesInTag(int id);
    }
}
