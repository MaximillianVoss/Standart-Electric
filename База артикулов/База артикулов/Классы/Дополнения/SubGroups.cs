namespace База_артикулов.Модели
{
    public partial class SubGroups
    {
        public SubGroups(Descriptors descriptors, Groups groups, LoadDiagrams loadDiagrams)
        {
            this.Descriptors = descriptors;
            this.Groups = groups;
            this.LoadDiagrams = loadDiagrams;
        }
        public object ToObject()
        {
            return new
            {
                id = this.id,
                title = this.Descriptors.title,
                titleShort = this.Descriptors.titleShort,
                description = this.Descriptors.description,
                code = this.Descriptors.code
            };
        }
    }
}
