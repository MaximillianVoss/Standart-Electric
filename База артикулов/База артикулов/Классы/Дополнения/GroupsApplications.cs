using System;

namespace База_артикулов.Модели
{
    public partial class GroupsApplications : IToObject
    {
        public GroupsApplications(SubGroups subGroup, Applications application)
        {
            this.SubGroups = subGroup;
            this.Applications = application;
        }
        public GroupsApplications() : this(null, null)
        {

        }
        public object ToObject()
        {
            return new
            {
                id = this.id,
                idApplication = this.Applications.id,
                idSubGroup = this.SubGroups.id,
                title = String.Format("{0} {1}", this.Applications.Descriptors.title, this.SubGroups.Descriptors.title)
            };
        }
    }
}
