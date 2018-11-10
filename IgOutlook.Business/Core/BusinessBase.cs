using Prism.Mvvm;
using System.Linq;

namespace IgOutlook.Business.Core
{
    public class BusinessBase : BindableBase
    {
        public void CopyProperties(object source)
        {
            if (source == null) return;

            foreach (var property in source.GetType().GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(this, property.GetValue(source));
                }
            }
        }

        public void CopyPropertiesTo(object target, params string[] propertiesToSkip)
        {
            if (this == null) return;

            foreach (var property in this.GetType().GetProperties())
            {
                if (propertiesToSkip.Length > 0)
                {
                    if (propertiesToSkip.FirstOrDefault(p => p == property.Name) != null)
                        continue;
                }
                if (property.CanWrite)
                {
                    property.SetValue(target, property.GetValue(this));
                }
            }
        }
       
    }
}
