namespace Momento.Services.Implementations.Code
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Momento.Data;
    using Momento.Services.Contracts.Code;
    using Momento.Services.Models.Code;
    using Momento.Models.Codes;
    using Momento.Models.Hashtags;
    using Momento.Models.Contracts;
    using Momento.Models.Hashtags.MappingTables;

    public class CodeService : ICodeService
    {
        private readonly MomentoDbContext context;
        private readonly IMapper mapper;

        public CodeService(MomentoDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void Create(CodeCreate model)
        {
            model.Id = null;
            var dbCode = mapper.Map<Code>(model);
            dbCode.CodeHashtags = ParseCodeHashtags<CodeHashtag>(model.Hashtag);
            foreach (var note in dbCode.Notes)
            {
                note.CodeNoteHashtags = ParseCodeHashtags<CodeNoteHashtag>(note.Hashtags);
            }

            context.Code.Add(dbCode);
            context.SaveChanges();
        }

        public T[] ParseCodeHashtags<T>(string hashString) where T: HashtagMappingClass, new()
        {
            var hashtags = hashString.Split('#',' ', System.StringSplitOptions.RemoveEmptyEntries);
            var result = new List<T>();

            foreach (var tagName in hashtags)
            {
                var dbTag = context.Hashtags.SingleOrDefault(x => x.Name == tagName);

                if(dbTag == null)
                {
                    dbTag = new Hashtag {
                        Name = tagName,
                    };
                }

                result.Add(new T {
                    Hashtag = dbTag,
                });
            }

            return result.ToArray();
        }
    }
}
